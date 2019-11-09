using System;
using System.Text;
using System.Data;
using System.Collections;

using System.IO;

using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using DBViewer.Common;

namespace DBViewer.Gui
{
	/// <summary>
	/// DatabaseDataGrid - Represents the underline DataTable. 
	/// In addition to the basic DataGrid features represents columns with a binary data (varbinary, binary, image) by their basic binary representations.
	/// Image - represents a small thumbnail picture and lets the user to view/load another image into the cell.
	/// Varbinar and binary - represents an editable string which is translated to its binary represenation when the edit is finished.
	/// </summary>
	public class DatabaseDataGrid : DataGrid
	{

		#region Private Constants.
		/// <summary>
		/// Delimites the type of the underline DataTable's columns.
		/// </summary>
		private const char TypeDelimiter = '#';
		/// <summary>
		/// Images tooltip.
		/// </summary>
		private const string ImageToolTipText = "To view / load an image double click on it...";
		/// <summary>
		/// Images tooltip delay
		/// </summary>
		private const int ImageToolTipAutoPopDelay = 2000;
		/// <summary>
		/// Constant for KyeDown event.
		/// </summary>
		private const int WM_KEYDOWN = 0x100;
		#endregion Private Constants.

		#region Private Memebers.
		/// <summary>
		/// current selected row in the grid.
		/// </summary>
		private int tooltipSelectedRow;
		/// <summary>
		/// current selected column in the grid.
		/// </summary>
		private int tooltipSelectedColumn;
		/// <summary>
		/// previous selected DataGrid row in the grid.
		/// </summary>
		private int selectedDataGridRow;
		/// <summary>
		/// last selected DataGrid type: cell, row and etc ...
		/// </summary>
		private HitTestType selectedType;
		/// <summary>
		/// images tooltip.
		/// </summary>
		private ToolTip imageToolTip;
		/// <summary>
		/// deleted rows.
		/// </summary>
		private ArrayList deletedRows;
		#endregion Private Memebers.
		
		#region Private Default Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		private DatabaseDataGrid() : base()
		{
			
		}
		#endregion Private Default Constructor.

		#region Private Methods.
		/// <summary>
		/// Constructs the database info from a given string.
		/// The string is supposed to be in the following format:
		/// x.x.x.x.database_name.table_name (server == ip)
		/// or
		/// server_name.database_name.table_name (server == string)
		/// </summary>
		/// <param name="info">see the summary</param>
		/// <returns>server, database and table names</returns>
		private string[] GetDatabaseInfo(string info)
		{
			try
			{ 
				// if server name is used then just return the splitted answer.
				string[] databaseInfo = info.Split(new char[]{'.'});
				if(databaseInfo.Length == 3)
					return databaseInfo;
					// else the extented server name is expected (i.e. using ips), then just construct the answer.
				else
				{
					int infoLength = databaseInfo.Length;

					string[] fullDatabaseInfo = new string[3];
					// constructs the server name from its ips.
					StringBuilder server = new StringBuilder();
					for(int i=0; i<infoLength-2; i++)
					{
						server.Append(databaseInfo[i]);
						if(i != infoLength-3)
							server.Append(".");
					}
					
					fullDatabaseInfo[0] = server.ToString();
					fullDatabaseInfo[1] = databaseInfo[infoLength-2];
					fullDatabaseInfo[2] = databaseInfo[infoLength-1];

					return fullDatabaseInfo;	
				}
			}
			catch (Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;
			}
		}
		/// <summary>
		/// Imitates the Ctrl+S on the DataGrid.
		/// </summary>
		/// <remarks>This event will fire only on DataGridTextBoxColumn</remarks>
		/// <param name="sender">DataGridTextBoxColumn</param>
		/// <param name="e">events args</param>
		private void TextBoxKeyDownEventHandler(object sender, KeyEventArgs e)
		{
			//Ctrl+S
			if((e.KeyCode == Keys.S) && e.Control)
			{
				// immitates the "Enter" key.
				this.OnKeyDown(new KeyEventArgs(Keys.Enter));
				this.OnKeyDown(e);	
			}
		}
		/// <summary>
		/// Registers the KeyDown EventHandler for DataGridTextBoxColumn;
		/// </summary>
		/// <param name="columnStyle"></param>
		private void EnableSave(DataGridTextBoxColumn columnStyle)
		{
			columnStyle.TextBox.KeyDown+=new KeyEventHandler(TextBoxKeyDownEventHandler);
		}
		/// <summary>
		/// Sets the DataGridColumnStyle's properties and adds it the given DataGridTableStyle.
		/// </summary>
		/// <param name="tableStyle">table style</param>
		/// <param name="columnStyle">column style</param>
		/// <param name="column">underline DataSource column</param>
		private void SetDataTableColumnStyleProperties(DataGridTableStyle tableStyle, DataGridColumnStyle columnStyle, DataColumn column)
		{
			// sets the column mapping name and header text.
			columnStyle.MappingName = column.ColumnName;					
			columnStyle.HeaderText = column.ColumnName;
			// adds the grid column style to the table column style
			tableStyle.GridColumnStyles.Add(columnStyle);
		}
		/// <summary>
		/// Inits the DataGridTableStyle according to the types of the columns.
		/// </summary>
		/// <param name="tableStyle">table style</param>
		/// <param name="column">underline DataSource column</param>
		private void InitDataTableColumnAndStyle(DataGridTableStyle tableStyle, DataColumn column)
		{
			//if column's name starts with # - then user DataGridColumnStyle should be used.
			if(column.ColumnName[0] == TypeDelimiter)
			{
				string[] columnType = column.ColumnName.Split(TypeDelimiter);	
				if(columnType[1] == DBViewerConstants.ImageColumnType)
				{
					DataGridImageColumn imageColumn = new DataGridImageColumn();
					column.ColumnName = columnType[2];
					SetDataTableColumnStyleProperties(tableStyle, imageColumn, column);
				}
				else if(columnType[1] == DBViewerConstants.BinaryColumnType)
				{
					DataGridBinaryColumn binaryColumn = new DataGridBinaryColumn();
					column.ColumnName = columnType[2];
					SetDataTableColumnStyleProperties(tableStyle, binaryColumn, column);
					EnableSave(binaryColumn);
				}
			}
			else
			{
				DataGridTextBoxColumn textBoxColumn = new DataGridTextBoxColumn();
				SetDataTableColumnStyleProperties(tableStyle, textBoxColumn, column);
				EnableSave(textBoxColumn);
			}

		}
		/// <summary>
		/// Extracts image from underline DataTable.
		/// Returns null if image can't be converted.
		/// </summary>
		/// <param name="row">row</param>
		/// <param name="column">column</param>
		/// <returns>Image from given row &amp; column or null</returns>
		private Image ExtractImageFromDataSource(int row, int column)
		{
			Image image = null;
			try
			{
				byte[] imageContent = ((DataTable)this.DataSource).Rows[row][column] as byte[];
				if(imageContent != null)
					image = Image.FromStream(new MemoryStream(imageContent));
				return image;
			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				return null;
			}
		}
		/// <summary>
		/// Saves image to the underline DataTable.
		/// </summary>
		/// <param name="image">image</param>
		/// <param name="row">row</param>
		/// <param name="column">column</param>
		private void SaveImageToDataSource(Image image, int row, int column)
		{
			MemoryStream stream = new MemoryStream();
			image.Save(stream, ImageFormat.Bmp);
			((DataTable)this.DataSource).Rows[row][column] = stream.GetBuffer();
		}
		/// <summary>
		/// Removes the i-th row from the data soruce.
		/// </summary>
		/// <param name="i"></param>
		private void RemoveDeletedRowFromDataSource(int i)
		{
			// places the deleted row in temporary holder.
			DataTable table = (DataTable)this.DataSource;
			if(i < table.Rows.Count)
			{
				DataRow row = table.Rows[i];
				deletedRows.Add(row.ItemArray);	
				table.Rows.Remove(row);
			}
		}
		/// <summary>
		/// If exists any deleted rows adds them to the table.
		/// </summary>
		/// <returns>DataSource</returns>
		private DataTable RestoreDeletedRowsInDataSource()
		{
			DataTable table = (DataTable)this.DataSource;

			if(deletedRows.Count > 0)
			{
				foreach(object[] rowItemArray in deletedRows)
				{
					table.Rows.Add(rowItemArray);
					
					int rowIndex = table.Rows.Count -1;
					table.Rows[rowIndex].AcceptChanges();
					table.Rows[rowIndex].Delete();
				}
			}
			return table;
		}
		#endregion  Private Methods.

		#region Public Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="table">DataSource's table</param>
		public DatabaseDataGrid(DataTable table)
		{

			this.BeginInit();
			this.SuspendLayout();

			tooltipSelectedRow = -1;
			tooltipSelectedColumn = -1;
			selectedDataGridRow = -1;
			
			selectedType = HitTestType.None;

			deletedRows = new ArrayList();

			// creates DataGridTableStyle.
			DataGridTableStyle tableStyle = new DataGridTableStyle();
			tableStyle.MappingName = table.TableName;

			// creates DataGridColumnStyles.
			foreach(DataColumn column in table.Columns)
				InitDataTableColumnAndStyle(tableStyle, column);

			// registers new DataGridTableStyle.
			TableStyles.Clear();
			TableStyles.Add(tableStyle);
			
			// bounds the DataSource.
			DataSource = table;
			
			tableStyle.AlternatingBackColor = System.Drawing.Color.White;
			
			tableStyle.BackColor = System.Drawing.Color.White;
			tableStyle.ForeColor = System.Drawing.Color.Black;
			
			tableStyle.GridLineColor = System.Drawing.Color.Silver;
			
			tableStyle.HeaderBackColor = System.Drawing.Color.DarkSlateBlue;
			tableStyle.HeaderFont = new System.Drawing.Font("Tahoma", 8F);
			tableStyle.HeaderForeColor = System.Drawing.Color.White;
			
			tableStyle.LinkColor = System.Drawing.Color.Purple;

			tableStyle.SelectionBackColor = System.Drawing.Color.Beige;
			tableStyle.SelectionForeColor = System.Drawing.Color.Brown;

			// sets the tool tip.
			imageToolTip = new ToolTip();
			imageToolTip.SetToolTip(this, ImageToolTipText);
			imageToolTip.AutoPopDelay = ImageToolTipAutoPopDelay;
			
			this.EndInit();			
			this.ResumeLayout(false);
			
		}
		#endregion Public Constructor.

		#region Protected Overrided Methods.
		/// <summary>
		/// OnMouseMove
		/// </summary>
		/// <remarks>
		///		shows tolltip for image columns.
		/// </remarks>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			// finds the point on the client side.
			HitTestInfo info = HitTest(e.X,  e.Y);
			if(info.Type == HitTestType.Cell && (info.Row != tooltipSelectedRow || info.Column != tooltipSelectedColumn))
			{
				
				DataGridImageColumn imageColumn = this.TableStyles[0].GridColumnStyles[info.Column] as DataGridImageColumn;
				if(imageColumn != null)
				{
					// forse redraw;
					imageToolTip.Active = false;
					imageToolTip.Active = true;
				}
				else
					imageToolTip.Active = false;

				tooltipSelectedRow = info.Row;
				tooltipSelectedColumn = info.Column;
			}
			
			base.OnMouseMove (e);
		}
		/// <summary>
		/// OnMouseDown
		/// </summary>
		/// <remarks>
		///		if the RowHeader hitted - unselect previouslySelectedDataGridRow and selects the new one. 
		/// </remarks>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// finds the point on the client side.
			HitTestInfo info = this.HitTest(e.X,  e.Y);
			
			// saves the last selected type.
			selectedType = info.Type;

			if(info.Type == HitTestType.RowHeader)
			{
				if(selectedDataGridRow != -1)
					this.UnSelect(selectedDataGridRow);
			
				selectedDataGridRow = info.Row;
				this.Select(selectedDataGridRow);	
			}
		
			else
				base.OnMouseDown (e);
		}
		/// <summary>
		/// OnDoubleClick
		/// </summary>
		/// <remarks>
		///		1. Shows the image from the DataSource in the <see cref="ImageViewer"/>.
		///		2. If the ImageViewer requests to save the loaded image, saves it the DataSource.
		/// </remarks>
		/// <param name="e"></param>
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);
			// finds the point on the client side.
			Point pt = this.PointToClient(Cursor.Position); 
			// test the point
			HitTestInfo info = this.HitTest(pt);
			if(info.Type == HitTestType.Cell)
			{
				// if it is DataGridImageColumn (column that represents image)
				DataGridImageColumn style = this.TableStyles[0].GridColumnStyles[info.Column] as DataGridImageColumn;
				if(style != null && info.Row < ((DataTable)this.DataSource).Rows.Count)
				{
					ImageViewer imageViewer = new ImageViewer();
					imageViewer.Image = ExtractImageFromDataSource(info.Row, info.Column);
					if(imageViewer.ShowDialog(this) == DialogResult.OK)
						SaveImageToDataSource(imageViewer.Image, info.Row, info.Column);
					imageViewer.Dispose();
				}
			}
			
		}
		/// <summary>
		/// IsInputKey
		/// </summary>
		/// <remarks>
		///		Marks Ctrl+C combination as InputKey in order to catch its event later.
		/// </remarks>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool IsInputKey(Keys keyData)
		{
			if(keyData == ( Keys.Control | Keys.C))
				return true;
			
			return base.IsInputKey (keyData);
		}	
		/// <summary>
		/// OnKeyDown
		/// </summary>
		/// <remarks>
		///		1. on Ctrl+C copies the DataRow into the Clipboard object.
		///		2. on Ctrl+V pasts the data from the Clipboard object into the DataSource = DataTable.
		/// </remarks>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			// if Ctrl+C
			if(e.KeyData == (Keys.C | Keys.Control))
			{
				DataTable table = (DataTable)this.DataSource;
				if(selectedDataGridRow < table.Rows.Count)
				{
					// saves the DataRow's data under the name of the DatabaseDataGrid class.
					DataFormats.Format format = DataFormats.GetFormat(this.ToString());

					// copies the data to the clipboard.
					IDataObject data = new DataObject();

					DataRow row = table.Rows[selectedDataGridRow];
					data.SetData(format.Name, false, row.ItemArray);
					Clipboard.SetDataObject(data, false);	
				}
			}
			// else if Ctrl+V
			else if(e.KeyData == (Keys.V | Keys.Control))
			{
				// retrieves the data from the clipboard
				IDataObject data = Clipboard.GetDataObject();
				string format = this.ToString();
				
				if(data.GetDataPresent(format))
				{
				
					object[] row = data.GetData(format) as object[];
					//adds new row to the underline DataSoruce - DataTable if needed.
					DataTable table = (DataTable)this.DataSource;
					if(table.Rows.Count < (selectedDataGridRow + 1))
						table.Rows.Add(row);
					else
						table.Rows[selectedDataGridRow].ItemArray = row;
				}
			
			}
			// else if Ctrl+S
			else if(e.KeyData == (Keys.S | Keys.Control))
			{
				SaveDatabaseTable();
			}
			// else
			else
				base.OnKeyDown (e);
		}
		/// <summary>
		/// Processes the "delete" key
		/// </summary>
		/// <param name="msg">message</param>
		/// <param name="keyData">key data</param>
		/// <returns>returns base.ProcessCmdKey</returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(msg.Msg==WM_KEYDOWN)
			{
				if(keyData == Keys.Delete)
				{
					// finds the point on the client side.
					Point pt = this.PointToClient(Cursor.Position); 
					// test the point
					HitTestInfo info = this.HitTest(pt);
					if(info.Type == HitTestType.RowHeader)
					{
						RemoveDeletedRowFromDataSource(info.Row);
					}
				}
			}
				
			return base.ProcessCmdKey (ref msg, keyData);
		}
		#endregion  Protected Overrided Methods.

		#region Public Methods.
		/// <summary>
		/// True whether table posses Modified, Deleted and Added rows.
		/// </summary>
		/// <returns>True whether table posses Modified, Deleted and Added rows.</returns>
		public bool IsSaveRequired()
		{
			if(deletedRows.Count > 0)
				return true;

			DataTable table = (DataTable)this.DataSource;
			foreach(DataRow row in table.Rows)
			{
				if(row.RowState == DataRowState.Modified || row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Added)
					return true;
			}
			return false;
		}
		/// <summary>
		/// Saves database table.
		/// </summary>
		public void SaveDatabaseTable()
		{
			// gets the db info (server, database, table).
			string[] databaseInfo = GetDatabaseInfo(this.Parent.Text);
			// adds the deleted rows to the table.
			DataTable table = RestoreDeletedRowsInDataSource();
			DBViewerGui.Instance().SaveDatabaseTable(databaseInfo[0], databaseInfo[1], databaseInfo[2], table);
			//deleted data reseted.
			deletedRows.Clear();
			//saved data marked as unchanged.
			table.AcceptChanges();
		}
		/// <summary>
		/// Activates DatabaseDataGrid Copy
		/// </summary>
		public void CopyRequested()
		{
			if(selectedType == HitTestType.Cell)
				SendKeys.Send("^(C)");
			if(selectedType == HitTestType.RowHeader)
				OnKeyDown(new KeyEventArgs(Keys.C | Keys.Control));
		}
		/// <summary>
		/// Activates DatabaseDataGrid Paste
		/// </summary>
		public void PasteRequested()
		{
			if(selectedType == HitTestType.Cell)
				SendKeys.Send("^(V)");
			if(selectedType == HitTestType.RowHeader)
				OnKeyDown(new KeyEventArgs(Keys.V | Keys.Control));
		}
		/// <summary>
		/// Activates DatabaseDataGrid Save
		/// </summary>
		public void SaveRequested()
		{
			if(selectedType == HitTestType.Cell)
				TextBoxKeyDownEventHandler(this, new KeyEventArgs(Keys.S | Keys.Control));
			else
				SaveDatabaseTable();				
		}

		#endregion Public Methods.
	}
	/// <summary>
	/// DataGridImageColumn - Column style for image column types. <see cref="DatabaseDataGrid"/>
	/// </summary>
	public class DataGridImageColumn : DataGridColumnStyle
	{
		#region Private Constants.
		/// <summary>
		/// Height of the cell.
		/// </summary>
		private const int height = 11;
		/// <summary>
		/// Width of the cell.
		/// </summary>
		private const int width = 55;
		/// <summary>
		/// Thumbnail image size.
		/// </summary>
		private const int ThumbnailSize = 200;
		#endregion Private Constants.

		#region Private Member.
		/// <summary>
		/// default image (when the cell is NULL)
		/// </summary>
		private Image defaultImage;
		/// <summary>
		/// default image for unknown format.
		/// </summary>
		private Image unknownImage;
		#endregion Private Member.
		
		#region Private Methods.
		/// <summary>
		/// A callback method - returns always true.
		/// </summary>
		/// <returns>true</returns>
		private bool ThumbnailCallback()
		{
			return true;
		}
		/// <summary>
		/// Paints the image in the cell.
		/// </summary>
		/// <param name="g">graphics</param>
		/// <param name="bounds">rectangle of the cell</param>
		/// <param name="manager">CurrencyManager of the DataGrid</param>
		/// <param name="rowNum">Row of the DataGrid</param>
		private void PaintImage(Graphics g, Rectangle bounds, CurrencyManager manager, int rowNum)
		{
			SolidBrush backBrush = new SolidBrush(Color.White);
			
			try
			{
				// thumbnail image from the cell's image.
				Image thumbnailImage;
				// gets the img from the DataSource.
				byte[] img = (GetColumnValueAtRow(manager, rowNum) as byte[]);
				// if no image in the current cell - displays the default image.
				if(img == null)
				{
					thumbnailImage = defaultImage;
				}
				else
				{
					Image cellImage = Image.FromStream(new MemoryStream(img));
					// creates thumbnail image from cell's image with default size : thumbnailSize
					thumbnailImage = cellImage.GetThumbnailImage(ThumbnailSize , ThumbnailSize, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);	
				}	

				g.FillRectangle(backBrush, bounds);
				g.DrawImage(thumbnailImage, bounds);
			}
			catch(ArgumentException e)
			{
				g.FillRectangle(backBrush, bounds);
				g.DrawImage(unknownImage, bounds);

				Log.WriteErrorToLog(e.Message);

			}
			catch(Exception e)
			{
                Log.WriteErrorToLog(e.Message);
				throw;
				
			}
			
		}
		#endregion Private Methods.
		
		#region Public Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		public DataGridImageColumn() : base()
		{
			//reads the bitmap for default image & unknown type from file.
			defaultImage = new Bitmap(this.GetType(), "noimage.bmp");
			unknownImage = new Bitmap(this.GetType(), "unknown.bmp");
		}
		#endregion Public Constructor.

		#region DataGridColumnStyle Methods.
		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected override Size GetPreferredSize(Graphics g, object value)
		{
			return new Size(width, height);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override int GetMinimumHeight()
		{
			return height;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected override int GetPreferredHeight(Graphics g, object value)
		{
			return height;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rowNum"></param>
		protected override void Abort(int rowNum)
		{
			// readonly.
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataSource"></param>
		/// <param name="rowNum"></param>
		/// <returns></returns>
		protected override bool Commit(CurrencyManager dataSource, int rowNum)
		{
			// readonly.
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="rowNum"></param>
		/// <param name="bounds"></param>
		/// <param name="readOnly"></param>
		/// <param name="instantText"></param>
		/// <param name="cellIsVisible"></param>
		protected override void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
		{
			//readonly
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <param name="source"></param>
		/// <param name="rowNum"></param>
		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum)
		{
			PaintImage(g, bounds, source, rowNum);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <param name="source"></param>
		/// <param name="rowNum"></param>
		/// <param name="alignToRight"></param>
		protected override void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight)
		{
			PaintImage(g, bounds, source, rowNum);
		}
		#endregion DataGridColumnStyle Methods.
	}
	/// <summary>
	/// DataGridBinaryColumn - Column style for binary column types. <see cref="DatabaseDataGrid"/>
	/// </summary>
	public class DataGridBinaryColumn : DataGridTextBoxColumn
	{
		#region Public Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		public DataGridBinaryColumn() : base()
		{
			
		}
		#endregion Public Constructor.

		#region DataGridColumnStyle Methods.
		/// <summary>
		/// Sets the column value as byte[].
		/// </summary>
		/// <param name="source"></param>
		/// <param name="rowNum"></param>
		/// <param name="value"></param>
		protected override void SetColumnValueAtRow(CurrencyManager source, int rowNum, object value)
		{
			
			// converts the value to string.
			string strValue = value.ToString();
			
			// constructs the binary data from the value.
			byte[] data = new byte[strValue.Length];
			for(int i=0; i < strValue.Length; i++)
				data[i] = Convert.ToByte(strValue[i]);
			
			// saves the data.
			base.SetColumnValueAtRow (source, rowNum, data);
		
		}
	
		/// <summary>
		/// Gets the column value as byte[]
		/// </summary>
		/// <param name="source"></param>
		/// <param name="rowNum"></param>
		/// <returns></returns>
		protected override object GetColumnValueAtRow(CurrencyManager source, int rowNum)
		{
			// gets the binary data if available
			object value = base.GetColumnValueAtRow(source, rowNum);
			// converts the data to binary if possible.
			byte[] data = value as byte[];
			// if the conversion failed then returns the base value.
			if(data == null)
				return value;
			// else
			else
			{
				// constructs binary data representation from the value.
				StringBuilder binaryRepresentation = new StringBuilder();
				int i=0;
				while(i < data.Length)
					binaryRepresentation.Append(data[i++]);
				
				return binaryRepresentation.ToString();
			}
		}
		#endregion DataGridColumnStyle Methods.

	}
}

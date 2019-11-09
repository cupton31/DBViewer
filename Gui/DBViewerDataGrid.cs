using System.Data;

using System.Drawing;
using System.Windows.Forms;

using DBViewer.Common;

namespace DBViewer.Gui
{
	/// <summary>
	/// DBViewerDataGrid - a form containing the DataGrid for DataTable.
	/// </summary>
	public class DBViewerDataGrid : Form
	{
		#region Private Member.
		/// <summary>
		/// Custom DataGrid.
		/// </summary>
		private DatabaseDataGrid data;
		/// <summary>
		/// Main Menu.
		/// </summary>
		private MainMenu dbDataGridMainMenu;
		/// <summary>
		/// File Menu.
		/// </summary>
		private MenuItem fileMenuItem;
		/// <summary>
		/// Save.
		/// </summary>
		private MenuItem saveMenuItem;
		/// <summary>
		/// Edit Menu.
		/// </summary>
		private MenuItem editMenuItem;
		/// <summary>
		/// Copy.
		/// </summary>
		private MenuItem copyMenuItem;
		/// <summary>
		/// Paste.
		/// </summary>
		private MenuItem pasteMenuItem;
		#endregion Private Member.

		#region Private Default Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		private DBViewerDataGrid()
		{
		}
		#endregion Private Default Constructor.

		#region Private Methods.
		/// <summary>
		/// Save
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveMenuItemEventHandler(object sender, System.EventArgs e)
		{
			data.SaveRequested();
		}
		/// <summary>
		/// Copy
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CopyMenuItemEventHandler(object sender, System.EventArgs e)
		{
			data.CopyRequested();
		}
		/// <summary>
		/// Paste
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PasteMenuItemEventHandler(object sender, System.EventArgs e)
		{
			data.PasteRequested();
		}
		/// <summary>
		/// Sets the Last Column's size to fit the DBViewerDataGrid's Parent.
		/// </summary>
		private void SetLastColumnSize()
		{
			int columns = ((DataTable)data.DataSource).Columns.Count; 
			
			int targetWidth = this.Parent.ClientSize.Width;
			int runningWidthUsed = data.TableStyles[0].RowHeaderWidth; 
			for(int i = 0; i < columns - 1; i++) 
				runningWidthUsed += data.TableStyles[0].GridColumnStyles[i].Width;
			if(runningWidthUsed < targetWidth) 
				//the fudge - 4 is for the grid's border 
				data.TableStyles[0].GridColumnStyles[columns - 1].Width = (targetWidth - runningWidthUsed - 4); 
		}
		/// <summary>
		/// Initializes the main menu
		/// </summary>
		private void InitializeMenu()
		{
			dbDataGridMainMenu = new MainMenu();
			fileMenuItem = new MenuItem();
			saveMenuItem = new MenuItem();
			editMenuItem = new MenuItem();
			copyMenuItem = new MenuItem();
			pasteMenuItem = new MenuItem();
			// 
			// dbDataGridMainMenu
			// 
			this.dbDataGridMainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							   this.fileMenuItem,
																							   this.editMenuItem});
			// 
			// fileMenuItem
			// 
			this.fileMenuItem.Index = 0;
			this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.saveMenuItem});
			this.fileMenuItem.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
			this.fileMenuItem.Text = "&File";
			// 
			// saveMenuItem
			// 
			this.saveMenuItem.Index = 0;
			this.saveMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.saveMenuItem.Text = "&Save";
			this.saveMenuItem.Click+=new System.EventHandler(SaveMenuItemEventHandler);
			// 
			// editMenuItem
			// 
			this.editMenuItem.Index = 1;
			this.editMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.copyMenuItem,
																						 this.pasteMenuItem});
			this.editMenuItem.MergeOrder = 1;
			this.editMenuItem.Text = "&Edit";
			// 
			// copyMenuItem
			// 
			this.copyMenuItem.Index = 0;
			this.copyMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.copyMenuItem.Text = "&Copy";
			this.copyMenuItem.Click += new System.EventHandler(this.CopyMenuItemEventHandler);
			// 
			// pasteMenuItem
			// 
			this.pasteMenuItem.Index = 1;
			this.pasteMenuItem.MergeOrder = 1;
			this.pasteMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.pasteMenuItem.Text = "&Paste";
			this.pasteMenuItem.Click += new System.EventHandler(this.PasteMenuItemEventHandler);
			// 
			// DBViewerDataGrid
			// 
			this.Menu = this.dbDataGridMainMenu;
			
		}
		#endregion Private Methods.

		#region Protected Overrided Methods.
		/// <summary>
		/// Controls resizing and DataGrid's last column size <see cref="SetLastColumnSize"/>
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize (e);
			SetLastColumnSize();
		}
		/// <summary>
		/// Checks whether saving is needed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			if(data.IsSaveRequired())
			{
				if(MessageBox.Show("   Save before exit ?\t\t", DBViewerConstants.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					data.SaveDatabaseTable();
			}
			base.OnClosing (e);
		}
		#endregion Protected Overrided Methods.
		
		#region Public Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="table"></param>
		public DBViewerDataGrid(DataTable table) : this()
		{
			this.SuspendLayout();
			string[] databaseInfo = table.TableName.Split(new char[]{'.'});

			// default properties for the DBViewerDataGrid Form.
			this.Text = table.TableName;
			this.Icon = new Icon(this.GetType(),"DBViewer.ico");
			this.WindowState = FormWindowState.Maximized;
		
			// creates database data grid.
			table.TableName = databaseInfo[databaseInfo.Length-1];
			data = new DatabaseDataGrid(table);
			data.Parent = this;
			data.Dock = DockStyle.Fill;
            if (DBViewerConstants.AdminMode)
            {
                foreach (DBViewerConstants.TableRule TableRule in DBViewerConstants.AdminTables)
                {
                    if (table.TableName == TableRule.TableName)
                    {
                        if (TableRule.ReadOnly == true)
                        {
                            data.ReadOnly = true;
                        }
                        else
                        {
                            data.ReadOnly = false;
                        }
                    }
                }
            }
            else
            {
                foreach (DBViewerConstants.TableRule TableRule in DBViewerConstants.UserTables)
                {
                    if (table.TableName == TableRule.TableName)
                    {
                        if (TableRule.ReadOnly == true)
                        {
                            data.ReadOnly = true;
                        }
                        else
                        {
                            data.ReadOnly = false;
                        }
                    }
                }
            }
			
			// initializes the main menu.
			InitializeMenu();

            // Fixes bug: opening a tableview form ruins z-order
            DBViewerGui.instance.BringToFront();

			this.ResumeLayout();
		}

        #endregion Public Constructor.

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DBViewerDataGrid
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "DBViewerDataGrid";
            this.TopMost = true;
            this.ResumeLayout(false);

        }
    }

}

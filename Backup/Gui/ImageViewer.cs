using System;
using System.IO;

using System.Drawing;
using System.Windows.Forms;

using DBViewer.Common;

namespace DBViewer.Gui
{
	#region LoadImageDelegate & ShowLoadImageProgressDelegate.
	/// <summary>
	/// LoadImageDelagate - responsible to load the image from the disk.
	/// </summary>
	delegate void LoadImageDelegate();
	/// <summary>
	/// ShowLoadImageProgressDelagate - responsible to show the progress of the LoadImageDelegate.
	/// </summary>
	delegate void ShowLoadImageProgressDelagate(int percentage);
	#endregion LoadImageDelegate & ShowLoadImageProgressDelegate.

	/// <summary>
	/// ImageViewer - graphically represents an image.
	/// </summary>
	public class ImageViewer : System.Windows.Forms.Form
	{

		#region Private Constants.
		/// <summary>
		/// Indicates the progress of the image load.
		/// </summary>
		private const int ImageProgressPercentage = 10;
		#endregion Private Constants.

		#region Private Members.
		/// <summary>
		/// Cancel image.
		/// </summary>
		private Button cancelImage;
		/// <summary>
		/// Accept image.
		/// </summary>
		private Button acceptImage;
		/// <summary>
		/// Load image.
		/// </summary>
		private Button loadImage;
		/// <summary>
		/// Load image progress.
		/// </summary>
		private ProgressBar loadedImageProgressBar;
		/// <summary>
		/// Load image file dialog.
		/// </summary>
		private OpenFileDialog loadedImageFileDialog;
		/// <summary>
		/// Panel that hold's the image.
		/// </summary>
		private Panel imagePanel;
		/// <summary>
		/// Image.
		/// </summary>
		private PictureBox image;
		#region Private Member - Windows Designer.
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion Private Member - Windows Designer.
		#endregion Private Members.

		#region Private Methods.
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ImageViewer));
			this.cancelImage = new System.Windows.Forms.Button();
			this.acceptImage = new System.Windows.Forms.Button();
			this.loadedImageProgressBar = new System.Windows.Forms.ProgressBar();
			this.loadImage = new System.Windows.Forms.Button();
			this.loadedImageFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.imagePanel = new System.Windows.Forms.Panel();
			this.image = new System.Windows.Forms.PictureBox();
			this.imagePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// cancelImage
			// 
			this.cancelImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelImage.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelImage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelImage.Location = new System.Drawing.Point(400, 488);
			this.cancelImage.Name = "cancelImage";
			this.cancelImage.TabIndex = 4;
			this.cancelImage.Text = "Cancel";
			// 
			// acceptImage
			// 
			this.acceptImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.acceptImage.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.acceptImage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.acceptImage.Location = new System.Drawing.Point(8, 488);
			this.acceptImage.Name = "acceptImage";
			this.acceptImage.TabIndex = 3;
			this.acceptImage.Text = "OK";
			// 
			// loadedImageProgressBar
			// 
			this.loadedImageProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.loadedImageProgressBar.Location = new System.Drawing.Point(80, 456);
			this.loadedImageProgressBar.Name = "loadedImageProgressBar";
			this.loadedImageProgressBar.Size = new System.Drawing.Size(224, 16);
			this.loadedImageProgressBar.Step = 1;
			this.loadedImageProgressBar.TabIndex = 1;
			// 
			// loadImage
			// 
			this.loadImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.loadImage.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.loadImage.Location = new System.Drawing.Point(312, 448);
			this.loadImage.Name = "loadImage";
			this.loadImage.Size = new System.Drawing.Size(75, 32);
			this.loadImage.TabIndex = 2;
			this.loadImage.Text = "Load Image";
			this.loadImage.Click += new System.EventHandler(this.LoadImageClickEventHandler);
			// 
			// loadedImageFileDialog
			// 
			this.loadedImageFileDialog.Filter = "bmp (*.bmp)|*.bmp|jpg (*.jpg)|*.jpg|All files (*.*)|*.*";
			// 
			// imagePanel
			// 
			this.imagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.imagePanel.AutoScroll = true;
			this.imagePanel.Controls.Add(this.image);
			this.imagePanel.Location = new System.Drawing.Point(8, 8);
			this.imagePanel.Name = "imagePanel";
			this.imagePanel.Size = new System.Drawing.Size(472, 424);
			this.imagePanel.TabIndex = 0;
			// 
			// image
			// 
			this.image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.image.Location = new System.Drawing.Point(8, 8);
			this.image.Name = "image";
			this.image.Size = new System.Drawing.Size(456, 408);
			this.image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.image.TabIndex = 0;
			this.image.TabStop = false;
			// 
			// ImageViewer
			// 
			this.AcceptButton = this.acceptImage;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelImage;
			this.ClientSize = new System.Drawing.Size(488, 518);
			this.Controls.Add(this.imagePanel);
			this.Controls.Add(this.loadImage);
			this.Controls.Add(this.loadedImageProgressBar);
			this.Controls.Add(this.acceptImage);
			this.Controls.Add(this.cancelImage);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(300, 200);
			this.Name = "ImageViewer";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ImageViewer";
			this.imagePanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Load Image Process.
		/// <summary>
		/// True if the load image progress is required.
		/// </summary>
		/// <param name="percentage">current percentage of the load process</param>
		/// <param name="lastPercentage">last percentage of the load process</param>
		/// <returns>True if the load image progress is required.</returns>
		private bool IsLoadImageProgressUpdateRequired(int percentage, int lastPercentage)
		{
			if(percentage != 0 && percentage % ImageProgressPercentage == 0)
				if(percentage != lastPercentage)
					return true;
					
			return false;
		}
		/// <summary>
		/// Updates the PictrueBox's progress bar.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		///		<item>Advances the ProgressBar by <see cref="ImageProgressPercentage"/></item>
		///		<item>LastLoadPercentage is saved after each update in order to avoid refreshing the UI too many times.</item>
		/// </list>
		/// </remarks>
		/// <param name="currentPart">current image's part</param>
		/// <param name="totalParts">total image's parts</param>
		/// <param name="lastLoadPercentage">last load percentage</param>
		private void UpdateLoadImageProgress(long currentPart, long totalParts, ref int lastLoadPercentage)
		{
			// calculates the percentage of the load process.
			int percentage = (int)(currentPart/(double)totalParts * 100);
			// checks whether load progress bar should be updated.
			if(IsLoadImageProgressUpdateRequired(percentage, lastLoadPercentage))
			{
				lastLoadPercentage = percentage;
				ShowLoadImageProgress(percentage);
			}
		}
		/// <summary>
		/// Updates image's ProgressBar by percentage.
		/// </summary>
		/// <param name="percentage">quantity by which progress bar is updated.</param>
		private void ShowLoadImageProgress(int percentage)
		{
			if(!this.InvokeRequired)
			{
				loadedImageProgressBar.Value = percentage;
				//check for completion.
				if(loadedImageProgressBar.Value == loadedImageProgressBar.Maximum)
				{
					loadImage.Enabled = true;
                    loadedImageFileDialog.Dispose();
				}
			}
			else
			{
				ShowLoadImageProgressDelagate showLoadImageProgressDelagate = new ShowLoadImageProgressDelagate(ShowLoadImageProgress);
				object[] parametrs = new object[]{percentage};
				this.BeginInvoke(showLoadImageProgressDelagate, parametrs);
				
			}
		}
		/// <summary>
		/// Loads the image.
		/// </summary>
		private void LoadImage()
		{
			Stream stream;
			if((stream = loadedImageFileDialog.OpenFile())!= null)
			{
				try
				{
					// image's length and content.
					long streamLength = stream.Length;
					byte[] imageContent = new byte[streamLength];
					// last loaded percentage.
					int lastLoadPercentage = 0;

					// reads the image content.
					long index = 0;
					int imageData = -1;
					while((imageData = stream.ReadByte()) != -1)
					{
						imageContent[index] = Convert.ToByte(imageData);
						// updates the load progress by ImageProgressPercentage.
						UpdateLoadImageProgress(index, streamLength, ref lastLoadPercentage);
						index++;
					}
					this.SuspendLayout();
					// gets the image from stream.
					image.Image = Image.FromStream(new MemoryStream(imageContent));
					// sets the progress process to 100%.
					ShowLoadImageProgress(100);
					this.ResumeLayout();
				}
				catch(Exception e)
				{
					Log.WriteErrorToLog(e.Message);
					throw;
				}
				finally
				{
					if(stream != null)
						stream.Close();
				}
			}
		}
		/// <summary>
		/// Handles LoadImage button event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoadImageClickEventHandler(object sender, System.EventArgs e)
		{
			
			if(loadedImageFileDialog.ShowDialog() == DialogResult.OK)
			{
				//disables the load button.
				loadImage.Enabled = false;
				//sets the PogressBar to minimum.
				loadedImageProgressBar.Value = loadedImageProgressBar.Minimum;

				if(image.Image != null)
					image.Image = null;
				
				//loads the image.
				LoadImageDelegate loadImageDelegate = new LoadImageDelegate(LoadImage);
				loadImageDelegate.BeginInvoke(null, null);
			}

		}
		#endregion Load Image Process.
		#endregion Private Methods.

		#region Protected Overrided Methods.
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		/// <summary>
		/// When resizing updates the image to be in the center.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			if(image.Image != null)
			{
				Console.Write(imagePanel.AutoScrollPosition.X + " " + imagePanel.AutoScrollPosition.Y);
				int leftUpperCorner = (this.ClientSize.Width/2 - image.ClientSize.Width/2);
				leftUpperCorner = (leftUpperCorner < imagePanel.Location.X) ? imagePanel.Location.X : leftUpperCorner;
				image.Location = new Point(leftUpperCorner, image.Location.Y);
			}
			base.OnResize (e);
		}
		
		#endregion Protected Overrided Methods.

		#region Public Default Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		public ImageViewer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
		}
		#endregion Public Default Constructor.

		#region Public Properties.
		/// <summary>
		/// Gets and Sets the image.
		/// </summary>
		public Image Image
		{
			get
			{
				return image.Image;
			}
			set
			{
				image.Image = value;
			}
		}
		#endregion Public Properties.
	}
}

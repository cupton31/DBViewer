using System;
using System.Windows.Forms;

using System.ComponentModel;

using DBViewer.Common;

namespace DBViewer.Gui
{
	/// <summary>
	/// DBViewerAbout - small info about the DBViewer.
	/// </summary>
	public class DBViewerAbout : Form
	{
		#region Private Members.
		private Button acceptButton;
		private Label applicationTitle;
		private Label applicationVersion;
		private Label applicationCopyright;
		#endregion Private Members.
		
		#region Required Designer Variable - Private Member.
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		#endregion Required Designer Variable - Private Member.

		#region Private Methods.
		/// <summary>
		/// Handles the load event of the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DBViewerAboutLoadEventHandler(object sender, EventArgs e)
		{
			this.Text = "About " + DBViewerConstants.ApplicationName;
			
			applicationTitle.Text = "Application: " + DBViewerConstants.ApplicationName;
			applicationVersion.Text = "Version: " + DBViewerConstants.ApplicationVersion;
			applicationCopyright.Text = "Copyright: " + DBViewerConstants.Copyright;
		}
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DBViewerAbout));
			this.applicationTitle = new System.Windows.Forms.Label();
			this.applicationVersion = new System.Windows.Forms.Label();
			this.applicationCopyright = new System.Windows.Forms.Label();
			this.acceptButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// applicationTitle
			// 
			this.applicationTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.applicationTitle.ForeColor = System.Drawing.Color.Navy;
			this.applicationTitle.Location = new System.Drawing.Point(8, 16);
			this.applicationTitle.Name = "applicationTitle";
			this.applicationTitle.Size = new System.Drawing.Size(208, 23);
			this.applicationTitle.TabIndex = 1;
			this.applicationTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// applicationVersion
			// 
			this.applicationVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.applicationVersion.ForeColor = System.Drawing.Color.Navy;
			this.applicationVersion.Location = new System.Drawing.Point(8, 56);
			this.applicationVersion.Name = "applicationVersion";
			this.applicationVersion.Size = new System.Drawing.Size(208, 23);
			this.applicationVersion.TabIndex = 2;
			this.applicationVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// applicationCopyright
			// 
			this.applicationCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.applicationCopyright.ForeColor = System.Drawing.Color.Navy;
			this.applicationCopyright.Location = new System.Drawing.Point(8, 96);
			this.applicationCopyright.Name = "applicationCopyright";
			this.applicationCopyright.Size = new System.Drawing.Size(208, 23);
			this.applicationCopyright.TabIndex = 3;
			this.applicationCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// acceptButton
			// 
			this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.acceptButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.acceptButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.acceptButton.ForeColor = System.Drawing.Color.Navy;
			this.acceptButton.Location = new System.Drawing.Point(76, 128);
			this.acceptButton.Name = "acceptButton";
			this.acceptButton.Size = new System.Drawing.Size(72, 24);
			this.acceptButton.TabIndex = 4;
			this.acceptButton.Text = "OK";
			// 
			// DBViewerAbout
			// 
			this.AcceptButton = this.acceptButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.acceptButton;
			this.ClientSize = new System.Drawing.Size(224, 158);
			this.Controls.Add(this.acceptButton);
			this.Controls.Add(this.applicationCopyright);
			this.Controls.Add(this.applicationVersion);
			this.Controls.Add(this.applicationTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DBViewerAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Load += new System.EventHandler(this.DBViewerAboutLoadEventHandler);
			this.ResumeLayout(false);

		}
		#endregion Windows Form Designer generated code
		#endregion Private Methods.

		#region Protected Methods.
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
		#endregion Protected Methods.

		#region Public Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		public DBViewerAbout()
		{
			InitializeComponent();
		}
		#endregion Public Constructor.

		
	}
}

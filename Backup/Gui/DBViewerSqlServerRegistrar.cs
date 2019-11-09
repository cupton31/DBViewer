using System;
using System.Windows.Forms;

using System.ComponentModel;

using System.Net;



namespace DBViewer.Gui
{
	/// <summary>
	/// DBViewerSqlServerRegistrator - collects info to register a new sql server.
	/// </summary>
	public class DBViewerSqlServerRegistrar : Form
	{

		#region Private Members.
		private GroupBox sqlServerDetails;
		private Button acceptRegistration;
		private Button cancelRegistration;
		
		private RadioButton integratedSecurity;
		private RadioButton specificNameAndPassword;
		
		private Label sqlServerNameLbl;
		private TextBox sqlServerNameTxtBox;
		
		private Label userNameLbl;
		private TextBox userNameTxtBox;

		private Label userPasswordLbl;
		private TextBox userPasswordTxtBox;
		#endregion Private Members.
		
		#region Required Designer Variable - Private Member.
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		#endregion Required Designer Variable - Private Member.

		#region Private Methods.
		/// <summary>
		/// ensures to disable specific name and password fields whether intergrated security was checked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IntegratedSecurityCheckedChangedEventHandler(object sender, EventArgs e)
		{
			if(integratedSecurity.Checked)
			{
				userNameTxtBox.Enabled = false;
				userPasswordTxtBox.Enabled = false;
			}
		}

		/// <summary>
		/// ensures to enable specific name and password fields whether this option was checked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SpecificNameAndPasswordCheckedChangedEventHandler(object sender, EventArgs e)
		{
			if(specificNameAndPassword.Checked)
			{
				userNameTxtBox.Enabled = true;
				userPasswordTxtBox.Enabled = true;
			}
		}
		
		#region Windows Form Designer generated code.
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DBViewerSqlServerRegistrar));
			this.acceptRegistration = new System.Windows.Forms.Button();
			this.cancelRegistration = new System.Windows.Forms.Button();
			this.sqlServerNameLbl = new System.Windows.Forms.Label();
			this.sqlServerNameTxtBox = new System.Windows.Forms.TextBox();
			this.sqlServerDetails = new System.Windows.Forms.GroupBox();
			this.userPasswordTxtBox = new System.Windows.Forms.TextBox();
			this.userNameTxtBox = new System.Windows.Forms.TextBox();
			this.userPasswordLbl = new System.Windows.Forms.Label();
			this.userNameLbl = new System.Windows.Forms.Label();
			this.specificNameAndPassword = new System.Windows.Forms.RadioButton();
			this.integratedSecurity = new System.Windows.Forms.RadioButton();
			this.sqlServerDetails.SuspendLayout();
			this.SuspendLayout();
			// 
			// acceptRegistration
			// 
			this.acceptRegistration.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.acceptRegistration.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.acceptRegistration.Location = new System.Drawing.Point(24, 232);
			this.acceptRegistration.Name = "acceptRegistration";
			this.acceptRegistration.TabIndex = 9;
			this.acceptRegistration.Text = "OK";
			// 
			// cancelRegistration
			// 
			this.cancelRegistration.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelRegistration.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelRegistration.Location = new System.Drawing.Point(200, 232);
			this.cancelRegistration.Name = "cancelRegistration";
			this.cancelRegistration.TabIndex = 10;
			this.cancelRegistration.Text = "Cancel";
			// 
			// sqlServerNameLbl
			// 
			this.sqlServerNameLbl.Location = new System.Drawing.Point(16, 32);
			this.sqlServerNameLbl.Name = "sqlServerNameLbl";
			this.sqlServerNameLbl.Size = new System.Drawing.Size(88, 20);
			this.sqlServerNameLbl.TabIndex = 1;
			this.sqlServerNameLbl.Text = "Sql Server name";
			this.sqlServerNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// sqlServerNameTxtBox
			// 
			this.sqlServerNameTxtBox.Location = new System.Drawing.Point(128, 32);
			this.sqlServerNameTxtBox.Name = "sqlServerNameTxtBox";
			this.sqlServerNameTxtBox.Size = new System.Drawing.Size(128, 20);
			this.sqlServerNameTxtBox.TabIndex = 2;
			this.sqlServerNameTxtBox.Text = "";
			// 
			// sqlServerDetails
			// 
			this.sqlServerDetails.Controls.Add(this.userPasswordTxtBox);
			this.sqlServerDetails.Controls.Add(this.userNameTxtBox);
			this.sqlServerDetails.Controls.Add(this.userPasswordLbl);
			this.sqlServerDetails.Controls.Add(this.userNameLbl);
			this.sqlServerDetails.Controls.Add(this.specificNameAndPassword);
			this.sqlServerDetails.Controls.Add(this.integratedSecurity);
			this.sqlServerDetails.Controls.Add(this.sqlServerNameTxtBox);
			this.sqlServerDetails.Controls.Add(this.sqlServerNameLbl);
			this.sqlServerDetails.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.sqlServerDetails.Location = new System.Drawing.Point(8, 8);
			this.sqlServerDetails.Name = "sqlServerDetails";
			this.sqlServerDetails.Size = new System.Drawing.Size(272, 216);
			this.sqlServerDetails.TabIndex = 0;
			this.sqlServerDetails.TabStop = false;
			this.sqlServerDetails.Text = "SQL Server";
			// 
			// userPasswordTxtBox
			// 
			this.userPasswordTxtBox.Location = new System.Drawing.Point(128, 185);
			this.userPasswordTxtBox.Name = "userPasswordTxtBox";
			this.userPasswordTxtBox.PasswordChar = '*';
			this.userPasswordTxtBox.Size = new System.Drawing.Size(128, 20);
			this.userPasswordTxtBox.TabIndex = 8;
			this.userPasswordTxtBox.Text = "";
			// 
			// userNameTxtBox
			// 
			this.userNameTxtBox.Location = new System.Drawing.Point(128, 145);
			this.userNameTxtBox.Name = "userNameTxtBox";
			this.userNameTxtBox.Size = new System.Drawing.Size(128, 20);
			this.userNameTxtBox.TabIndex = 6;
			this.userNameTxtBox.Text = "";
			// 
			// userPasswordLbl
			// 
			this.userPasswordLbl.Location = new System.Drawing.Point(32, 184);
			this.userPasswordLbl.Name = "userPasswordLbl";
			this.userPasswordLbl.Size = new System.Drawing.Size(56, 23);
			this.userPasswordLbl.TabIndex = 7;
			this.userPasswordLbl.Text = "Password";
			this.userPasswordLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// userNameLbl
			// 
			this.userNameLbl.Location = new System.Drawing.Point(32, 144);
			this.userNameLbl.Name = "userNameLbl";
			this.userNameLbl.Size = new System.Drawing.Size(56, 23);
			this.userNameLbl.TabIndex = 5;
			this.userNameLbl.Text = "User";
			this.userNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// specificNameAndPassword
			// 
			this.specificNameAndPassword.Location = new System.Drawing.Point(24, 112);
			this.specificNameAndPassword.Name = "specificNameAndPassword";
			this.specificNameAndPassword.Size = new System.Drawing.Size(200, 24);
			this.specificNameAndPassword.TabIndex = 4;
			this.specificNameAndPassword.Text = "Use a specific name and password";
			this.specificNameAndPassword.CheckedChanged += new System.EventHandler(this.SpecificNameAndPasswordCheckedChangedEventHandler);
			// 
			// integratedSecurity
			// 
			this.integratedSecurity.Location = new System.Drawing.Point(24, 72);
			this.integratedSecurity.Name = "integratedSecurity";
			this.integratedSecurity.Size = new System.Drawing.Size(200, 24);
			this.integratedSecurity.TabIndex = 3;
			this.integratedSecurity.Text = "Integrated Security";
			this.integratedSecurity.CheckedChanged += new System.EventHandler(this.IntegratedSecurityCheckedChangedEventHandler);
			// 
			// DBViewerSqlServerRegistrar
			// 
			this.AcceptButton = this.acceptRegistration;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelRegistration;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.sqlServerDetails);
			this.Controls.Add(this.cancelRegistration);
			this.Controls.Add(this.acceptRegistration);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DBViewerSqlServerRegistrar";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Sql Server Registration";
			this.sqlServerDetails.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion Windows Form Designer generated code.
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
		public DBViewerSqlServerRegistrar()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			// sets the integrated security as a default.
			integratedSecurity.Checked = true;
			// sets the local machine name.
			sqlServerNameTxtBox.Text = Dns.GetHostName();
		}
		#endregion Public Constructor.

		#region Public Properties.
		/// <summary>
		/// gets the server's name.
		/// </summary>
		public string ServerName
		{
			get
			{
				return sqlServerNameTxtBox.Text;
			}
		}

		/// <summary>
		/// gets the user's name.
		/// </summary>
		public string UserName
		{
			get
			{
				return userNameTxtBox.Text;
			}
		}
		/// <summary>
		/// gets the user's password.
		/// </summary>
		public string UserPassword
		{
			get
			{
				return userPasswordTxtBox.Text;
			}
		}
		#endregion Public Properties.
	}
}

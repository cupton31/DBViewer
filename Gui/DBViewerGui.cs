using System;
using System.Text;
using System.Data;
using System.Collections;

using System.Windows.Forms;

using System.ComponentModel;

using DBViewer.Common;
using DBViewer.BL;

namespace DBViewer.Gui
{
	/// <summary>
	/// DBViewerGui - DBViewer main UI.
	/// </summary>
	public class DBViewerGui : Form , IDBViewerGui 
	{

		#region Singelton - Public Static Instance.
		/// <summary>
		/// Instance member.
		/// </summary>
		public static DBViewerGui instance;
		#endregion Singelton - Private Static Instance.
		
		#region DatabaseTreeView, Splitter - Public/Private Members.
		/// <summary>
		/// database tree representation.
		/// </summary>
		public DatabaseTreeView dbTree;
		/// <summary>
		/// splitter between dbTree and dbData.
		/// </summary>
		private Splitter splitter;
		#endregion DatabaseTreeView, Splitter - Private Members.

		#region Menu Items - Private Members.
		/// <summary>
		/// Menu
		/// </summary>
		private MainMenu dbViewerMenu;
		/// <summary>
		/// File
		/// </summary>
		private MenuItem fileMenuItem;
		/// <summary>
		/// Exit
		/// </summary>
		private MenuItem exitMenuItem;
		/// <summary>
		/// Window
		/// </summary>
		private MenuItem windowMenuItem;
		/// <summary>
		/// Arrange Icons
		/// </summary>
		private MenuItem arrangeIconsMenuItem;
		/// <summary>
		/// Cascade
		/// </summary>
		private MenuItem cascadeMenuItem;
		/// <summary>
		/// Tables Vertically
		/// </summary>
		private MenuItem tablesVeritcallyMenuItem;
		/// <summary>
		/// Tables Horizontally
		/// </summary>
		private MenuItem tablesHorizontallyMenuItem;
        private IContainer components;
        #endregion Menu Items - Private Members.

        #region Windos Form Designer Component - Private Member.
        #endregion Windos Form Designer Component.

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBViewerGui));
            this.splitter = new System.Windows.Forms.Splitter();
            this.dbViewerMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileMenuItem = new System.Windows.Forms.MenuItem();
            this.exitMenuItem = new System.Windows.Forms.MenuItem();
            this.windowMenuItem = new System.Windows.Forms.MenuItem();
            this.arrangeIconsMenuItem = new System.Windows.Forms.MenuItem();
            this.cascadeMenuItem = new System.Windows.Forms.MenuItem();
            this.tablesVeritcallyMenuItem = new System.Windows.Forms.MenuItem();
            this.tablesHorizontallyMenuItem = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // splitter
            // 
            this.splitter.Location = new System.Drawing.Point(0, 0);
            this.splitter.Name = "splitter";
            this.splitter.Size = new System.Drawing.Size(3, 566);
            this.splitter.TabIndex = 1;
            this.splitter.TabStop = false;
            // 
            // dbViewerMenu
            // 
            this.dbViewerMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenuItem,
            this.windowMenuItem});
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.Index = 0;
            this.fileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.exitMenuItem});
            this.fileMenuItem.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.fileMenuItem.Text = "&File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Index = 0;
            this.exitMenuItem.MergeOrder = 1;
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItemEventHandler);
            // 
            // windowMenuItem
            // 
            this.windowMenuItem.Index = 1;
            this.windowMenuItem.MdiList = true;
            this.windowMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.arrangeIconsMenuItem,
            this.cascadeMenuItem,
            this.tablesVeritcallyMenuItem,
            this.tablesHorizontallyMenuItem});
            this.windowMenuItem.MergeOrder = 2;
            this.windowMenuItem.Text = "&Window";
            // 
            // arrangeIconsMenuItem
            // 
            this.arrangeIconsMenuItem.Index = 0;
            this.arrangeIconsMenuItem.Text = "&Arrange Icons";
            this.arrangeIconsMenuItem.Click += new System.EventHandler(this.ArrangeIconsMenuItemEventHandler);
            // 
            // cascadeMenuItem
            // 
            this.cascadeMenuItem.Index = 1;
            this.cascadeMenuItem.Text = "&Cascade";
            this.cascadeMenuItem.Click += new System.EventHandler(this.CascadeMenuItemEventHandler);
            // 
            // tablesVeritcallyMenuItem
            // 
            this.tablesVeritcallyMenuItem.Index = 2;
            this.tablesVeritcallyMenuItem.Text = "Tables &Vertically";
            this.tablesVeritcallyMenuItem.Click += new System.EventHandler(this.TablesVeritcallyMenuItemEventHandler);
            // 
            // tablesHorizontallyMenuItem
            // 
            this.tablesHorizontallyMenuItem.Index = 3;
            this.tablesHorizontallyMenuItem.Text = "Tables Hori&zontally";
            this.tablesHorizontallyMenuItem.Click += new System.EventHandler(this.TablesHorizontallyMenuItemEventHandler);
            // 
            // DBViewerGui
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.splitter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Menu = this.dbViewerMenu;
            this.Name = "DBViewerGui";
            this.Text = "DBViewer";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.DBViewerGui_Shown);
            this.ResumeLayout(false);

		}
		#endregion

		#region Public Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		public DBViewerGui()
		{
			InitializeComponent();

			// sets the database TreeView.
			dbTree = new DatabaseTreeView();
			dbTree.Parent = this;
			dbTree.Dock = DockStyle.Left;
        }
        #endregion Private Constructor.

        #region Private Methods.
        // Purpose: Loads the Database into the TreeView whe nthe Form gets shown for the first time.
        private void DBViewerGui_Shown(object sender, EventArgs e)
        {
            DBViewerSqlServerRegistrar registrar = new DBViewerSqlServerRegistrar();

            if (registrar.ShowDialog(this) == DialogResult.OK)
            {
                // gets the server, user name and password.
                string server = registrar.ServerName;
                string user = registrar.UserName;
                string password = registrar.UserPassword;

                // updates the DBViewerGui that a sql server should be added.
                DBViewerGui.Instance().AddSqlServer(server, user, password);
            }

            registrar.Dispose();
        }
        /// <summary>
        /// Exit Menu Item Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitMenuItemEventHandler(object sender, EventArgs e)
		{
			// closes current form.
			this.Close();
		}
		/// <summary>
		/// About Menu Item Event Handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AboutMenuItemEventHandler(object sender, EventArgs e)
		{
			DBViewerAbout about = new DBViewerAbout();
			about.ShowDialog(this);
			about.Dispose();
		}
		/// <summary>
		/// Arrange Items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ArrangeIconsMenuItemEventHandler(object sender, System.EventArgs e)
		{
			this.LayoutMdi(MdiLayout.ArrangeIcons);
		}
		/// <summary>
		/// Cascade Items
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CascadeMenuItemEventHandler(object sender, System.EventArgs e)
		{
			this.LayoutMdi(MdiLayout.Cascade);
		}
		/// <summary>
		/// Items Vertically
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TablesVeritcallyMenuItemEventHandler(object sender, System.EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileVertical);
		}
		/// <summary>
		/// Items Horizontally
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TablesHorizontallyMenuItemEventHandler(object sender, System.EventArgs e)
		{
			this.LayoutMdi(MdiLayout.TileHorizontal);
		}
		/// <summary>
		/// Adds DataTable first column to an ArrayList.
		/// (the method helps to transfer info to the DatabaseTreeView controller).
		/// </summary>
		/// <param name="table">DataTable</param>
		/// <returns>First DataTable column in the ArrayList</returns>
		private ArrayList ConstructDatabasesRepresentation(DataTable table)
		{
			ArrayList ret = new ArrayList();
			foreach(DataRow row in table.Rows)
				ret.Add(row[0]);
			
			return ret;
		}
		/// <summary>
		/// Cunstructs list of database's tables for view in the TreeView.
		/// </summary>
		/// <param name="database">Database</param>
		/// <param name="tables">Tables</param>
		/// <returns>An ArrayList of tables for representation.</returns>
		private ArrayList ConstructTablesRepresentation(TreeNode database, DataTable tables)
		{
			ArrayList ret = new ArrayList();
			foreach(DataRow table in tables.Rows)
			{
                string TableName = table[0].ToString();
                if (!dbTree.TreeNodeContains(database, TableName))
                {
                    if (DBViewerConstants.AdminMode)
                    {
                        foreach (DBViewerConstants.TableRule TableRule in DBViewerConstants.AdminTables)
                        {
                            if (TableRule.TableName == TableName)
                            {
                                ret.Add(table[0]);
                            }
                        }
                    }
                    else
                    {
                        foreach (DBViewerConstants.TableRule TableRule in DBViewerConstants.UserTables)
                        {
                            if (TableRule.TableName == TableName)
                            {
                                ret.Add(table[0]);
                            }
                        }
                    }
                }
			}
			return ret;
		}
		/// <summary>
		/// Checks whether DBViewerDataGrid was previously opened by DBViewer.
		/// </summary>
		/// <remarks>Activates DBViewerDataGrid if was previosly opened.</remarks>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		/// <returns>True if DBViewerDataGrid was previously opened by DBViewer.</returns>
		private bool IsExistedDBViewerDataGrid(string server, string database, string table)
		{
			// constructs the DataGrid's (tables) title.
			StringBuilder title = new StringBuilder();
			title.Append(server);
			title.Append(".");
			title.Append(database);
			title.Append(".");
			title.Append(table);
			
			// searches for existing mdi child.
			Form[] children = this.MdiChildren;
			for(int i=0; i<children.Length; i++)
			{
				if(children[i].Text == title.ToString())
				{	
					// prevent flickering.
					children[i].Visible = false;
					// activate the child.
					children[i].BringToFront();
					children[i].Activate();
					children[i].Visible = true;
					return true;
				}
			}
				
			return false;
		}
		#endregion Private Methods.
		
		#region Protected Methods.
		/// <summary>
		/// Dispose.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion Protected Methods.

		#region Public Methods.
		/// <summary>
		/// DBViewerGui Instance - Singelton.
		/// </summary>
		/// <returns></returns>
		public static DBViewerGui Instance()
		{
			if(instance == null)
                instance = new DBViewerGui();

			return instance;
		}

        /// <summary>
        /// Fires when DatabaseTreeView is requested for new sql server.
        /// </summary>
        /// <param name="server">server</param>
        /// <param name="user">user</param>
        /// <param name="password">password</param>
        public void AddSqlServer(string server, string user, string password)
		{
            // Set Forms Text Title
            this.Text = "EndpointMachine_Id = " + DBViewerConstants.EndpointMachine_Id;

			if(dbTree.TreeNodeContains(dbTree.SelectedNode, server))
			{
				MessageBox.Show(server + " SQL server already exists", DBViewerConstants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		
			DBViewerApp.Instance().DatabasesInfoRequested(server, user, password);
		}

		/// <summary>
		/// Fires when DatabaseTreeView is requested for all tables of the sql databases in a given sql server.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="databases">database</param>
		public void AddSqlDataTables(string server, ArrayList databases)
		{
			DBViewerApp.Instance().DatabasesTablesInfoRequested(server, databases);	
		}
		/// <summary>
		/// Fires when DatabaseTreeView is requested for a table. 
		/// </summary>
		/// <remarks>Checks whether the table is already represented by the DatabaseDataGrid and if true activates it instead of creating a new table.</remarks>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		public void ViewDataTable(string server, string database, string table)
		{
			if(!IsExistedDBViewerDataGrid(server, database, table))
				DBViewerApp.Instance().DatabaseTableRequested(server, database, table);
		}
		/// <summary>
		/// Save requisted
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		/// <param name="data">data</param>
		public void SaveDatabaseTable(string server, string database, string table, DataTable data)
		{
			DBViewerApp.Instance().SaveDatabaseTableRequested(server, database, table, data);
		}
		/// <summary>
		/// Represents the list of the databases in the DatabaseTreeView.
		/// </summary>
		/// <param name="databases">databases</param>
		public void ShowDatabasesInfo(DataTable databases)
		{
			try
			{
				if(databases == null)
					throw new ArgumentNullException();

				ArrayList nodes = ConstructDatabasesRepresentation(databases);

				TreeNode serverNode = new TreeNode(databases.TableName, 1, 1);
				dbTree.SelectedNode.Nodes.Add(serverNode);
				dbTree.RefreshTree(serverNode, nodes);
			}
			catch (Exception e)
			{
				Log.WriteErrorToLogAndNotifyUser(e.Message);
				// catches all exceptions.
			}
		}
		/// <summary>
		/// Represents the list of the tables for all databases in the DatabaseTreeView.
		/// </summary>
		/// <param name="databasesTables">databases tables</param>
		public void ShowDatabasesTablesInfo(DataSet databasesTables)
		{
			try
			{
				if(databasesTables == null)
					throw new ArgumentNullException();

				// for each server in the "SQL servers" node
				foreach(TreeNode server in dbTree.Nodes[0].Nodes)
				{
					// if the server should be updated then ...
					if(databasesTables.DataSetName == server.Text)
					{
						foreach(TreeNode database in server.Nodes)
						{
							ArrayList nodes = ConstructTablesRepresentation(database, databasesTables.Tables[database.Text]);
                            dbTree.RefreshTree(database, nodes);
						}
					}
				}
			}
			catch (Exception e)
			{
				Log.WriteErrorToLogAndNotifyUser(e.Message);
				// catches all exceptions.
			}
		}
		/// <summary>
		/// Represents the table's info in the DBViewerDataGrid
		/// </summary>
		/// <param name="table">table</param>
		public void ShowDatabaseTableInfo(DataTable table)
		{
			try
			{
				DBViewerDataGrid dbData = new DBViewerDataGrid(table);
				dbData.MdiParent = this;
				dbData.Show();
			}
			catch (Exception e)
			{
				Log.WriteErrorToLogAndNotifyUser(e.Message);
				// catches all exceptions.
			}
		}
		#endregion Public Methods.

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			try
			{
				Application.EnableVisualStyles();
                DBViewerConstants.connectionString = "data source=IA-SQL01\\IALICENSE;initial catalog=LightBridgeDev;persist security info=True;user id=iadev;password=iadev;encrypt=False;trustservercertificate=False;user instance=False;context connection=False;MultipleActiveResultSets=True;App=EntityFramework";
                DBViewerConstants.EndpointMachine_Id = 2;
                Application.Run(DBViewerGui.Instance());
			}
			catch
			{
				// catches all exceptions.
			}
		}
    }
}

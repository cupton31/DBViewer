using System;
using System.Collections;

using System.Drawing;
using System.Windows.Forms;

namespace DBViewer.Gui
{

	/// <summary>
	/// DatabaseTreeView inherits all the features of the TreeView control and adds a support for database tree representation.
	/// DatabaseTreeView has 4 levels of hierarchy
	///	1. SQL Servers = root node
	///		2. servers (local for example)  = second level node
	///			3. databases (Northwind for example) = third level node
	///				4. tables (Customers for example) = forth level node
	/// </summary>
	public class DatabaseTreeView : TreeView
	{
		#region Private Constants.
		#region TreeView.
		private const int DatabaseTreeViewIndent = 15;
		private const int DatabaseTreeViewItemHeight = 18;
		#endregion TreeView.
		
		#region Context Menu.
		private const string DatabaseTreeViewRootNode = "SQL Servers";
		private const string SqlServerRegistrationMenuItemText = "Add new server ...";
		private const string ViewTableDataMenuItemText = "View table";
		#endregion Context Menu.
		#endregion Private Constants.

		#region Private Members.
		private ContextMenu databaseTreeViewContextMenu;
		private MenuItem sqlServerRegistrationMenuItem;
		private MenuItem viewTableDataMenuItem;
		#endregion Private Members.

		#region Private Methods.

		private void SendRequestForDataTableView()
		{
			if(CalculateNodeHierarchy(this.SelectedNode) == (int)DatabaseTreeViewHierarchy.Table)
			{
				string path = this.SelectedNode.FullPath;
				string[] arguments = path.Split(this.PathSeparator[0]);

				DBViewerGui.Instance().ViewDataTable(arguments[1], arguments[2], arguments[3]);
			}
		}

		/// <summary>
		/// Calculates the depth of the TreeNode node in the TreeView
		/// </summary>
		/// <param name="node">The node which depth should be calculated</param>
		/// <returns>The depth of the node</returns>
		private int CalculateNodeHierarchy(TreeNode node)
		{
			string path = node.FullPath;
			string[] depth = path.Split(this.PathSeparator[0]);
			
			return depth.Length;
		}

		/// <summary>
		/// Handles the Popup event of the context menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SqlServerRegistrationContextMenuPopupEventHandler(object sender, EventArgs e)
		{
			int hierarchy = CalculateNodeHierarchy(SelectedNode);

			if(hierarchy == (int)DatabaseTreeViewHierarchy.Root)
			{
				sqlServerRegistrationMenuItem.Visible = true;
				viewTableDataMenuItem.Visible = false;
			}
			else if(hierarchy == (int)DatabaseTreeViewHierarchy.Table)
			{
				sqlServerRegistrationMenuItem.Visible = false;
				viewTableDataMenuItem.Visible = true;
			}
			else
			{
				sqlServerRegistrationMenuItem.Visible = false;
				viewTableDataMenuItem.Visible = false;
			}
		}

		/// <summary>
		/// Handles the "Add new server ..." click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SqlServerRegistrationMenuItemClickEventHandler(object sender, EventArgs e)
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
		/// Handles the "View table" click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ViewTableDataMenuItemClickEventHandler(object sender, EventArgs e)
		{
			SendRequestForDataTableView();
		}

		#endregion Private Methods.

		#region Protected Overrided Methods.
		/// <summary>
		/// Selects the node in TreeView when a right click event happened.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			// adds right mouse button selection.
			if(e.Button == MouseButtons.Right)
			{
				if(this.GetNodeAt(e.X, e.Y) != null)
					this.SelectedNode = this.GetNodeAt(e.X, e.Y);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
		{
			base.OnBeforeExpand (e);
			// TODO: add a check the eliminates the need ot expand it every time.
			if(CalculateNodeHierarchy(e.Node) == (int)DatabaseTreeViewHierarchy.Server)
			{
				ArrayList databases = new ArrayList();
				foreach(TreeNode node in e.Node.Nodes)
					databases.Add(node.Text);
				DBViewerGui.Instance().AddSqlDataTables(e.Node.Text, databases);
			}
        }
		/// <summary>
		/// Opens the requested DataTable
		/// </summary>
		/// <param name="e"></param>
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick (e);

			SendRequestForDataTableView();
		}

		#endregion Protected Overrided Methods.

		#region Public Methods.
		/// <summary>
		/// Constructor.
		/// </summary>
		public DatabaseTreeView() : base()
		{
			// ident and height.
			Indent = DatabaseTreeViewIndent;
			ItemHeight = DatabaseTreeViewItemHeight;	

			// more space for databeses and tables.
			Width *= 2;

			// tab index.
			TabIndex = 0;

			// context menu.
			databaseTreeViewContextMenu = new ContextMenu();
			sqlServerRegistrationMenuItem = new MenuItem(SqlServerRegistrationMenuItemText);			
			sqlServerRegistrationMenuItem.Index = 0;
			databaseTreeViewContextMenu.MenuItems.Add(sqlServerRegistrationMenuItem);
			viewTableDataMenuItem = new MenuItem(ViewTableDataMenuItemText);
			viewTableDataMenuItem.Index = 1;
			databaseTreeViewContextMenu.MenuItems.Add(viewTableDataMenuItem);

			ContextMenu = databaseTreeViewContextMenu;
			
			// events.
			this.databaseTreeViewContextMenu.Popup += new EventHandler(SqlServerRegistrationContextMenuPopupEventHandler);
			this.sqlServerRegistrationMenuItem.Click += new EventHandler(SqlServerRegistrationMenuItemClickEventHandler);
			this.viewTableDataMenuItem.Click += new EventHandler(ViewTableDataMenuItemClickEventHandler);
			
			ImageList = new ImageList();
			ImageList.Images.Add(new Bitmap(this.GetType(),"root.ico"));
			ImageList.Images.Add(new Bitmap(this.GetType(),"srv.ico"));
			ImageList.Images.Add(new Bitmap(this.GetType(),"db.ico"));
			ImageList.Images.Add(new Bitmap(this.GetType(),"table.ico"));

			RefreshTree();
        }

		/// <summary>
		/// Constructs the tree.
		/// </summary>
		public void RefreshTree()
		{
			BeginUpdate();
			
			Nodes.Add(new TreeNode(DatabaseTreeViewRootNode, 0, 0));

			EndUpdate();
		}
		/// <summary>
		/// Constracts the tree from the root node.
		/// </summary>
		/// <param name="root">Root node.</param>
		/// <param name="nodes">Nodes to be added to the root node.</param>
		public void RefreshTree(TreeNode root, ArrayList nodes)
		{
			if(root == null || nodes == null)
				throw new ArgumentNullException();

			BeginUpdate();
			
			int hierarchy = CalculateNodeHierarchy(root);
			
			foreach(string node in nodes)
				root.Nodes.Add(new TreeNode(node, hierarchy, hierarchy));

			EndUpdate();
		}
		/// <summary>
		/// True if one of the node's descendants text equals to the given text.
		/// </summary>
		/// <param name="node">Node</param>
		/// <param name="text">Text</param>
		/// <returns>True if one of the node's descendants text equals to the given text.</returns>
		public bool TreeNodeContains(TreeNode node, string text)
		{
			foreach(TreeNode descendant in node.Nodes)
				if(descendant.Text == text)
					return true;
			return false;
		}
		#endregion Public Methods.
	}
}

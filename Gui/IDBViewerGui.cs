using System.Data;

namespace DBViewer.Gui
{
	/// <summary>
	/// IDBViewerGui - defines an interface of the DBViewer main UI.
	/// </summary>
	public interface IDBViewerGui
	{
		/// <summary>
		/// Represents the list of databases in DBViewerGui.
		/// </summary>
		/// <param name="databases">databases</param>
		void ShowDatabasesInfo(DataTable databases);
		/// <summary>
		/// Represents the list of all databases tables in DBViewerGui.
		/// </summary>
		/// <param name="databasesTables">databasesTables</param>
		void ShowDatabasesTablesInfo(DataSet databasesTables);
		/// <summary>
		/// Represents a specific table in the DBViewerGui.
		/// </summary>
		/// <param name="databaseTable">databaseTable</param>
		void ShowDatabaseTableInfo(DataTable databaseTable);
	}
}

using System.Collections;
using System.Data;

namespace DBViewer.BL
{
	/// <summary>
	/// IDBViewerApp - The interface of DBViewerApp <see cref="DBViewerApp"/>
	/// </summary>
	public interface IDBViewerApp
	{
		/// <summary>
		/// Retrieves the list of the database from the SQL server.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		void DatabasesInfoRequested(string server, string user, string password);
		/// <summary>
		/// Retrieves the list of the tables for each database from the SQL server. 
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="databases">server's databases</param>
		void DatabasesTablesInfoRequested(string server, ArrayList databases);
		/// <summary>
		/// Retrieves the table's data.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		void DatabaseTableRequested(string server, string database, string table);
		/// <summary>
		/// Saves the DataTable into the DB.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		/// <param name="data">data</param>
		void SaveDatabaseTableRequested(string server, string database, string table, DataTable data);
	}
}

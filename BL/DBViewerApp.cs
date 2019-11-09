using System;
using System.Text;
using System.Data;
using System.Collections;

using System.Globalization;

using DBViewer.Common;
using DBViewer.Gui;
using DBViewer.DAL;

namespace DBViewer.BL
{
	/// <summary>
	/// DBViewerApp - DBViewer Business logic.
	/// </summary>
	public sealed class DBViewerApp : IDBViewerApp 
	{
		#region Private Constants.	
		#region Queries.
		/// <summary>
		/// Query's length
		/// </summary>
		private const int QueryLength = 100;
		/// <summary>
		/// Database Queries.
		/// </summary>
		private const string DatabasesQuery	= "SELECT DISTINCT CATALOG_NAME FROM INFORMATION_SCHEMA.SCHEMATA";
		private const string DatabasesTablesQuery = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES";
		private const string DatabaseTableQuery = "SELECT * FROM ";
        private const string DatabaseTableQuery1 = "WHERE EndpointMachine_Id='";
        private const string DatabaseTableQuery2 = "';";
        private const string DatabaseTableTypesQuery = "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=";
		#endregion Queries.
		#endregion Private Constants.
		
		#region Private Static - DBViewerApp Instance.
		/// <summary>
		/// DBViewerApp Instance - Singelton
		/// </summary>
		private static DBViewerApp instance;
		#endregion Private Static -  DBViewerApp Instance.	

		#region Private Default Constractor.
		/// <summary>
		/// Constructor.
		/// </summary>
		private DBViewerApp()
		{	
		}
		#endregion Private Default Constractor.

		#region Private Methods.
		/// <summary>
		/// Foreach binary field adds an identification to its ColumnName for Gui purpose.
		/// </summary>
		/// <param name="data">DataTable with the data</param>
		/// <param name="types">DataTable's types</param>
		private static void ConstructDataTablesTypes(DataTable data, DataTable types)
		{
			int index = 0;
			// each row in the types DataTable represents a sql column datatype in the data DataTable's Columns collection.
			foreach(DataRow row in types.Rows)
			{
				// should by only one column in the types DataTable.
				// uses String.Compare with insensitive comparision.
				if(String.Compare((string)row[0], DBViewerConstants.ImageColumnType, false, CultureInfo.InvariantCulture) == 0)
					data.Columns[index].ColumnName = (DBViewerConstants.ImageColumnTypeDelimiter + data.Columns[index].ColumnName);
				else if(String.Compare((string)row[0], DBViewerConstants.BinaryColumnType, false, CultureInfo.InvariantCulture) == 0 || String.Compare((string)row[0], DBViewerConstants.VarBinaryColumnType, false, CultureInfo.InvariantCulture) == 0)
					data.Columns[index].ColumnName = (DBViewerConstants.BinaryColumnTypeDelimiter + data.Columns[index].ColumnName);

				index++;	
			}
		}
		/// <summary>
		/// Constructs the DataTable's full name according to its: server, database and table names.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		/// <returns>DataTable's full name</returns>
		private static string ConstructDataTableFullName(string server, string database, string table)
		{
			StringBuilder datatableName = new StringBuilder();
			datatableName.Append(server);
			datatableName.Append(".");
			datatableName.Append(database);
			datatableName.Append(".");
			datatableName.Append(table);
			return datatableName.ToString();
		}
		#endregion Private Methods.

		#region Public Static - DBViewerApp Instance.
		/// <summary>
		/// DBViewer Instance - Singelton.
		/// </summary>
		/// <returns></returns>
		public static DBViewerApp Instance()
		{
			if(instance == null)
				instance = new DBViewerApp();
			return instance;
		}

		#endregion Public Static - DBViewerApp Instance.

		#region Public Methods.
		#region IDBViewerApp Methods.
		/// <summary>
		/// Retrieves the list of databases from the SQL server.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		public void DatabasesInfoRequested(string server, string user, string password)
		{	
			try
			{
				if(server == null || user == null || password == null)
					throw new ArgumentNullException();
				
				// gets the list of all databases in the given sql server.
				DBViewerDAL.Instance().SetConnection(server, user, password);
				DataTable data = DBViewerDAL.Instance().GetData(DatabasesQuery);
				data.TableName = server;

				// saves user's credentials.
				DBViewerCache.Instance().AddCredentials(server, user, password);
								
				// represents the data in the DatabaseTreeView.
				DBViewerGui.Instance().ShowDatabasesInfo(data);
			}
			catch(Exception e)
			{
				// all exception catched.
				Log.WriteErrorToLogAndNotifyUser(e.Message);
			}
		}
		/// <summary>
		/// Retrieves the list of all databases tables from the SQL server.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="databases">an ArrayList of databases</param>
		public void DatabasesTablesInfoRequested(string server, ArrayList databases)
		{
			try
			{
				if(server == null || databases == null)
					throw new ArgumentNullException();
			
				DataSet databasesTables = new DataSet(server);
				databasesTables.Locale = CultureInfo.InvariantCulture; 

				foreach(string database in databases)
				{
					// gets user name and password.
					UserCredentials credentials = DBViewerCache.Instance().GetCredentials(server);
					// sets sql connection to the sql server.
					DBViewerDAL.Instance().SetConnection(server, database, credentials.User, credentials.Password);
					// gets the tables for the database.
					DataTable databaseTable = DBViewerDAL.Instance().GetData(DatabasesTablesQuery);
					// constructs the answer.
					databaseTable.TableName = database;
                    databasesTables.Tables.Add(databaseTable);
				}
			
				// represents the data in the DatabaseTreeView.
				DBViewerGui.Instance().ShowDatabasesTablesInfo(databasesTables);
			}
			catch(Exception e)
			{
				// all exception catched.
				Log.WriteErrorToLogAndNotifyUser(e.Message);
			}
		}
		/// <summary>
		/// Retrieves the table's data from the SQL server's database and table.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		public void DatabaseTableRequested(string server, string database, string table)
		{
			try
			{
				DataTable data = null;
				string datatableName = ConstructDataTableFullName(server, database, table);
				
				// if cache requested - checks whether the table was fetched already.
				//if(DBViewerCache.Instance().IsCacheRequested())
				{
					data = DBViewerCache.Instance().GetData(server, database, datatableName);
					if(data != null)
						DBViewerGui.Instance().ShowDatabaseTableInfo(data);	
				}

				// if(data == null) or the cache isn't requested.
				// if the cache isn't requested the data will be always null.
				if(data == null)
				{
					// gets user name and password.
					UserCredentials credentials = DBViewerCache.Instance().GetCredentials(server);
			
					// sets sql connection to the sql server.
					DBViewerDAL.Instance().SetConnection(server, database, credentials.User, credentials.Password);

					// constructs a query
					StringBuilder dataQuery = new StringBuilder(QueryLength);
					dataQuery.Append(DatabaseTableQuery);
					dataQuery.Append("dbo.[");
					dataQuery.Append(table);
					dataQuery.Append("] ");
                    if (!DBViewerConstants.AdminMode)
                    {
                        if (DBViewerConstants.UserTables.Find(x => (x.hasEndpointMachineIdForeignKey == true) && (x.TableName == table)) != null)
                        {
                            dataQuery.Append(DatabaseTableQuery1);
                            dataQuery.Append(DBViewerConstants.EndpointMachine_Id.ToString());
                            dataQuery.Append("';");
                        }
                    }

					// gets database data.
					data = DBViewerDAL.Instance().GetData(dataQuery.ToString());
					data.TableName = datatableName;

					// retrieves the sql datatypes info for the fetched table.
					StringBuilder typesQuery = new StringBuilder(QueryLength);
					typesQuery.Append(DatabaseTableTypesQuery);
					typesQuery.Append("'");
					typesQuery.Append(table);
					typesQuery.Append("'");
					DataTable databaseTypes = DBViewerDAL.Instance().GetData(typesQuery.ToString());

					// updates the answer with sql datatypes info.
					ConstructDataTablesTypes(data, databaseTypes);

					// if cache requested - saves the data in the cache.
					//if(DBViewerCache.Instance().IsCacheRequested())
						DBViewerCache.Instance().AddData(server, database, credentials.User, credentials.Password, data);

					// represents the data in the DatabaseTreeView.
					DBViewerGui.Instance().ShowDatabaseTableInfo(data);
				}
			}
			catch(Exception e)
			{
				// all exception catched.
				Log.WriteErrorToLogAndNotifyUser(e.Message);
			}
		}

		/// <summary>
		/// Saves changed DataTable.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		/// <param name="data">data (actual DataTable)</param>
		public void SaveDatabaseTableRequested(string server, string database, string table, DataTable data)
		{
			try
			{
				UserCredentials credentials = DBViewerCache.Instance().GetCredentials(server);
				// sets sql connection to the sql server.
				DBViewerDAL.Instance().SetConnection(server, database, credentials.User, credentials.Password);
				DBViewerDAL.Instance().Save(data);
				// if the cache requested - resets the cache
				//if(DBViewerCache.Instance().IsCacheRequested())
					DBViewerCache.Instance().RemoveData(server, database, ConstructDataTableFullName(server, database, table));
			}
			catch(Exception e)
			{
				// all exception catched.
				Log.WriteErrorToLogAndNotifyUser(e.Message);
			}
		}
		#endregion IDBViewerApp Methods.
		#endregion Public Methods.
	}
}

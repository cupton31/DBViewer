using System;
using System.Text;

using System.Data;
using System.Data.SqlClient;

using System.Globalization;

using DBViewer.Common;

namespace DBViewer.DAL
{
	/// <summary>
	/// DBViewer: Data access layer.
	/// </summary>
	public sealed class DBViewerDAL : IDisposable
	{

		#region Private Constants.
		/// <summary>
		/// Maximum connection string length.
		/// </summary>
		private const int ConnectionStringLength	=	100;
		#endregion Private Constants.
			
		#region Private Memebers.
		/// <summary>
		/// SQL Connection.
		/// </summary>
		private SqlConnection connection;
		/// <summary>
		/// SQL Command.
		/// </summary>
		private SqlCommand command;
		#endregion Private Memebers.
	
		#region Private Static Member - DBViewerDal instance.
		/// <summary>
		/// DBViewerDal instance - singelton.
		/// </summary>
		private static DBViewerDAL instance;
		#endregion Private Static Instance - DBViewerDal instance.

		#region Private Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		private DBViewerDAL()
		{
		}
		#endregion Private Constructor.

		#region Private Methods.
		/// <summary>
		/// Connects to the Database using the following parameters.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		private void ConnectToDatabase(string server, string database, string user, string password)
		{
			try
			{
                connection = new SqlConnection(DBViewerConstants.connectionString);
                command = new SqlCommand();
			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;
			}
		}

		/// <summary>
		/// Constructs the data which was extracted from the database according to user's query.
		/// </summary>
		/// <param name="reader">SqlReader - holds the queried data.</param>
		///<returns>Queried data in DataTable.</returns>
		private static DataTable ConstructData(SqlDataReader reader)
		{
			try
			{
				if(reader.IsClosed)
					throw new InvalidOperationException("Attempt to use a closed SqlDataReader");
				
				DataTable dataTable = new DataTable();
			
				// constructs the columns data.
				for(int i=0; i<reader.FieldCount; i++)
					dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));

				// constructs the table's data.
				while(reader.Read())
				{	
					object[] row = new object[reader.FieldCount];
					reader.GetValues(row);
					dataTable.Rows.Add(row);
				}
				// Culture info.
				dataTable.Locale = CultureInfo.InvariantCulture;
				// Accepts changes.
				dataTable.AcceptChanges();
				
				return dataTable;
			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;
			}
		}
		#endregion Private Methods.

		#region Public Static Method - DBViewerDal instance.
		/// <summary>
		/// DBViewerDal Instance
		/// </summary>
		/// <returns>DBViewerDal</returns>
		public static DBViewerDAL Instance()
		{
			if(instance == null)
				instance = new DBViewerDAL();
			return instance;
		}
		#endregion Public Static Method - DBViewerDal instance.

		#region Public Methods - SetConnection.
		/// <summary>
		/// Sets the underline connection.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		public void SetConnection(string server, string user, string password)
		{
			ConnectToDatabase(server, "", user, password);
		}

		/// <summary>
		///  Sets the underline connection.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		public void SetConnection(string server, string database, string user, string password)
		{
			ConnectToDatabase(server, database, user, password);
		}
		#endregion Public Methods - SetConnection.

		#region Public Methods.
		/// <summary>
		/// Gets the data from the database according to the user's query.
		/// </summary>
		/// <param name="query">query to extract the data from the database.</param>
		/// <returns>Quried data in DataTable</returns>
		public DataTable GetData(string query)
		{
			try
			{
				// opens connection. 
				command.CommandType = CommandType.Text;
				command.CommandText = query;
				command.Connection = connection;

				connection.Open();
				
				// executes the query.
				SqlDataReader reader = command.ExecuteReader();
				DataTable dataTable = ConstructData(reader);				
				
				// closes connection.
				reader.Close();
				
				return dataTable;
			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;		
			}
			finally
			{
				connection.Close();
			}
		}
		/// <summary>
		/// Saves the table to the DB.
		/// </summary>
		/// <param name="table">database table.</param>
        public void Save(DataTable table)
		{
			try
			{
				// prepares select command.
				string query = "SELECT * FROM " + table.TableName;
				
				command.CommandType = CommandType.Text;
				command.CommandText = query;
				command.Connection = connection;
				// opens connection.
				connection.Open();
				// gets transaction context.
				SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);
				command.Transaction = transaction;
				// sets the SqlCommandBuilder that constructs update, delete, insert commands.
				SqlDataAdapter dataAdapter = new SqlDataAdapter();
				dataAdapter.SelectCommand = command;
				SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

				try
				{
					DataTable changes;

					changes = table.GetChanges(DataRowState.Deleted); 
					if(changes != null)
						dataAdapter.Update(changes);
					changes = table.GetChanges(DataRowState.Modified);
					if(changes != null)
						dataAdapter.Update(changes);
					changes = table.GetChanges(DataRowState.Added);
					if(changes != null)
						dataAdapter.Update(changes);
					
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;
			}
		}
		#region IDisposable Members.
		/// <summary>
		/// Disposes from DBViewerDal
		/// </summary>
		/// <remarks>DBViewerDal uses SqlConnection which implements disposible</remarks>
		public void Dispose()
		{
			if(connection != null)
			{
				if(connection.State == ConnectionState.Open)
					connection.Close();
				connection.Dispose();
			}
		}
		#endregion IDisposable Members.
		#endregion Public Methods.
	}
}

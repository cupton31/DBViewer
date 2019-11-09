using System;
using System.Data;
using System.Collections;

using System.Globalization;

using DBViewer.Common;

namespace DBViewer.BL
{
	/// <summary>
	/// DBViewerCacheItem - represents an item in the DBViewerCache.
	/// (contains: user's credentials, user's databases and its' tables)
	/// </summary>
	public class DBViewerCacheItem
	{
		#region  Private Members.
		/// <summary>
		/// each database and its tables.
		/// </summary>
		private Hashtable data;
		/// <summary>
		/// credentials for SQL server.
		/// </summary>
		private UserCredentials credentials;
		#endregion  Private Members.

		#region Private Default Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		private DBViewerCacheItem()
		{
			
		}
		#endregion Private Default Constructor.

		#region Public Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		public DBViewerCacheItem(string user, string password)
		{
			this.data = new Hashtable();
			this.credentials = new UserCredentials(user, password);
		}
		#endregion Public Constructor.

		#region Public Property.
		/// <summary>
		/// Gets user credentials.
		/// </summary>
		public UserCredentials Credentials
		{
			get
			{
				return credentials;
			}
		}
		#endregion Public Property.

		#region Public Method.		
		/// <summary>
		/// Adds data to the cache
		/// </summary>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		public void AddData(string database, DataTable table)
		{
			try
			{
				DataSet databaseData;

				// if the database already exists in the cache.
				if(data.Contains(database))
				{
					databaseData = (DataSet)data[database];
					if(databaseData.Tables.Contains(table.TableName))
						throw new InvalidOperationException(table.TableName + " already exists.");
					else
						databaseData.Tables.Add(table.Copy());
				}
				// otherwise.
				else
				{
					databaseData = new DataSet(database);
					databaseData.Locale = CultureInfo.InvariantCulture;
					databaseData.Tables.Add(table.Copy());

					data.Add(database, databaseData);
				}
			}
			catch
			{
				throw;
			}
		}
		/// <summary>
		/// Removes the data from the cache according to its:
		/// </summary>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		public void RemoveData(string database, string table)
		{
			try
			{
				// if the database already exists in the cache.
				if(data.Contains(database))
				{
					DataSet databaseData = (DataSet)data[database];
					if(databaseData.Tables.Contains(table))
						// updating the data.
						databaseData.Tables.Remove(table);
				}
			}
			catch
			{
				throw;
			}
			
		}
		/// <summary>
		/// Gets the DataTable from the cached item.
		/// </summary>
		/// <param name="database">database name</param>
		/// <param name="table">table name</param>
		/// <returns>Cached DataTable</returns>
		public DataTable GetData(string database, string table)
		{
			try
			{
				if(data[database] != null)
				{
					DataTable tableData = ((DataSet)data[database]).Tables[table];
					if(tableData != null)
						return tableData.Copy();
					else 
						return null;
				}
				else
					return null;
			}
			catch
			{
				throw;
			}
		}
		#endregion Public Method.
	}
}

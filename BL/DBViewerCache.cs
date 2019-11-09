using System;
using System.Data;
using System.Collections;

using System.Configuration;

using DBViewer.Common;

namespace DBViewer.BL
{
	/// <summary>
	/// DBViwerCache - caches the data in the system.
	/// </summary>
	public class DBViewerCache
	{
		#region Private Constants.
		/// <summary>
		/// Cache tag.
		/// </summary>
		private const string CacheConfigTagName = "Cache";
		/// <summary>
		/// Cache attribute.
		/// </summary>
		private const string CacheConfigAttributeName = "key";
		#endregion  Private Constants.

		#region Private Static Member - DBViewerCache Instance.
		/// <summary>
		/// DBViewerCache Instance - Singelton
		/// </summary>
		public static DBViewerCache instance;
		#endregion Private Static Member - DBViewerCache Instance.

		#region Private Member.
		/// <summary>
		/// Cached Data.
		/// </summary>
		private Hashtable dataCache;
		#endregion  Private Member.

		#region Private Default Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		private DBViewerCache()
		{
			dataCache = new Hashtable();
		}
		#endregion Private Default Constructor.

		#region Public Static Method - DBViewerCache Instance.
		/// <summary>
		/// DBViewerCache Instance.
		/// </summary>
		/// <returns>DBViewerCache</returns>
		public static DBViewerCache Instance()
		{
			if(instance == null)
				instance = new DBViewerCache();
			return instance;
		}
        #endregion Public Static Method - DBViewerCache Instance.

        #region Public Methods.
        /// <summary>
        /// True if caching requested
        /// </summary>
        /// <returns>True if caching requested</returns>
        public bool IsCacheRequested()
		{	
			try
			{
				Hashtable cacheConfig = (Hashtable)ConfigurationManager.GetSection(CacheConfigTagName);
                // not parsed to int for performance reason.
                if ( (string)cacheConfig[CacheConfigAttributeName] == "1" ) 
					return true;
				else
					return false;	
			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;
			}
		}
		/// <summary>
		/// Adds sql server's DataTable to the cache.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		/// <param name="table">table</param>
		public void AddData(string server, string database, string user, string password, DataTable table)
		{
			try
			{
				if(server == null || database == null || user == null || password == null)
					throw new ArgumentNullException();

				DBViewerCacheItem dataItem;
			
				// if the server exists in the cache.
				if(dataCache.Contains(server))
				{
					// gets the DBViewerCacheItem
					dataItem = (DBViewerCacheItem)dataCache[server];
					// adds the cached table to its database.
					dataItem.AddData(database, table);
				}
				// the server added to the cache.
				else
				{
					dataItem = new DBViewerCacheItem(user, password);
					if(database.Length != 0 && table != null)
						dataItem.AddData(database, table);
					
					dataCache.Add(server, dataItem);
				}
			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;
			}
		}
		/// <summary>
		/// Removes the data from the cache according to:
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		public void RemoveData(string server, string database, string table)
		{
			try
			{
				if(server == null || database == null || table == null)
					throw new ArgumentNullException();

				DBViewerCacheItem dataItem;
			
				// if the server exists in the cache.
				if(dataCache.Contains(server))
				{
					// gets the DBViewerCacheItem
					dataItem = (DBViewerCacheItem)dataCache[server];
					dataItem.RemoveData(database, table);
				}
				else
				{
					throw new InvalidOperationException("Can't update non existing server: " + server);
				}

			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;
			}
		}
		/// <summary>
		/// Adds sql server's credentials to the cache.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="user">user</param>
		/// <param name="password">password</param>
		public void AddCredentials(string server, string user, string password)
		{
			
			try
			{
				if(server == null || user == null || password == null)
					throw new ArgumentNullException();

				// if the server exists in cache.
				if(dataCache.Contains(server))
					throw new ApplicationException(server + "'s credentials already exist");	
					// the server added to the cache.
				else
				{
					DBViewerCacheItem dataItem = new DBViewerCacheItem(user, password);					
					dataCache.Add(server, dataItem);
				}	
			}
			catch(Exception e)
			{
				Log.WriteErrorToLog(e.Message);
				throw;
			}
		}
		/// <summary>
		/// Gets a table from the sql server and a database.
		/// </summary>
		/// <param name="server">server</param>
		/// <param name="database">database</param>
		/// <param name="table">table</param>
		/// <returns></returns>
		public DataTable GetData(string server, string database, string table)
		{
			if(server == null || database == null || table == null)
				throw new ArgumentNullException();

			if(!dataCache.Contains(server))	
				return null;
			return ((DBViewerCacheItem)dataCache[server]).GetData(database, table);
		}
		/// <summary>
		/// Gets user's credentials for sql server.
		/// </summary>
		/// <param name="server">server</param>
		/// <returns>User's credentials</returns>
		public UserCredentials GetCredentials(string server)
		{
			if(server == null)
				throw new ArgumentNullException();

			if(!dataCache.Contains(server))
				throw new ApplicationException(server + " doesn't exist");
			return ((DBViewerCacheItem)dataCache[server]).Credentials;
		}
		#endregion  Public Methods.
	}
}

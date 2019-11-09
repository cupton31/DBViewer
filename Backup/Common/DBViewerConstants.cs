using System.IO;
using System.Reflection;

namespace DBViewer.Common
{
	/// <summary>
	/// DBViewerConstants - constants of DBViewer.
	/// </summary>
	public sealed class DBViewerConstants
	{

		#region Private Member - Application Name.
		/// <summary>
		/// Holds application name.
		/// </summary>
		private static string applicationName = Assembly.GetExecutingAssembly().GetName().Name;
		/// <summary>
		/// Holds application's version.
		/// </summary>
		private static string applicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

		/// <summary>
		/// Holds application's path.
		/// </summary>
		private static string applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring(8));
		#endregion Private Member - Application Name.

		#region Private Default Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		private DBViewerConstants()
		{
		}
		#endregion Private Default Constructor.

		#region Public Constants - SQL Server's types.
		#region Image Type.
		/// <summary>
		/// Image type.
		/// </summary>
		public const string ImageColumnType = "image";
		/// <summary>
		/// Image type delimiter - added to the DataTables ColumnName in order to mark column's type.
		/// </summary>
		public const string ImageColumnTypeDelimiter = "#image#";
		#endregion Image Type.

		#region Binary Type.
		/// <summary>
		/// Binary Type.
		/// </summary>
		public const string BinaryColumnType = "binary";
		/// <summary>
		/// VarBinary Type.
		/// </summary>
		public const string VarBinaryColumnType = "varbinary";
		/// <summary>
		/// Binary and VarBinary type delimiter - added to the DataTables ColumnName in order to mark column's type.
		/// </summary>
		public const string BinaryColumnTypeDelimiter = "#binary#";
		#endregion Binary Type.
		#endregion Public Constants - SQL Server's types.

		#region Public Static Properties - Application Name & Version.
		/// <summary>
		/// Gets the application name.
		/// </summary>
		/// <remarks>If retrieving application's name failed "DBViewer" string will be returned.</remarks>
		public static string ApplicationName
		{
			get
			{
				if(applicationName.Length != 0)
					return applicationName;
				return "DBViewer";
			}
		}
		/// <summary>
		/// Gets the application version.
		/// </summary>
		/// <remarks>If retrieving application's version failed - "1.0.0" string will be returned.</remarks>
		public static string ApplicationVersion
		{
			get
			{
				if(applicationVersion.Length != 0)
                    return applicationVersion;
				return "1.0.0";
			}
		}
		/// <summary>
		/// Gets the application path.
		/// </summary>
		/// <remarks>If retrieving application's path failed - "c:\\" string will be returned.</remarks>
		public static string ApplicationPath
		{
			get
			{
				if(applicationPath.Length != 0)
					return applicationPath;
				return "c:\\";
			}
		}
		/// <summary>
		/// Gets the copyright info.
		/// </summary>
		public static string Copyright
		{
			get
			{
				return "Uri Nikolaevsky";
			}
		}
		#endregion Public Properties - Application Name.

	}
}

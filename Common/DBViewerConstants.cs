using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DBViewer.Common
{
	/// <summary>
	/// DBViewerConstants - constants of DBViewer.
	/// </summary>
	public sealed class DBViewerConstants
	{
        public class TableRule
        {
            public string TableName;
            public bool ReadOnly;
            public bool hasEndpointMachineIdForeignKey;

            public TableRule (string TableName, bool CanEdit, bool hasEndpointMachineIdForeignKey)
            {
                this.TableName = TableName;
                this.ReadOnly = !CanEdit;
                this.hasEndpointMachineIdForeignKey = hasEndpointMachineIdForeignKey;
            }
        }

        public const string server = "SQL Server"; // Name in TreeView (no '\\' in TreeView names!)
        public const string user = "iadev";
        public const string password = "iadev";
        public static string connectionString;
        public static int EndpointMachine_Id;
        public static bool AdminMode;
        public static List<TableRule> UserTables = new List<TableRule>()
        {
            new TableRule("CheckIn", false, true),
            new TableRule("LogFilePull", false, true),
            new TableRule("Win32_Account", false, true),
            new TableRule("Win32_BaseBoard", false, true),
            new TableRule("Win32_BaseService", false, true),
            new TableRule("Win32_Battery", false, true),
            new TableRule("Win32_BIOS", false, true),
            new TableRule("Win32_BootConfiguration", false, true),
            new TableRule("Win32_Bus", false, true),
            new TableRule("Win32_CacheMemory", false, true),
            new TableRule("Win32_CDROMDrive", false, true),
            new TableRule("Win32_ClusterShare", false, true),
            new TableRule("Win32_ComputerSystem", false, true),
            new TableRule("Win32_CurrentProbe", false, true),
            new TableRule("Win32_DesktopMonitor", false, true),
            new TableRule("Win32_DiskDrive", false, true),
            new TableRule("Win32_DiskPartition", false, true),
            new TableRule("Win32_Fan", false, true),
            new TableRule("Win32_HeatPipe", false, true),
            new TableRule("Win32_MemoryDevice", false, true),
            new TableRule("Win32_NetworkAdapter", false, true),
            new TableRule("Win32_OperatingSystem", false, true),
            new TableRule("Win32_PageFile", false, true),
            new TableRule("Win32_ParallelPort", false, true),
            new TableRule("Win32_PhysicalMemory", false, true),
            new TableRule("Win32_PnPEntity", false, true),
            new TableRule("Win32_Processor", false, true),
            new TableRule("Win32_QuickFixEngineering", false, true),
            new TableRule("Win32_Refrigeration", false, true),
            new TableRule("Win32_Registry", false, true),
            new TableRule("Win32_ScheduledJob", false, true),
            new TableRule("Win32_SCSIController", false, true),
            new TableRule("Win32_SerialPort", false, true),
            new TableRule("Win32_Service", false, true),
            new TableRule("Win32_SMBIOSMemory", false, true),
            new TableRule("Win32_SystemDriver", false, true),
            new TableRule("Win32_TemperatureProbe", false, true),
            new TableRule("Win32_UserAccount", false, true),
            new TableRule("Win32_VideoController", false, true)
        };
        public static List<TableRule> AdminTables = new List<TableRule>()
        {
            new TableRule("CheckIn", true, true),
            new TableRule("LogFilePull", true, true),
            new TableRule("Dealer", true, false),
            new TableRule("EndpointMachine", false, false),
            new TableRule("Facility", true, false),
            new TableRule("MachineType_UpToDateDRVersion", true, false),
            new TableRule("MasterImage", true, false),
            new TableRule("Systemm", true, false),
            new TableRule("Win32_Account", false, true),
            new TableRule("Win32_BaseBoard", false, true),
            new TableRule("Win32_BaseService", false, true),
            new TableRule("Win32_Battery", false, true),
            new TableRule("Win32_BIOS", false, true),
            new TableRule("Win32_BootConfiguration", false, true),
            new TableRule("Win32_Bus", false, true),
            new TableRule("Win32_CacheMemory", false, true),
            new TableRule("Win32_CDROMDrive", false, true),
            new TableRule("Win32_ClusterShare", false, true),
            new TableRule("Win32_ComputerSystem", false, true),
            new TableRule("Win32_CurrentProbe", false, true),
            new TableRule("Win32_DesktopMonitor", false, true),
            new TableRule("Win32_DiskDrive", false, true),
            new TableRule("Win32_DiskPartition", false, true),
            new TableRule("Win32_Fan", false, true),
            new TableRule("Win32_HeatPipe", false, true),
            new TableRule("Win32_MemoryDevice", false, true),
            new TableRule("Win32_NetworkAdapter", false, true),
            new TableRule("Win32_OperatingSystem", false, true),
            new TableRule("Win32_PageFile", false, true),
            new TableRule("Win32_ParallelPort", false, true),
            new TableRule("Win32_PhysicalMemory", false, true),
            new TableRule("Win32_PnPEntity", false, true),
            new TableRule("Win32_Processor", false, true),
            new TableRule("Win32_QuickFixEngineering", false, true),
            new TableRule("Win32_Refrigeration", false, true),
            new TableRule("Win32_Registry", false, true),
            new TableRule("Win32_ScheduledJob", false, true),
            new TableRule("Win32_SCSIController", false, true),
            new TableRule("Win32_SerialPort", false, true),
            new TableRule("Win32_Service", false, true),
            new TableRule("Win32_SMBIOSMemory", false, true),
            new TableRule("Win32_SystemDriver", false, true),
            new TableRule("Win32_TemperatureProbe", false, true),
            new TableRule("Win32_UserAccount", false, true),
            new TableRule("Win32_VideoController", false, true)
        };

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

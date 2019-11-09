using System;
using System.IO;
using System.Text;

using System.Windows.Forms;

using System.Diagnostics;

namespace DBViewer.Common
{
	/// <summary>
	/// Log - logs the errors in the application.
	/// </summary>
	public sealed class Log
	{
		#region Private Constants.
		/// <summary>
		/// Log message length.
		/// </summary>
		private const int LogMessageLength = 100;
		/// <summary>
		/// Log file name.
		/// </summary>
		private static readonly string LogFileName = DBViewerConstants.ApplicationPath + "\\log.txt";
		/// <summary>
		/// Log instance name - in order to find the previous method on the stack of calls.
		/// </summary>
		private const string LogInstanceName = "DBViewer.Common.Log";
		#endregion Private Constants.

		#region Private Default Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		private Log()
		{
		}
		#endregion Private Default Constructor.
		
		#region Static Constructor.
		/// <summary>
		/// Constructor.
		/// </summary>
		static Log()
		{
			try
			{
				// creates the log or appends the text at the end.
				StreamWriter log;
				if(File.Exists(LogFileName))
					log = File.AppendText(LogFileName);
				else
					log = File.CreateText(LogFileName);
			
				Trace.Listeners.Add(new TextWriterTraceListener(log));	
			}
			catch
			{
				//writing to log shouldn't raise an exception.
			}
		}
		#endregion Static Constructor.	

		#region Public Static Methods.
		/// <summary>
		/// Writes the error to the log.
		/// </summary>
		/// <param name="message">error message</param>
		public static void WriteErrorToLog(string message)
		{
			try
			{
				// variables.
				int index = 1;
				string cls;
				string method;
				StackFrame frame;

				// gets the info of the calling method.
				StackTrace stack = new StackTrace();
				do 
				{
					frame = stack.GetFrame(index++);
					 cls = frame.GetMethod().ReflectedType.FullName;
				}
				
				while(cls == Log.LogInstanceName);
				method = frame.GetMethod().Name;

				// constructs the message.
				StringBuilder logMessage = new StringBuilder(LogMessageLength);
                logMessage.Append(DateTime.Now.ToShortDateString());
				logMessage.Append("-");
				logMessage.Append(DateTime.Now.ToLongTimeString());
				logMessage.Append(":: ");
				logMessage.Append(cls);
				logMessage.Append(".");
				logMessage.Append(method);
				logMessage.Append("()- ");
				logMessage.Append(message);

				Trace.WriteLine(logMessage.ToString(), TraceLevel.Error.ToString());
			}
			catch
			{
				//writing to log shouldn't raise an exception.
			}
		}
		/// <summary>
		/// Writes the error to the log and notifies user about the error.
		/// </summary>
		/// <param name="message">error message</param>
		public static void WriteErrorToLogAndNotifyUser(string message)
		{
			try
			{
				WriteErrorToLog(message);
				MessageBox.Show(message, DBViewerConstants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch
			{
				//writing to log shouldn't raise an exception.
			}
		}
		#endregion Public Static Methods.
	}
}

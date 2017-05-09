using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace InzoneDataSyncService
{
    public class EventLogWriter
    {
        public static void WriteToEventLog(string eventMessage, EventLogEntryType eventType)
        {
            string source = "InzoneDataSyncService";
            string log = "Application";

            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, log);

            EventLog.WriteEntry(source, eventMessage, eventType);
        }

        public static void WriteToLogFile(string contents)
        {
            try
            {
                // Create file name based on current day
                string datePart = DateTime.Now.ToString("yyyy-MM-dd");
                string logPath = Properties.Settings.Default.LogFilePath.ToString();
                string machineName = Environment.MachineName.ToString();

                // test if directory exists.
                DirectoryInfo dInfo = new DirectoryInfo(logPath);

                if (!dInfo.Exists)
                {
                    dInfo.Create();

                    string foldercreated = "Folder " + logPath + " created.";
                    EventLogWriter.WriteToEventLog(foldercreated, EventLogEntryType.Information);
                }

                string logFileName = string.Format(@"{0}\{1}-{2}-InzoneSyncLog.txt", logPath, datePart, machineName);
                string logText = DateTime.Now.ToString() + ":" + contents + "\r\n";

                File.AppendAllText(logFileName, logText);
                return;
            }
            catch (Exception ex)
            {
                EventLogWriter.WriteToEventLog(ex.Message.ToString(), EventLogEntryType.Error);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace InzoneDataSyncService
{
    public class ModemApplication
    {
        public static void TestModemApplicatinRunning()
        {
            string myModemAppProcess = Properties.Settings.Default.ModemApplicationProcess;

            Process[] pName = Process.GetProcessesByName(myModemAppProcess);

            if (pName.Length == 0)
            {
                EventLogWriter.WriteToLogFile(myModemAppProcess + " is NOT running. Attempting to start.");
                string myModemApp = Properties.Settings.Default.ModemApplicationLocation;

                try
                {
                    Process.Start(myModemApp);

                    if (pName.Length == 0)
                    {
                        EventLogWriter.WriteToLogFile(myModemAppProcess + " started successfully.");
                    }
                    else
                    {
                        EventLogWriter.WriteToLogFile(myModemAppProcess + " would not start.");

                    }
                }
                catch (Exception ex)
                {
                    EventLogWriter.WriteToLogFile("ModemApp " + ex.Message.ToString());
                }
            }
            else
            {
                EventLogWriter.WriteToLogFile(myModemAppProcess + " is running.");
            }
        }
    }
}

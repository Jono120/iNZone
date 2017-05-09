using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InzoneDataSyncService.InzoneKioskWebServiceProxy;

using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Net;

using System.Runtime.InteropServices;

namespace InzoneDataSyncService
{
    public class ServerTime
    {

        public static void SyncDateTime()
        {
            try
            {
                if (Properties.Settings.Default.SyncSystemDateTime)
                {
                    WebProxy myProxy = new WebProxy(Properties.Settings.Default.ProxyAddress, true);

                    // setup proxy
                    if (Properties.Settings.Default.UseProxy == true)
                    {
                        myProxy.Credentials = new NetworkCredential(Properties.Settings.Default.ProxyUsername, Properties.Settings.Default.ProxyPassword);
                    }
                    
                    
                    // Attach the authentication header
                    AuthHeader header = new AuthHeader();
                    header.KioskID = Environment.MachineName;
                    header.Password = DataSync.EncodePassword(Environment.MachineName);

                    InzoneKioskWebService GetTimeWebService = new InzoneKioskWebService();

                    if (Properties.Settings.Default.UseProxy == true)
                    {
                        GetTimeWebService.Proxy = myProxy;
                    }

                    GetTimeWebService.AuthHeaderValue = header;

                    EventLogWriter.WriteToLogFile("Current local PC DateTime is: " + DateTime.Now.ToString());

                    EventLogWriter.WriteToLogFile("Attempting to get DateTime from server.");
                    string TheServerDateTime = GetTimeWebService.ServerDateTime();
                    EventLogWriter.WriteToLogFile("The DateTime from the server is: " + TheServerDateTime);

                    ServerTime.SYSTEMTIME.SetLocalSystemDateTime(DateTime.Parse(TheServerDateTime));
                    EventLogWriter.WriteToLogFile("New local PC time is: " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                EventLogWriter.WriteToLogFile("SyncDateTime:" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// SYSTEMTIME structure with some useful methods
        /// </summary>
        public struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;

            /// <summary>
            /// Convert form System.DateTime
            /// </summary>
            /// <param name="time"></param>
            public void FromDateTime(DateTime time)
            {
                wYear = (ushort)time.Year;
                wMonth = (ushort)time.Month;
                wDayOfWeek = (ushort)time.DayOfWeek;
                wDay = (ushort)time.Day;
                wHour = (ushort)time.Hour;
                wMinute = (ushort)time.Minute;
                wSecond = (ushort)time.Second;
                wMilliseconds = (ushort)time.Millisecond;
            }

            //SetLocalTime C# Signature
            [DllImport("Kernel32.dll")]

            public static extern bool SetLocalTime(ref SYSTEMTIME Time);

            public static void SetLocalSystemDateTime(DateTime NewDateTime)
            {
                //Convert to SYSTEMTIME
                SYSTEMTIME st = new SYSTEMTIME();
                st.FromDateTime(NewDateTime);
                //Call Win32 API to set time
                SetLocalTime(ref st);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

using System.Data.SqlClient;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.Server;
using Microsoft.Samples.Synchronization.Data.SqlExpress;
using InzoneDataSyncService.InzoneKioskWebServiceProxy;

using System.Security;
using System.Security.Cryptography;
using System.Security.Principal;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

using System.Xml;
using System.Management;

namespace InzoneDataSyncService
{

    public partial class DataSync : ServiceBase
    {
        // SQL Client connection
        SqlConnection clientConnection = null;

        // RAS location
        private const string VPNPROCESS = "C:\\WINDOWS\\system32\\rasphone.exe";
        
        // Initalise the timer
        Timer timer = new Timer();


        public DataSync()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            EventLogWriter.WriteToLogFile("Starting Data Sync Service");
            EventLogWriter.WriteToLogFile("Timer Interval : " + Properties.Settings.Default.TimerInterval.ToString() + " milliseconds.");

            // Call OnElapsedTime after service first starts to get data upload attempt to occur immediately

            if (ManageInternetConnection() == true)
            {
                // Sync Time with Server
                ServerTime.SyncDateTime();

                EventLogWriter.WriteToLogFile("Data Sync Started - First Run");
                SynchronizeData();
                EventLogWriter.WriteToLogFile("Data Sync Complete - First Run" + "\r\n");
            }
            else 
            {
                EventLogWriter.WriteToLogFile("Second attempt to connect to Internet.");

                if (ManageInternetConnection() == true)
                {
                    // Sync Time with Server
                    ServerTime.SyncDateTime();

                    EventLogWriter.WriteToLogFile("Data Sync Started - First Run");
                    SynchronizeData();
                    EventLogWriter.WriteToLogFile("Data Sync Complete - First Run" + "\r\n");
                }
            }
            
                
            // ad 1: handle Elapsed Event
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

            //ad 2: set interval to 1 minute (= 60,000 milliseconds)
            timer.Interval = Properties.Settings.Default.TimerInterval;

            //ad 3: enabling the timer
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
            EventLogWriter.WriteToLogFile("Data Sync Service Stoped" + "\r\n");
        }
        

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            if (InSyncPeriod())
            {
                if (ManageInternetConnection() == true)
                {
                    EventLogWriter.WriteToLogFile("Data Sync Started");
                    SynchronizeData();
                    EventLogWriter.WriteToLogFile("Data Sync Complete" + "\r\n");
                }
            }
        }


        private bool InSyncPeriod()
        {
            if (Properties.Settings.Default.ExcludeWeekends)
            {
                DateTime CurrentDate = DateTime.Now;

                if (CurrentDate.DayOfWeek == DayOfWeek.Saturday || CurrentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    EventLogWriter.WriteToLogFile("Weekends Excluded.");
                    return false;
                }
                
            }
            DateTime CurrentTime = DateTime.Now;

            try
            {
                DateTime StartTime = DateTime.Parse(Properties.Settings.Default.DataSyncStartTime);
                DateTime EndTime = DateTime.Parse(Properties.Settings.Default.DataSyncEndTime);

                if (CurrentTime >= StartTime && CurrentTime <= EndTime)
                {
                    return true;
                }
                else
                {
                    EventLogWriter.WriteToLogFile("Outside Data Sync Period. " + StartTime.ToString() + " - " + EndTime.ToString());
                    return false; 
                }
            }
            catch (Exception ex)
            {
                EventLogWriter.WriteToEventLog(ex.Message.ToString(), EventLogEntryType.Error);
                return true;
            }

        }

        private void SynchronizeData()
        {
            try
            {
                // 1. Create instance of the sync components (client, agent, server)
                SyncAgent syncAgent = new SyncAgent();
                WebProxy myProxy = new WebProxy(Properties.Settings.Default.ProxyAddress, true);
                
                // setup proxy
                if (Properties.Settings.Default.UseProxy == true)
                {
                    myProxy.Credentials = new NetworkCredential(Properties.Settings.Default.ProxyUsername, Properties.Settings.Default.ProxyPassword);
                    EventLogWriter.WriteToLogFile("Using Proxy" + "\r\n");
                }

                // 2. Prepare server db connection
                InzoneKioskWebServiceProxy.InzoneKioskWebService syncWebService = new InzoneKioskWebServiceProxy.InzoneKioskWebService();

                if (Properties.Settings.Default.UseProxy == true)
                {
                    syncWebService.Proxy = myProxy;
                }

                // Write service being used to log file
                EventLogWriter.WriteToLogFile(string.Format("Web Service: {0}",syncWebService.Url.ToString()));

                // Attach the authentication header
                AuthHeader header = new AuthHeader();
                header.KioskID = Environment.MachineName;
                //header.Password = Environment.MachineName; // TODO: This will need to be stored in a config (or use some encryption
                //  algorithm that is known to both machine and server.

                header.Password = EncodePassword(Environment.MachineName);
                syncWebService.AuthHeaderValue = header;
                syncAgent.RemoteProvider = new ServerSyncProviderProxy(syncWebService);

                // 3. Prepare client db connection and attach it to the sync provider
                SqlExpressClientSyncProvider clientSyncProvider = new SqlExpressClientSyncProvider();
                syncAgent.LocalProvider = clientSyncProvider;

                clientConnection = new SqlConnection(Properties.Settings.Default.DefaultConnectionString);
                clientSyncProvider.Connection = clientConnection;
                
                // 4. Create SyncTables and SyncGroups

                // Participant
                SyncTable tableParticipant = new SyncTable("Participant");
                tableParticipant.CreationOption = TableCreationOption.UseExistingTableOrFail;
				tableParticipant.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.Bidirectional;

                // Interaction
                SyncTable tableInteraction = new SyncTable("Interaction");
                tableInteraction.CreationOption = TableCreationOption.UseExistingTableOrFail;
                tableInteraction.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.Bidirectional;

                // Interaction Video
                SyncTable tableInteractionVideo = new SyncTable("InteractionVideo");
                tableInteractionVideo.CreationOption = TableCreationOption.UseExistingTableOrFail;
                tableInteractionVideo.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.Bidirectional;

				//BadWordFilter
				SyncTable tableBadWordFilter = new SyncTable("BadWordFilter");
				tableBadWordFilter.CreationOption = TableCreationOption.UseExistingTableOrFail;
                tableBadWordFilter.SyncDirection = Microsoft.Synchronization.Data.SyncDirection.Bidirectional;

                // Sync changes for both tables as one bunch, using SyncGroup object
                // This is important if the tables has PK-FK relationship, grouping will ensure that 
                // and FK change won't be applied before its PK is applied
                SyncGroup orderGroup = new SyncGroup("AllChanges");
                
				tableParticipant.SyncGroup = orderGroup;
				tableInteraction.SyncGroup = orderGroup;
				tableInteractionVideo.SyncGroup = orderGroup;
				tableBadWordFilter.SyncGroup = orderGroup;

                syncAgent.Configuration.SyncTables.Add(tableParticipant);
                syncAgent.Configuration.SyncParameters.Add(new Microsoft.Synchronization.Data.SyncParameter("@KioskID", Environment.MachineName));
                syncAgent.Configuration.SyncTables.Add(tableInteraction);
                syncAgent.Configuration.SyncTables.Add(tableInteractionVideo);
				syncAgent.Configuration.SyncTables.Add(tableBadWordFilter);

                // 5. Kickoff sync process                
                // Setup the progress form and sync progress event handler  
                clientSyncProvider.SyncAdapters.Add(TableAdapter.ParticipantSyncAdapter());
                clientSyncProvider.SyncAdapters.Add(TableAdapter.InteractionSyncAdapter());
                clientSyncProvider.SyncAdapters.Add(TableAdapter.InteractionVideoSyncAdapter());
				clientSyncProvider.SyncAdapters.Add(TableAdapter.BadWordFilterSyncAdapter());

                // select new anchor command                
                SqlCommand anchorCmd = new SqlCommand();
                anchorCmd.CommandType = CommandType.Text;
                anchorCmd.CommandText = "Select @" + Microsoft.Synchronization.Data.SyncSession.SyncNewReceivedAnchor + " = @@DBTS";  // for SQL Server 2005 SP2, use "min_active_rowversion() - 1"
                anchorCmd.Parameters.Add("@" + Microsoft.Synchronization.Data.SyncSession.SyncNewReceivedAnchor, SqlDbType.Timestamp).Direction = ParameterDirection.Output;

                clientSyncProvider.SelectNewAnchorCommand = anchorCmd;

                //clientSyncProvider.ApplyChangeFailed += new EventHandler<ApplyChangeFailedEventArgs>(ShowFailures);
                SyncStatistics syncStats = syncAgent.Synchronize();

                // write sync stats to log file
                if (syncStats.TotalChangesUploaded > 0 || syncStats.UploadChangesApplied > 0 || syncStats.UploadChangesFailed > 0)
                {
					string syncMessage;
					syncMessage = "Total Changes Uploaded : " + syncStats.TotalChangesUploaded.ToString() + "\r\n";
					syncMessage += "Upload Changes Applied : " + syncStats.UploadChangesApplied.ToString() + "\r\n";
					syncMessage += "Upload Changes Failed : " + syncStats.UploadChangesFailed.ToString() + "\r\n";
					syncMessage += "Total Changes Downloaded : " + syncStats.TotalChangesDownloaded.ToString() + "\r\n";
					syncMessage += "Download Changes Applied : " + syncStats.DownloadChangesApplied.ToString() + "\r\n";
					syncMessage += "Download Changes Failed : " + syncStats.DownloadChangesFailed.ToString() + "\r\n";
					EventLogWriter.WriteToLogFile(syncMessage);
                }
                else
                {
                    EventLogWriter.WriteToLogFile("No Data to sync.");
                }
            }
            catch (Exception ex)
            {
                EventLogWriter.WriteToEventLog(ex.Message.ToString(), EventLogEntryType.Error);
            }
        }

        public static string EncodePassword(string originalPassword)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            // Convert the original password to Bytes; then create the hash
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            // Bytes to string
            return System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(encodedBytes), "-", "").ToLower();
        }

        private bool ManageInternetConnection()
        {
            bool isConnected = ConnectionExists("1");

            if (isConnected == false)
            {
                if (Properties.Settings.Default.UseRAS)
                {
                    try
                    {
                        if (Properties.Settings.Default.UseModemApplication)
                        {
                            ModemApplication.TestModemApplicatinRunning();
                        }
                        
                        string RASConnectionName = Properties.Settings.Default.RASConnectionName;
                        string phoneBookPath = Properties.Settings.Default.RASphoneBookPath;

                        string args = @" -f " + "\"" + phoneBookPath + "\"" + " -d " + "\"" + RASConnectionName + "\"";

                        try
                        {
                            EventLogWriter.WriteToLogFile("Initiating RAS Connection");

                            /*
                            ProcessStartInfo myProcess = new ProcessStartInfo(VPNPROCESS);
                            myProcess.Arguments = args;
                            myProcess.UserName = "inzone";
                            myProcess.Password = myPassword;
                            myProcess.Domain = "";
                            myProcess.UseShellExecute = false;
                            Process.Start(myProcess);
                            System.Threading.Thread.Sleep(20000);
                            */
                            
                            Process.Start(VPNPROCESS, args);
                            //System.Threading.Thread.Sleep(60000);
                            System.Threading.Thread.Sleep(Properties.Settings.Default.RASConnectionPause);

                        }
                        catch (Exception ex)
                        { 
                            EventLogWriter.WriteToLogFile("VPNProcess " + ex.Message.ToString());
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        EventLogWriter.WriteToLogFile(ex.Message.ToString());
                    }
                }
                else
                {
                    EventLogWriter.WriteToLogFile("RAS Disabled. Can not get Network Connection.");
                }
                isConnected = ConnectionExists("2");
            }
            return isConnected;
        }

        private bool ConnectionExists(string attempt)
        {
            WebRequest request = WebRequest.Create(Properties.Settings.Default.RASTestURL);
            
            try
            {
                WebProxy myProxy = new WebProxy(Properties.Settings.Default.ProxyAddress, true);

                // setup proxy
                if (Properties.Settings.Default.UseProxy == true)
                {
                    myProxy.Credentials = new NetworkCredential(Properties.Settings.Default.ProxyUsername, Properties.Settings.Default.ProxyPassword);
                    request.Proxy = myProxy;
                }

                WebResponse response = request.GetResponse();
                response.Close();
                EventLogWriter.WriteToLogFile(string.Format("Internet Connection Exists. (Attempt {0})",attempt));
                return true;
            }
            catch
            {
                // Write Connection not exist to log file
                EventLogWriter.WriteToLogFile(string.Format("Cannot connect to: {0}. (Attempt {1})", Properties.Settings.Default.RASTestURL,attempt));
                return false; // host not reachable. 
            }
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.Server;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace InzoneKioskWebservice
{
	/// <summary>
	/// Webservice to synchronize tables between kiosks and master database using
	/// MS Sync Framework
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class InzoneKioskWebService : System.Web.Services.WebService
	{
		private DbServerSyncProvider _serverProvider;

		/// <summary>
		/// Used to authenticate each of the web service calls
		/// </summary>
		public class AuthHeader : SoapHeader
		{
			public string KioskID;
			public string Password;
		}

		/// <summary>
		/// Special SOAP header object used to authenticate users connecting to this web service.
		/// </summary>
		public AuthHeader Authentication;

		private void Authenticate()
		{
			// Check that authentication information has been supplied.
			if (Authentication == null)
			{
				EventLogWriter.WriteToLogFile("Missing authentication information");
				throw new Exception("Missing authentication information.  Unable to execute web method.");
			}
			EventLogWriter.WriteToLogFile(string.Format("Kiosk: {0}, Password: {1}", Authentication.KioskID, Authentication.Password));

			// Validate the authentication
			string kioskID = Authentication.KioskID;
			if (Authentication.Password != EncodePassword(kioskID))
			{
				EventLogWriter.WriteToLogFile("Invalid password information.  Unable to execute web method.");
				throw new Exception("Invalid password information.  Unable to execute web method.");
			}
		}

		private static string EncodePassword(string originalPassword)
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


		public InzoneKioskWebService()
		{
			_serverProvider = new DbServerSyncProvider();

			// 1. Prepare server db connection and attach it to the sync agent

			//SqlConnection serverConnection = new SqlConnection(Properties.Settings.Default.DefaultConnectionString);
			SqlConnection serverConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["InzoneKioskWebserviceConnectionString"].ConnectionString);

			_serverProvider.Connection = serverConnection;

			// 2. Create sync adapter for each sync table and attach it to the agent
			_serverProvider.SyncAdapters.Add(TableAdapter.ParticipantSyncAdapter());
			_serverProvider.SyncAdapters.Add(TableAdapter.InteractionSyncAdapter());
			_serverProvider.SyncAdapters.Add(TableAdapter.InteractionVideoSyncAdapter());
			_serverProvider.SyncAdapters.Add(TableAdapter.BadWordFilterSyncAdapter());

			// 3. Setup provider wide commands

			// select new anchor command
			SqlCommand anchorCmd = new SqlCommand();
			anchorCmd.CommandType = CommandType.Text;
			anchorCmd.CommandText = "Select @" + SyncSession.SyncNewReceivedAnchor + " = @@DBTS";  // for SQL Server 2005 SP2, use "min_active_rowversion() - 1"
			anchorCmd.Parameters.Add("@" + SyncSession.SyncNewReceivedAnchor, SqlDbType.Timestamp).Direction = ParameterDirection.Output;

			_serverProvider.SelectNewAnchorCommand = anchorCmd;
		}

		/// <summary>
		///  Access server sync parameters
		/// </summary>
		/// <returns>ServerInfo struct</returns>
		[SoapHeader("Authentication")]
		[WebMethod]
		public SyncServerInfo GetServerInfo(SyncSession session)
		{
			// Authenticate this request
			Authenticate();

			return _serverProvider.GetServerInfo(session);
		}


		/// <summary>
		/// Access table schema in DataSet form
		/// </summary>
		/// <param name="tableNames">Table names to bring the schema for</param>       
		/// <returns>Schema information object</returns>
		[SoapHeader("Authentication")]
		[WebMethod]
		public SyncSchema GetSchema(Collection<string> tableNames, SyncSession session)
		{
			// Authenticate this request
			Authenticate();

			return _serverProvider.GetSchema(tableNames, session);
		}


		/// <summary>
		/// Enumerate group changes
		/// </summary>
		/// <param name="groupMetadata">Sync group metadata object</param>
		/// <param name="syncSession">Session variables</param>
		/// <returns>SyncContext with data table for changes in each sync table in the group</returns>
		[SoapHeader("Authentication")]
		[WebMethod]
		public SyncContext GetChanges(SyncGroupMetadata groupMetadata, SyncSession syncSession)
		{
			// Authenticate this request
			Authenticate();

			return _serverProvider.GetChanges(groupMetadata, syncSession);
		}


		/// <summary>
		/// Apply group changes 
		///  Soft errors (i.e. Data exceptions and conflicts) are returned in the form of SyncError array.
		///  Hard errors are reported via exceptions
		/// </summary>
		/// <param name="groupMetadata">Sync group metadata object</param>
		/// <param name="dataSet">Changes for each table in the group to apply at the server</param>
		/// <param name="syncSession">Session variables</param>
		/// <returns>SyncContext with sync exceptions and conflicts encountered during application of changes</returns>
		[SoapHeader("Authentication")]
		[WebMethod]
		public SyncContext ApplyChanges(SyncGroupMetadata groupMetadata, DataSet dataSet, SyncSession syncSession)
		{
			// Authenticate this request
			Authenticate();

			return _serverProvider.ApplyChanges(groupMetadata, dataSet, syncSession);
		}

		/// <summary>
		/// Get DateTime from server
		/// </summary>
		[SoapHeader("Authentication")]
		[WebMethod]
		public string ServerDateTime()
		{
			// Authenticate this request
			Authenticate();

			return DateTime.Now.ToString();
		}
	}
}

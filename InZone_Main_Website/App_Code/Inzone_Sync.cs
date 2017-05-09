using System;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

/// <summary>
/// Summary description for Inzone_Sync
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Inzone_Sync: System.Web.Services.WebService {

	public struct SyncJob {
		public int ID;
		public string Table;
		public string SQL;
		public string LocalSQL;
		public string ExecutionMessage;
		public bool Success;
	}
	public Inzone_Sync() {

		//Uncomment the following line if using designed components 
		//InitializeComponent(); 
	}

	[WebMethod]
	public SyncJob[] ProcessSyncJobs(SyncJob[] jobs) {
		SqlConnection conn = null;
		SqlCommand comm = null;
		try {
			conn = new SqlConnection(
				ConfigurationManager.ConnectionStrings["InzoneMirrorConnectionString"].ConnectionString);
			conn.Open();
			comm = conn.CreateCommand();
			comm.CommandType = System.Data.CommandType.Text;
			for (int j = 0; j < jobs.Length; j++) {
				comm.CommandText = jobs[j].SQL;
				try {
					writeToLog(DateTime.Now.ToString() + ": " + jobs[j].SQL);
					int rowsAffected = comm.ExecuteNonQuery();
					jobs[j].SQL = string.Empty;
					if (rowsAffected == 0) {
						jobs[j].Success = false;
						jobs[j].ExecutionMessage = "SQL statement affected 0 rows";
						jobs[j].LocalSQL = "INSERT INTO SyncErrors (TableName, RecordID, ServerMessage) " +
							"('" + jobs[j].Table + "', " + jobs[j].ID.ToString() + ", '" + jobs[j].ExecutionMessage + "')";
					} else {
						jobs[j].Success = true;
						jobs[j].LocalSQL = "UPDATE " + jobs[j].Table + " SET " +
														"Sync = 1, LastSync = '" + DateTime.Now.ToString("d-MMM-yyyy hh:mm:ss") + "' " +
							"WHERE ID = " + jobs[j].ID.ToString();
					}
				} catch (Exception jobException) {
					jobs[j].Success = false;
					jobs[j].ExecutionMessage = jobException.Message;
					jobs[j].LocalSQL = "INSERT INTO SyncErrors (TableName, RecordID, ServerMessage) " +
						"('" + jobs[j].Table + "', " + jobs[j].ID.ToString() + ", '" + jobException.Message.Replace("'", "''") + "')";
				} finally {
					if (!jobs[j].Success) {
						comm.CommandText = jobs[j].LocalSQL;
						writeToLog(DateTime.Now.ToString() + ": " + jobs[j].LocalSQL);
						comm.ExecuteNonQuery();
					}
				}
			}
		} catch (Exception ex) {
			writeToLog(
				"Exception in inzone.co.nz::Inzone_Sync::ProcessSyncJobs()\r\n" +
				DateTime.Now.ToString() + "\r\n" +
				"Exception: " + ex.Message + "\r\n" +
				"Stack: " + ex.StackTrace
			);
		} finally {
			if (comm != null) comm.Dispose();
			if (conn != null) conn.Dispose();
		}
		return jobs;
	}

	/// <summary>
	/// Takes a bunch of Sync jobs and executes them against the database connection
	/// </summary>
	/// <param name="jobs">And array of SyncJobs</param>
	/// <returns></returns>
	[WebMethod]
	public SyncJob[] ProcessSyncJobsV2(SyncJob[] jobs) {
		SqlConnection conn = null;
		SqlCommand comm = null;
		try {

			// establish db connection and create a command object
			conn = new SqlConnection(
				ConfigurationManager.ConnectionStrings["InzoneMirrorConnectionString"].ConnectionString);
			conn.Open();
			comm = conn.CreateCommand();
			comm.CommandType = System.Data.CommandType.Text;

			// process each SyncJob in turn
			for (int j = 0; j < jobs.Length; j++) {

				// store the original SQL statement
				string originalSql = jobs[j].SQL;
				// there maybe several statements here,
				// so we need to split them off into array
				string[] commands = originalSql.Split(
					new string[] { "/@@/" }, StringSplitOptions.RemoveEmptyEntries);

				// set the SQL field to empty - we retransmit
				// back to the caller - so we try and minimize
				// bandwidth needed
				jobs[j].SQL = string.Empty;
				jobs[j].Success = false;	// default state is failed

				// process each command 
				for (int c = 0; c < commands.Length; c++) {

					// execute the command
					try {
						comm.CommandText = commands[c];
						int rowsAffected = comm.ExecuteNonQuery();

						if (rowsAffected != 0) {
							// affected some row(s) so assume success!
							jobs[j].Success = true;
							jobs[j].ExecutionMessage = c.ToString();
							jobs[j].LocalSQL = "UPDATE "
								+ jobs[j].Table + " SET " + "Sync = 1, LastSync = '"
								+ DateTime.Now.ToString("d-MMM-yyyy hh:mm:ss")
								+ "' " + "WHERE ID = " + jobs[j].ID.ToString();
							break;
						}
					} catch (Exception commandException) {
						// log any exceptions we catch for process of bug finding
						jobs[j].ExecutionMessage = "Exception: " + commandException.Message + ". ";
						writeToLog("This statement caused an exception: " + commands[c] + "\r\n" +
							"Exception message: " + commandException.Message);
					}

				}

				// if there was a failure, then none of the commands
				// affected any rows so we must report this
				if (!jobs[j].Success) {
					jobs[j].ExecutionMessage += "Statement(s) affected 0 rows!";
					// generate SQL for remote error log
					jobs[j].LocalSQL =
						"INSERT INTO SyncErrors (TableName, RecordID, ServerMessage) VALUES "
						+ "('" + jobs[j].Table + "', " + jobs[j].ID.ToString() + ", '"
						+ jobs[j].ExecutionMessage.Replace("'", "''") + "')";
					// log on local server
					writeToLog("A SQL statement affected 0 rows:\r\n"
						+ "Original SQL: " + originalSql);
				}
			}
		} catch (Exception ex) {
			writeToLog(
				"Exception in inzone.co.nz::Inzone_Sync::ProcessSyncJobs()\r\n" +
				"Exception: " + ex.Message + "\r\n" +
				"Stack: " + ex.StackTrace
			);
		} finally {
			if (comm != null) comm.Dispose();
			if (conn != null) conn.Dispose();
		}
		return jobs;
	}
	private void writeToLog(string text) {
		string log_file = Server.MapPath("~/logs");
		if (!log_file.EndsWith("\\")) log_file += "\\";
		log_file += "syncerrors.txt";
		File.AppendAllText(log_file, DateTime.Now.ToString() + ": " + text + "\r\n\r\n");
	}
}


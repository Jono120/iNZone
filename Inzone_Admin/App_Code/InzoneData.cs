using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Inzone {

	public struct ParticipantInfo {
		public int ID;
		public string Firstname;
		public string Lastname;
		public string Email;
		public string Address1;
		public string Address2;
		public string Suburb;
		public string Town;
		public string Association;
		public int Age;
		public DateTime DOB;
		public string Gender;
		public string ContactPhone;
		public int RegistrationCode;
		public bool LoggedIn;
		public bool KnowsCareer;
		public int LastCustomerID;
	};

	public class InzoneData: IDisposable {

		private SqlConnection _databaseConnection = null;

		public void Test() {
			SqlCommand comm = _databaseConnection.CreateCommand();
			comm.CommandType = System.Data.CommandType.StoredProcedure;
			comm.CommandText = "WARNING_Test";
			comm.Parameters.AddWithValue("@DateTime", DateTime.Now);
			comm.ExecuteNonQuery();
			comm.Dispose();
		}

		public InzoneData(string connectionStringName) {
			_databaseConnection = new SqlConnection(
					ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
			_databaseConnection.Open();
		}

		public int CustomerInsert(string name) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				SqlParameter returnValue = new SqlParameter("@returnvalue", System.Data.SqlDbType.Int);
				returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "Customer_Insert";
				comm.Parameters.AddWithValue("@Name", name);
				comm.Parameters.Add(returnValue);
				comm.ExecuteNonQuery();
				return (int)comm.Parameters["@returnvalue"].Value;
			} catch {
				return -1;
			} finally {
				comm.Dispose();
			}
		}

		public int EventInsert(string name, DateTime eventDate, bool isCurrent) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				SqlParameter returnValue = new SqlParameter("@returnvalue", System.Data.SqlDbType.Int);
				returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "Event_Insert";
				comm.Parameters.AddWithValue("@Name", name);
				comm.Parameters.AddWithValue("@EventDate", eventDate);
				comm.Parameters.AddWithValue("@Current", isCurrent);
				comm.Parameters.Add(returnValue);
				comm.ExecuteNonQuery();
				return (int)comm.Parameters["@returnvalue"].Value;
			} catch {
				return -1;
			} finally {
				comm.Dispose();
			}
		}

		public string GetActiveEventName() {
			SqlCommand comm = _databaseConnection.CreateCommand();
			SqlDataReader dr = null;
			try {
				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "Event_GetActive";
				dr = comm.ExecuteReader();
				if (dr.Read()) {
					return dr["Name"].ToString();
				} else {
					return "Unable to obtain current event name";
				}
			} catch {
				return "Unable to obtain current event name";
			} finally {
				if (dr != null) {
					dr.Close();
					dr.Dispose();
				}
				comm.Dispose();
			}
		}

		public int ParticipantInsert(string firstname, string lastname, string email,
				string address1, string address2, string suburb, string town, string association,
				string age, string dob, string gender, string contactPhone, string careerDecided, string customerId) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				SqlParameter returnValue = new SqlParameter("@returnvalue", System.Data.SqlDbType.Int);
				returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "Participant_Insert";
				comm.Parameters.Add(returnValue);
				comm.Parameters.AddWithValue("@Firstname", firstname);
				comm.Parameters.AddWithValue("@Lastname", lastname);
				comm.Parameters.AddWithValue("@Email", email);
				comm.Parameters.AddWithValue("@Address1", address1);
				comm.Parameters.AddWithValue("@Address2", nullableString(address2));
				comm.Parameters.AddWithValue("@Suburb", nullableString(suburb));
				comm.Parameters.AddWithValue("@Town", town);
				comm.Parameters.AddWithValue("@Association", nullableString(association));
				comm.Parameters.AddWithValue("@Age", stringToNullableInt(age));
				comm.Parameters.AddWithValue("@DOB", DateTime.Parse(dob));
				comm.Parameters.AddWithValue("@Gender", nullableString(gender));
				comm.Parameters.AddWithValue("@ContactPhone", nullableString(contactPhone));
				comm.Parameters.AddWithValue("@KnowsCareer", (careerDecided.Trim() == "1" ? 1 : 0));
				comm.Parameters.AddWithValue("@CustomerID", stringToNullableInt(customerId));
				comm.ExecuteNonQuery();
				return (int)comm.Parameters["@returnvalue"].Value;
			} catch {
				throw;
				//return 0;
			} finally {
				comm.Dispose();
			}
		}

		public bool ParticipantGetInfo(int participantId, out ParticipantInfo pi) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			SqlDataReader dr = null;
			pi = new ParticipantInfo();
			try {

				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "Participant_Get";
				comm.Parameters.AddWithValue("@ParticipantID", participantId);
				dr = comm.ExecuteReader();
				if (!dr.Read()) {
					return false;
				} else {
					pi.ID = (int)dr["ID"];
					pi.Firstname = dr["Firstname"].ToString();
					pi.Lastname = dr["Lastname"].ToString();
					pi.Email = dr["Email"].ToString();
					pi.Address1 = nullObjectToString(dr["Address1"]);
					pi.Address2 = nullObjectToString(dr["Address2"]);
					pi.Suburb = nullObjectToString(dr["Suburb"]);
					pi.Town = nullObjectToString(dr["Town"]);
					pi.Association = nullObjectToString(dr["Association"]);
					pi.Age = nullObjectToInt(dr["Age"]);
					pi.DOB = (dr["DOB"] == DBNull.Value ? DateTime.Now : (DateTime)dr["DOB"]);
					pi.Gender = dr["Gender"].ToString();
					pi.ContactPhone = nullObjectToString(dr["ContactPhone"]);
					pi.RegistrationCode = nullObjectToInt(dr["RegistrationCode"]);
					pi.LoggedIn = Convert.ToBoolean(dr["LoggedIn"]);
					pi.KnowsCareer = (dr["KnowsCareer"] == DBNull.Value ? false : Convert.ToBoolean(dr["KnowsCareer"]));
					pi.LastCustomerID = nullObjectToInt(dr["LastCustomerID"]);
					return true;
				}

			} catch {
				return false;
			} finally {
				if (dr != null) {
					dr.Close();
					dr.Dispose();
				}
				comm.Dispose();
			}
		}

		public int ParticipantLogIn(int registrationCode, int customerId) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				SqlParameter returnValue = new SqlParameter("@returnvalue", System.Data.SqlDbType.Int);
				returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "Participant_LogIn";
				comm.Parameters.Add(returnValue);
				comm.Parameters.AddWithValue("@RegistrationCode", registrationCode);
				comm.Parameters.AddWithValue("@CustomerID", customerId);
				comm.ExecuteNonQuery();
				return (int)comm.Parameters["@returnvalue"].Value;
			} catch {
				return 0;
			} finally {
				comm.Dispose();
			}
		}

		public bool ParticipantLogOut(int participantId, bool info) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "Participant_LogOut";
				comm.Parameters.AddWithValue("@ParticipantID", participantId);
				comm.Parameters.AddWithValue("@Info", info);
				comm.ExecuteNonQuery();
				return true;
			} catch {
				throw;
				//return false;
			} finally {
				comm.Dispose();
			}
		}

		public int GenerateRegistrationCode() {
			SqlCommand comm = CreateCommandRV();
			try {
				comm.CommandType = CommandType.StoredProcedure;
				comm.CommandText = "RegistrationCode_Generate";
				comm.ExecuteNonQuery();
				return (int)comm.Parameters["@returnvalue"].Value;
			} catch {
				return -1;
			} finally {
				comm.Dispose();
			}
		}

		public int GenerateRegistrationCodeForParticipant(int participantID) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				int registrationCode = GenerateRegistrationCode();
				if (registrationCode == -1) return -1;

				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "RegistrationCode_Update";
				comm.Parameters.AddWithValue("@ParticipantID", participantID);
				comm.Parameters.AddWithValue("@RegistrationCode", registrationCode);
				comm.ExecuteNonQuery();
				return registrationCode;
			} catch {
				return -1;
			} finally {
				comm.Dispose();
			}
		}

		public bool InteractionSubscribe(int participantId, int customerId, bool value) {
			// updates an interaction record to
			// indicate whether they want to receive
			// further communication from Inzone/Customer
			// - this is needed for new mailing system
			// - returns true on success
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				comm.CommandType = CommandType.StoredProcedure;
				comm.CommandText = "Interaction_Subscribe";
				comm.Parameters.AddWithValue("@ParticipantID", participantId);
				comm.Parameters.AddWithValue("@CustomerID", customerId);
				comm.Parameters.AddWithValue("@Subscribed", value);
				comm.ExecuteNonQuery();
				return true;
			} catch (Exception subEx) {
				Utils.writeToErrorLog("InteractionSubscribe(): " + subEx.Message);
				return false; // query failed for some reason
			} finally {
				comm.Dispose();
			}
		}

		public int InteractionInsert(int participantID, int customerID) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				SqlParameter returnValue = new SqlParameter("@returnvalue", System.Data.SqlDbType.Int);
				returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "Interaction_Insert";

				comm.Parameters.AddWithValue("@ParticipantID", participantID);
				comm.Parameters.AddWithValue("@CustomerID", customerID);
				comm.Parameters.Add(returnValue);
				comm.ExecuteNonQuery();
				return (int)comm.Parameters["@returnvalue"].Value;
			} catch {
				return -1;
			} finally {
				comm.Dispose();
			}
		}

		/// <summary>
		/// I MUST BE CALLED FIRST before any other Sync* operations
		/// </summary>
		public int SyncCustomers(DataSet customers, out int[] badIds, out int[] goodIds) {
			Debug.Print("Syncing Customers...");

			// I MUST BE CALLED FIRST before any other Sync* operations
			int totalRecords = customers.Tables[0].Rows.Count;
			int recordsTransfered = 0;
			string serverMessage;
			Collection<int> cbadIds = new Collection<int>(), cgoodIds = new Collection<int>();
			SqlCommand comm = CreateCommandRV();
			comm.CommandType = CommandType.StoredProcedure;
			comm.CommandText = "Sync_Customers_Insert";
			comm.Parameters.AddWithValue("@ID", -1);
			comm.Parameters.AddWithValue("@Name", String.Empty);

			foreach (DataRow row in customers.Tables[0].Rows) {
				comm.Parameters["@returnvalue"].Value = DBNull.Value;
				comm.Parameters["@ID"].Value = row["ID"];
				comm.Parameters["@Name"].Value = row["Name"];
				try {
					comm.ExecuteNonQuery();
					cgoodIds.Add((int)row["ID"]);
					recordsTransfered++;
				} catch (Exception ex) {
					// error
					Debug.Print("Customers sync error (ID: " + row["ID"].ToString() + "): " + ex.Message);
					cbadIds.Add((int)row["ID"]);
					WriteToLog(recordsTransfered, totalRecords, "Error updating Customer record ID = " +
							row["ID"].ToString() + " -- Query returned " + comm.Parameters["@returnvalue"].Value.ToString());
				}
			}
			if (recordsTransfered == totalRecords) {
				Debug.Print("All Customer records syncronised successfully");
				serverMessage = "All Customer records syncronised successfully";
			} else if (recordsTransfered == 0) {
				Debug.Print("No Customer records were syncronised");
				serverMessage = "No Customer records were syncronised";
			} else {
				Debug.Print("Some Customer records were syncronised successfully");
				serverMessage = "Some Customer records were syncronised successfully";
			}
			WriteToLog(recordsTransfered, totalRecords, serverMessage);
			badIds = new int[cbadIds.Count];
			goodIds = new int[cgoodIds.Count];
			cbadIds.CopyTo(badIds, 0);
			cgoodIds.CopyTo(goodIds, 0);
			return (cbadIds.Count == 0 ? 1 : 0);
		}

		/// <summary>
		/// I must not be called before SyncCustomers() !!
		/// </summary>
		public int SyncEvents(DataSet events, out int[] badIds, out int[] goodIds) {
			Debug.Print("Syncing Events...");
			// I must not be called before SyncCustomers() !!
			int totalRecords = events.Tables[0].Rows.Count;
			int recordsTransfered = 0;
			string serverMessage;
			Collection<int> cbadIds = new Collection<int>(), cgoodIds = new Collection<int>();
			SqlCommand comm = CreateCommandRV();
			comm.CommandType = CommandType.StoredProcedure;
			comm.CommandText = "Sync_Events_Insert";
			comm.Parameters.AddWithValue("@ID", -1);
			comm.Parameters.AddWithValue("@Name", String.Empty);
			comm.Parameters.AddWithValue("@Current", false);
			comm.Parameters.AddWithValue("@EventDate", DateTime.Now);

			foreach (DataRow row in events.Tables[0].Rows) {
				comm.Parameters["@returnvalue"].Value = DBNull.Value;
				comm.Parameters["@ID"].Value = row["ID"];
				comm.Parameters["@Name"].Value = row["Name"];
				comm.Parameters["@Current"].Value = row["Current"];
				comm.Parameters["@EventDate"].Value = row["EventDate"];
				try {
					comm.ExecuteNonQuery();
					cgoodIds.Add((int)row["ID"]);
					recordsTransfered++;
				} catch (Exception ex) {
					// error
					Debug.Print("Events sync error (ID: " + row["ID"].ToString() + "): " + ex.Message);
					cbadIds.Add((int)row["ID"]);
					WriteToLog(recordsTransfered, totalRecords, "Error updating Event record ID = " +
							row["ID"].ToString() + " -- Query returned " + comm.Parameters["@returnvalue"].Value.ToString());
				}
			}
			if (recordsTransfered == totalRecords) {
				Debug.Print("All Event records syncronised successfully");
				serverMessage = "All Event records syncronised successfully";
			} else if (recordsTransfered == 0) {
				Debug.Print("No Event records were syncronised");
				serverMessage = "No Event records were syncronised";
			} else {
				Debug.Print("Some Event records were syncronised successfully");
				serverMessage = "Some Event records were syncronised successfully";
			}
			WriteToLog(recordsTransfered, totalRecords, serverMessage);
			badIds = new int[cbadIds.Count];
			goodIds = new int[cgoodIds.Count];
			cbadIds.CopyTo(badIds, 0);
			cgoodIds.CopyTo(goodIds, 0);
			return (cbadIds.Count == 0 ? 1 : 0);
		}

		/// <summary>
		/// I must not be called before SyncEvents() !!
		/// </summary>
		public int SyncParticipants(DataSet participants, out int[] badIds, out int[] goodIds) {
			Debug.Print("Syncing Participants...");
			// I must not be called before SyncEvents() !!
			int totalRecords = participants.Tables[0].Rows.Count;
			int recordsTransfered = 0;
			string serverMessage;
			Collection<int> cbadIds = new Collection<int>(), cgoodIds = new Collection<int>();
			SqlCommand comm = CreateCommandRV();
			comm.CommandType = CommandType.StoredProcedure;
			comm.CommandText = "Sync_Participants_Insert";
			comm.Parameters.AddWithValue("@ID", -1);
			comm.Parameters.AddWithValue("@Firstname", String.Empty);
			comm.Parameters.AddWithValue("@Lastname", String.Empty);
			comm.Parameters.AddWithValue("@Email", String.Empty);
			comm.Parameters.AddWithValue("@Address1", String.Empty);
			comm.Parameters.AddWithValue("@Address2", DBNull.Value);
			comm.Parameters.AddWithValue("@Suburb", DBNull.Value);
			comm.Parameters.AddWithValue("@Town", String.Empty);
			comm.Parameters.AddWithValue("@Association", DBNull.Value);
			comm.Parameters.AddWithValue("@Age", DBNull.Value);
			comm.Parameters.AddWithValue("@DOB", DBNull.Value);
			comm.Parameters.AddWithValue("@Gender", DBNull.Value);
			comm.Parameters.AddWithValue("@ContactPhone", DBNull.Value);
			comm.Parameters.AddWithValue("@RegistrationCode", DBNull.Value);
			comm.Parameters.AddWithValue("@LoggedIn", false);
			comm.Parameters.AddWithValue("@KnowsCareer", DBNull.Value);
			comm.Parameters.AddWithValue("@LastCustomerID", DBNull.Value);
			comm.Parameters.AddWithValue("@Info", DBNull.Value);

			foreach (DataRow row in participants.Tables[0].Rows) {
				comm.Parameters["@returnvalue"].Value = DBNull.Value;
				comm.Parameters["@ID"].Value = row["ID"];
				comm.Parameters["@Firstname"].Value = row["Firstname"];
				comm.Parameters["@Lastname"].Value = row["Lastname"];
				comm.Parameters["@Email"].Value = row["Email"];
				comm.Parameters["@Address1"].Value = row["Address1"];
				comm.Parameters["@Address2"].Value = row["Address2"];
				comm.Parameters["@Suburb"].Value = row["Suburb"];
				comm.Parameters["@Town"].Value = row["Town"];
				comm.Parameters["@Association"].Value = row["Association"];
				comm.Parameters["@Age"].Value = row["Age"];
				comm.Parameters["@DOB"].Value = row["DOB"];
				comm.Parameters["@Gender"].Value = row["Gender"];
				comm.Parameters["@ContactPhone"].Value = row["ContactPhone"];
				comm.Parameters["@RegistrationCode"].Value = row["RegistrationCode"];
				comm.Parameters["@LoggedIn"].Value = row["LoggedIn"];
				comm.Parameters["@KnowsCareer"].Value = row["KnowsCareer"];
				comm.Parameters["@LastCustomerID"].Value = row["LastCustomerID"];
				comm.Parameters["@Info"].Value = row["Info"];
				try {
					comm.ExecuteNonQuery();
					cgoodIds.Add((int)row["ID"]);
					recordsTransfered++;
				} catch (Exception ex) {
					Debug.Print("Participants sync error (ID: " + row["ID"].ToString() + "): " + ex.Message);
					cbadIds.Add((int)row["ID"]);
					WriteToLog(recordsTransfered, totalRecords, "Error updating Participant record ID = " +
							row["ID"].ToString() + " -- Query returned " + comm.Parameters["@returnvalue"].Value.ToString());
				}
			}
			if (recordsTransfered == totalRecords) {
				Debug.Print("All Participant records syncronised successfully");
				serverMessage = "All Participant records syncronised successfully";
			} else if (recordsTransfered == 0) {
				Debug.Print("No Participant records were syncronised");
				serverMessage = "No Participant records were syncronised";
			} else {
				Debug.Print("Some Participant records were syncronised successfully");
				serverMessage = "Some Participant records were syncronised successfully";
			}
			WriteToLog(recordsTransfered, totalRecords, serverMessage);
			badIds = new int[cbadIds.Count];
			goodIds = new int[cgoodIds.Count];
			cbadIds.CopyTo(badIds, 0);
			cgoodIds.CopyTo(goodIds, 0);
			return (cbadIds.Count == 0 ? 1 : 0);
		}

		/// <summary>
		/// I must not be called before SyncParticipants() !!
		/// </summary>
		public int SyncInteractions(DataSet interactions, out int[] badIds, out int[] goodIds) {
			Debug.Print("Syncing Interactions...");
			// I must not be called before SyncParticipants() !!
			int totalRecords = interactions.Tables[0].Rows.Count;
			int recordsTransfered = 0;
			string serverMessage;
			Collection<int> cbadIds = new Collection<int>(), cgoodIds = new Collection<int>();
			SqlCommand comm = CreateCommandRV();
			comm.CommandType = CommandType.StoredProcedure;
			comm.CommandText = "Sync_Interactions_Insert";
			comm.Parameters.AddWithValue("@ID", -1);
			comm.Parameters.AddWithValue("@ParticipantID", -1);
			comm.Parameters.AddWithValue("@EventID", -1);
			comm.Parameters.AddWithValue("@CustomerID", -1);
			comm.Parameters.AddWithValue("@DateTime", DateTime.Now);
			comm.Parameters.AddWithValue("@Subscribed", false);

			foreach (DataRow row in interactions.Tables[0].Rows) {
				comm.Parameters["@returnvalue"].Value = DBNull.Value;
				comm.Parameters["@ID"].Value = row["ID"];
				comm.Parameters["@ParticipantID"].Value = row["ParticipantID"];
				comm.Parameters["@EventID"].Value = row["EventID"];
				comm.Parameters["@CustomerID"].Value = row["CustomerID"];
				comm.Parameters["@DateTime"].Value = row["DateTime"];
				comm.Parameters["@Subscribed"].Value = row["Subscribed"];
				try {
					comm.ExecuteNonQuery();
					cgoodIds.Add((int)row["ID"]);
					recordsTransfered++;
				} catch (Exception ex) {
					// error
					Debug.Print("Interactions sync error (ID: " + row["ID"].ToString() + "): " + ex.Message);
					cbadIds.Add((int)row["ID"]);
					WriteToLog(recordsTransfered, totalRecords, "Error updating Interaction record ID = " +
							row["ID"].ToString() + " -- Query returned " + comm.Parameters["@returnvalue"].Value.ToString());
				}
			}
			if (recordsTransfered == totalRecords) {
				Debug.Print("All Interaction records syncronised successfully");
				serverMessage = "All Interaction records syncronised successfully";
			} else if (recordsTransfered == 0) {
				Debug.Print("No Interaction records were syncronised");
				serverMessage = "No Interaction records were syncronised";
			} else {
				Debug.Print("Some Interaction records were syncronised successfully");
				serverMessage = "Some Interaction records were syncronised successfully";
			}
			WriteToLog(recordsTransfered, totalRecords, serverMessage);
			badIds = new int[cbadIds.Count];
			goodIds = new int[cgoodIds.Count];
			cbadIds.CopyTo(badIds, 0);
			cgoodIds.CopyTo(goodIds, 0);
			return (cbadIds.Count == 0 ? 1 : 0);
		}

		public bool SyncSetSyncFlag(string table, bool flag, params int[] ids) {
			if (ids.Length == 0)
				return true;
			SqlCommand comm = _databaseConnection.CreateCommand();
			string updateSql, inClause = String.Empty;
			for (int id = 0; id < ids.Length; id++)
				inClause += ids[id].ToString() + (id < ids.Length - 1 ? ", " : "");
			updateSql = "UPDATE " + table + " SET Sync = "
					+ (flag ? "1" : "0") + ", LastSync = { fn NOW() } " +
					"WHERE ID IN (" + inClause + ")";
			try {
				comm.CommandType = CommandType.Text;
				comm.CommandText = updateSql;
				int rowsAffected = comm.ExecuteNonQuery();
				if (rowsAffected == ids.Length)
					return true;
				else
					return false;
			} catch {
				//throw new Exception(ex.Message + " -- " + updateSql);
				return false;
			} finally {
				comm.Dispose();
			}
		}

		public DataSet SimpleSelect(string storedProcedure) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			SqlDataAdapter da = null;
			//try {
			comm.CommandType = CommandType.StoredProcedure;
			comm.CommandText = storedProcedure;
			da = new SqlDataAdapter(comm);
			DataSet ds = new DataSet();
			da.Fill(ds);
			return ds;
			//} catch {
			//  return new DataSet();
			//} finally {
			//  comm.Dispose();
			//}
		}

		public int WriteToLog(int recordsTransfered, int totalRecords, string serverMessage) {
			SqlCommand comm = CreateCommandRV();
			try {
				comm.CommandType = CommandType.StoredProcedure;
				comm.CommandText = "Sync_WriteToLog";
				comm.Parameters.AddWithValue("@RecordsTransfered", recordsTransfered);
				comm.Parameters.AddWithValue("@TotalRecords", totalRecords);
				comm.Parameters.AddWithValue("@ServerMessage", serverMessage);
				comm.ExecuteNonQuery();
				return (int)comm.Parameters["@returnvalue"].Value;
			} catch {
				return 0;
			} finally {
				comm.Dispose();
			}
		}

		private SqlCommand CreateCommandRV() {
			SqlCommand comm = _databaseConnection.CreateCommand();
			SqlParameter returnValue = new SqlParameter("@returnvalue", System.Data.SqlDbType.Int);
			returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
			comm.Parameters.Add(returnValue);
			return comm;
		}

		private bool isRegistrationCodeBeingUsed(int code) {
			SqlCommand comm = _databaseConnection.CreateCommand();
			try {
				SqlParameter returnValue = new SqlParameter("@returnvalue", System.Data.SqlDbType.Bit);
				returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
				comm.CommandType = System.Data.CommandType.StoredProcedure;
				comm.CommandText = "RegistrationCode_Exists";
				comm.Parameters.AddWithValue("@Code", code);
				comm.Parameters.Add(returnValue);
				comm.ExecuteNonQuery();
				if ((int)comm.Parameters["@returnvalue"].Value == 1) return true;
				else return false;
			} catch {
				return true;
			} finally {
				comm.Dispose();
			}
		}

		private object nullableString(string str) {
			if (str == null || str.Trim().Length == 0)
				return DBNull.Value;
			else
				return str;
		}

		private object stringToNullableInt(string strInt) {
			if (strInt == null || strInt.Length == 0)
				return DBNull.Value;
			else {
				try {
					int intStr = Convert.ToInt32(strInt);
					return intStr;
				} catch { return DBNull.Value; }
			}
		}

		private string nullObjectToString(object obj) {
			if (obj == DBNull.Value)
				return string.Empty;
			else
				return Convert.ToString(obj);
		}

		private int nullObjectToInt(object obj) {
			if (obj == DBNull.Value)
				return -1;
			else
				return Convert.ToInt32(obj);
		}

		#region IDisposable Members

		void IDisposable.Dispose() {
			Kill();
		}

		#endregion

		public void Kill() {
			if (_databaseConnection != null) {
				_databaseConnection.Close();
				_databaseConnection.Dispose();
			}
		}
	}
}
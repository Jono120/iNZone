using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Inzone;
using Webstream;

public partial class Synchronize: System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		InzoneData data = new InzoneData("InZoneConnectionString");
		DataSet customers = data.SimpleSelect("Sync_Customers_NotSynced");
		DataSet events = data.SimpleSelect("Sync_Events_NotSynced");
		DataSet participants = data.SimpleSelect("Sync_Participants_NotSynced");
		DataSet interactions = data.SimpleSelect("Sync_Interactions_NotSynced");
		lblCustomerCount.Text = customers.Tables[0].Rows.Count.ToString();
		lblEventCount.Text = events.Tables[0].Rows.Count.ToString();
		lblParticipantCount.Text = participants.Tables[0].Rows.Count.ToString();
		lblInteractionCount.Text = interactions.Tables[0].Rows.Count.ToString();
		lblTotalCount.Text = (customers.Tables[0].Rows.Count + events.Tables[0].Rows.Count +
				participants.Tables[0].Rows.Count + interactions.Tables[0].Rows.Count).ToString();
	}
	protected void btnSyncNow_Click(object sender, EventArgs e) {

		string password = UrlEncoding.HashBase64UrlEncode(Utils.GetAppSetting("WebServicePassword"));

		Response.Buffer = false;
		Response.BufferOutput = false;

		InzoneData data = new InzoneData("InZoneConnectionString");
		InzoneSync.Syncronisation sync = new InzoneSync.Syncronisation();
		sync.Timeout = 1800000;
		Utils.writeToErrorLog("Beginning Sync...");
		Response.Write("Beginning synchronization with InZone web services at: " + sync.Url + "<br/>");
		Response.Flush();
		if (syncTable("Customers", data, sync, password) != 1) {
			Utils.writeToErrorLog("At least 1 Customer record did not sync. Sync cannot continue.<br/>");
			Response.Write("At least 1 Customer record did not sync. Sync cannot continue.<br/>");
		} else {
			Response.Write("All Customer records were sync'd....");
		}
		if (syncTable("Events", data, sync, password) != 1) {
			Utils.writeToErrorLog("At least 1 Event record did not sync. Sync cannot continue.<br/>");
			Response.Write("At least 1 Event record did not sync. Sync cannot continue.<br/>");
		} else {
			Response.Write("All Event records were sync'd....");
		}
		if (syncTable("Participants", data, sync, password) != 1) {
			Utils.writeToErrorLog("At least 1 Participant record did not sync. Sync cannot continue.<br/>");
			Response.Write("At least 1 Participant record did not sync. Sync cannot continue.<br/>");
		} else {
			Response.Write("All Participant records were sync'd....");
		}
		if (syncTable("Interactions", data, sync, password) != 1) {
			Utils.writeToErrorLog("At least 1 Interaction record did not sync. Sync cannot continue.<br/>");
			Response.Write("At least 1 Interaction record did not sync. Sync cannot continue.<br/>");
		} else {
			Response.Write("All Interaction records were sync'd....");
		}

		Response.Write("<br/>Sync complete<br/><br/>You may view the sync log for more information on any errors.<br/>" +
			"<a href='Synchronize.aspx'>Back to the Sync page</a>");
		data.Kill();
		Response.Flush();
		Response.End();
		//              Response.AddHeader("refresh", "2;URL=/Home.aspx");


	}
	private int syncTable(string table, InzoneData data, InzoneSync.Syncronisation sync, string password) {

		int[] goodIds, badIds;
		int totalUnsynced, result;
		DataSet records;

		Response.Write(table + "... ");
		Response.Flush();

		records = data.SimpleSelect("Sync_" + table + "_NotSynced");
		totalUnsynced = records.Tables[0].Rows.Count;

		Response.Write("Total unsync'd: " + totalUnsynced.ToString() + "... ");
		Response.Flush();

		switch (table) {
			case "Customers":
				result = sync.SyncCustomers(records, password, out badIds, out goodIds);
				break;
			case "Events":
				result = sync.SyncEvents(records, password, out badIds, out goodIds);
				break;
			case "Participants":
				result = sync.SyncParticipants(records, password, out badIds, out goodIds);
				break;
			case "Interactions":
				result = sync.SyncInteractions(records, password, out badIds, out goodIds);
				break;
			default:
				result = 0;
				badIds = new int[1] { -1 };
				goodIds = new int[1] { -1 };
				break;
		}

		Response.Write("Sync flag set: " + data.SyncSetSyncFlag(table, true, goodIds).ToString() + "... ");
		Response.Write("Sync flag set: " + data.SyncSetSyncFlag(table, false, badIds).ToString() + "... ");
		Response.Write("Result code: " + result.ToString() + " (" + (result == 1 ? "GOOD" : "BAD") + ")... ");
		Response.Write("Good records: " + goodIds.Length.ToString() +
			"... Bad records: " + badIds.Length.ToString() + "<br/>");
		Response.Flush();

		return result;
	}
}

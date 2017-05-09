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

public partial class RegisterEvent: System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {

	}
	protected void btnRegisterEvent_Click(object sender, EventArgs e) {
		InzoneData data = new InzoneData("InZoneConnectionString");
		// get user input
		string eventName = txtEventName.Text.Trim();
		DateTime eventDate;
		try {
			eventDate = DateTime.Parse(txtEventDate.Text);
		} catch {
			lblStatus.Text = "Could not recognise date entered. Please try again.";
			return;
		}

		int eventId;
		if ((eventId = data.EventInsert(eventName, eventDate, false)) == -1) {
			lblStatus.Text = "Error inserting new Event. You should call Paul!";
		} else {
			lblStatus.Text = "Event added OK (ID = " + eventId.ToString() + ")";
		}

		data.Kill();
	}
}

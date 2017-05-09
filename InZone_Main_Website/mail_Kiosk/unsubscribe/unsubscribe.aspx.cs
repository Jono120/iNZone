using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class mail_unsubscribe_unsubscribe: System.Web.UI.Page {

	private int mailoutId;

	protected void Page_Load(object sender, EventArgs e) {

		// make sure QueryString is OK
		if (Request.QueryString.Count == 0) {
			showError("Invalid request (1)");
			return;
		}
		if (Request.QueryString[0] == null) {
			showError("Invalid request (2)");
			return;
		}
		// attempt to parse the QueryString to get the mailout id
		try {
			string temp = Webstream.UrlEncoding.Base64UrlDecode(
				Request.QueryString[0]);
			mailoutId = Convert.ToInt32(temp);
		} catch {
			showError("Invalid request (3)");
			return;
		}

		// work to do during postback
		if (!IsPostBack) {
			// create database connection & grab mailout record
            Inzone.MailService.Kiosk.InZoneKioskDAL db;
            Inzone.MailService.Kiosk.Mailout m;
            try {
                db = new Inzone.MailService.Kiosk.InZoneKioskDAL();
				m = db.Mailouts.First(mo => mo.ID == mailoutId);
			} catch {
				showError("Unable to process request (database error (1))");
				return;
			}
			// get partner name
			string partnerName = m.Partner.DisplayName + " (" + m.Partner.PartnerName + ")";
			// check to see if they have
			// already unsubscribed
			Nullable<bool> unsub = m.Interaction.Subscribed;
			if (unsub != null && unsub == false) {
				showError("You have already unsubscribed to "
					+ partnerName);
				return;
			}
			// prepare prompt
			lblCustomerName.Text = partnerName;
			// close db and show prompt
			db.Dispose();
			divPrompt.Visible = true;
		}
	}

	private void showError(string message) {
		divPrompt.Visible = false;
		divOK.Visible = false;
		lblError.Text = message;
		divError.Visible = true;
	}
	protected void btnNo_Click(object sender, EventArgs e) {
		showError("You chose not to unsubscribe after all!");
	}
	protected void btnYes_Click(object sender, EventArgs e) {
		// process unsubscribe...
		// create db connection and get mailout record
        Inzone.MailService.Kiosk.InZoneKioskDAL db;
        Inzone.MailService.Kiosk.Mailout m;
		try {
            db = new Inzone.MailService.Kiosk.InZoneKioskDAL();
			m = db.Mailouts.First(mo => mo.ID == mailoutId);
		} catch {
			showError("Unable to process request (database error (2))");
			return;
		}
		// set the interaction record to unsubscribed and
		// save the changes
		m.Interaction.Subscribed = false;
		db.SubmitChanges(ConflictMode.FailOnFirstConflict);
		// don't need this anymore
		db.Dispose();
		// show the success
		divPrompt.Visible = false;
		divOK.Visible = true;
	}
}

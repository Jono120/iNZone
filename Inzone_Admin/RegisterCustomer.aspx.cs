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

public partial class RegisterCustomer: System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {

	}
	protected void btnRegisterCustomer_Click(object sender, EventArgs e) {
		InzoneData data = new InzoneData("InZoneConnectionString");
		int customerId = data.CustomerInsert(txtCustomerName.Text);
		lblStatus.Text = "Customer added OK (ID = " + customerId.ToString() + ")";
		txtCustomerName.Text = String.Empty;
        data.Kill();
	}
}

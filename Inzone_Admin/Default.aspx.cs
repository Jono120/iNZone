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
using System.Xml;

using Inzone;

public partial class _Default: System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
	}
	protected void btnTest_Click(object sender, EventArgs e) {
		InzoneData data = new InzoneData("InZoneConnectionString");
		data.Test();
		lblTestResult.Text = "Test complete";
        data.Kill();
	}
}

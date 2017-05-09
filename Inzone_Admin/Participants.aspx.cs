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

public partial class Participants : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
			if (!IsPostBack) {
				InzoneData data = new InzoneData("InZoneConnectionString");
				lblCurrentEventName.Text = data.GetActiveEventName();
                data.Kill();
			}
    }
}

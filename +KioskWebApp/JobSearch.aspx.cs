using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace KioskApplication
{
    public partial class JobSearch : System.Web.UI.Page
    {
        // Add job search sites to trusted sites
        protected void Page_Load(object sender, EventArgs e)
        {
            string index = Request.QueryString["Provider"];
            if (String.IsNullOrWhiteSpace(index)) index = "1";
            int provider = Int32.Parse(index);
            JobSearchServiceProviders providers = Helper.GetJobSearchServiceProviders(Server.MapPath("~/" + ConfigurationManager.AppSettings["JobSearchProvidersFolderName"] + "/JobSearchServiceProviders.xml"));
            IFramePlaceHolder.Controls.Clear();
            IFramePlaceHolder.Controls.Add(new LiteralControl("<iframe width='1250px' height='725px' src='" + providers.Provider[provider-1].Url + "'></iframe>"));
        }
    }
}
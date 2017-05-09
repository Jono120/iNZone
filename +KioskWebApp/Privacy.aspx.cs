using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KioskApplication
{
    /// <summary>
    /// The Privacy Statement Page
    /// </summary>
    public partial class Privacy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Hide the OK button if not accessed from MyDetails page
            if(Request.UrlReferrer.AbsolutePath.Contains("MyDetails.aspx"))
            {
                AcceptButton.Visible = false;
                DeclineButton.Visible = false;
            }
            else
            {
                AcceptButton.Visible = true;
                DeclineButton.Visible = true;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            AcceptButton.Click += new ImageClickEventHandler(AcceptButton_Click);
            DeclineButton.Click += new ImageClickEventHandler(DeclineButton_Click);
            AcceptButton.Attributes.Add("onmouseover", "src=\"Images/accept_btn_over.png\"");
            AcceptButton.Attributes.Add("onmouseout", "src=\"Images/accept_btn.png\"");
            DeclineButton.Attributes.Add("onmouseover", "src=\"Images/decline_btn_over.png\"");
            DeclineButton.Attributes.Add("onmouseout", "src=\"Images/decline_btn.png\"");
            base.OnInit(e);
        }

        void DeclineButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        void AcceptButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Registration.aspx");
        }
    }
}

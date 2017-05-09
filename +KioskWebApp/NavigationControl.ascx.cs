using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KioskApplication
{
    public partial class NavigationControl : System.Web.UI.UserControl
    {
        #region Properties
        public string PartnerID { get; set; }

        public ImageButton HomeButton
        {
            get { return HomeImageButton; }
        }

        public ImageButton LogoutButton
        {
            get { return LogoutImageButton; }
        }

        public void SetParticipantName(string firstName, string lastName)
        {
            ParticipantLabel.Text = firstName + " " + lastName;
            Session["FirstName"] = firstName;
            Session["LastName"] = lastName;
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ParticipantLabel.Text = Session["FirstName"] + " " + Session["LastName"];

            //Need to check if the participant has answered the question "Would you like more info"
            //if the navigation control is accessed from the videos page
            if (Request.Url.AbsolutePath.Contains("Videos.aspx"))
            {
                HomeImageButton.OnClientClick = "Navigation_OnClick('" + PartnerID + "','Landing.aspx'); return false;";
                MyDetailsImageButton.OnClientClick = "Navigation_OnClick('" + PartnerID + "','MyDetails.aspx'); return false;";
                Session["PreviousPage"] = Request.Url.AbsoluteUri;
                LogoutImageButton.OnClientClick = "Navigation_OnClick('" + PartnerID + "','Login.aspx'); return false;";
            }
            else if (!Request.Url.AbsolutePath.Contains("MyDetails.aspx"))
            {
                HomeImageButton.Click += new ImageClickEventHandler(HomeImageButton_Click);
                MyDetailsImageButton.Click += new ImageClickEventHandler(MyDetailsImageButton_Click);
                LogoutImageButton.Click += new ImageClickEventHandler(LogoutImageButton_Click);
            }

            string url = Request.Url.AbsolutePath;

            if (url.Contains("Landing.aspx"))
                HomeImageButton.Enabled = false;
            else if (url.Contains("MyDetails.aspx"))
                MyDetailsImageButton.Enabled = false;
        }

        protected override void OnInit(EventArgs e)
        {
            HomeImageButton.Attributes.Add("onmouseover", "src=\"Images/home_btn_over.png\"");
            HomeImageButton.Attributes.Add("onmouseout", "src=\"Images/home_btn.png\"");

            MyDetailsImageButton.Attributes.Add("onmouseover", "src=\"Images/my_details_btn_over.png\"");
            MyDetailsImageButton.Attributes.Add("onmouseout", "src=\"Images/my_details_btn.png\"");

            LogoutImageButton.Attributes.Add("onmouseover", "src=\"Images/log_out_btn_over.png\"");
            LogoutImageButton.Attributes.Add("onmouseout", "src=\"Images/log_out_btn.png\"");

            base.OnInit(e);
        }

        void LogoutImageButton_Click(object sender, ImageClickEventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }

        void MyDetailsImageButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/MyDetails.aspx");
        }

        void HomeImageButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Landing.aspx");
        }
    }
}
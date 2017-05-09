using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using Olympic.AutoDataLayer;
using System.Configuration;

namespace KioskApplication
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();

            UsernameTextBox.Focus();
        }

        protected override void OnInit(EventArgs e)
        {
            LoginButton.Click += new ImageClickEventHandler(LoginButton_Click);
            RegisterButton.Click += new ImageClickEventHandler(RegisterButton_Click);
            ForgotPasswordButton.Click += new ImageClickEventHandler(ForgotPasswordButton_Click);
            LoginButton.Attributes.Add("onmouseover", "src=\"Images/login_btn_over.png\"");
            LoginButton.Attributes.Add("onmouseout", "src=\"Images/login_btn.png\"");
            RegisterButton.Attributes.Add("onmouseover", "src=\"Images/register_now_btn_over.png\"");
            RegisterButton.Attributes.Add("onmouseout", "src=\"Images/register_now_btn.png\"");
            ForgotPasswordButton.Attributes.Add("onmouseover", "src=\"Images/forgot_password_btn_over.png\"");
            ForgotPasswordButton.Attributes.Add("onmouseout", "src=\"Images/forgot_password_btn.png\"");

            base.OnInit(e);
        }

        void ForgotPasswordButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/ForgotPassword.aspx?Username=" + UsernameTextBox.Text.Trim());
        }

        void RegisterButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Privacy.aspx?Previous=Register");
        }

        void LoginButton_Click(object sender, ImageClickEventArgs e)
        {
            if (String.IsNullOrEmpty(MobilePhoneTextBox.Text))
            {
                if (String.IsNullOrEmpty(UsernameTextBox.Text) || String.IsNullOrEmpty(PasswordTextBox.Text))
                {
                    ShowErrorMessage("Please enter Mobile Phone or a combination of Username and Password to login.");
                    MobilePhoneTextBox.Focus();
                    return;
                }
                else
                {
                    InitiateLoginProcess(false);
                }
            }
            else
            {
                InitiateLoginProcess(true);
            }

        }
        private void ShowErrorMessage(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visible = true;
        }

        private void LoginParticipant(Participant participant)
        {
            Session.Clear();
            Session.Add("ParticipantID", participant.ID);
            Session.Add("FirstName", participant.FirstName);
            Session.Add("LastName", participant.LastName);

            //Randomly select a tab
            Random random = new Random();

            int numOfTabs =
                Helper.GetKioskVideos(Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/CategoriesConfig.xml")).Category.Count;

            Session.Add("SelectedTab", random.Next(0, numOfTabs));
        }

        private void InitiateLoginProcess(bool byMobile)
        {
            string username;
            string password;

            if (byMobile)
            {
                username = MobilePhoneTextBox.Text.Trim();
                password = MobilePhoneTextBox.Text.Trim();
            }
            else
            {
                username = UsernameTextBox.Text.Trim();
                password = PasswordTextBox.Text.Trim();
            }

            //Login the user, redirect to landing page if login ok
            SearchFilter userNameSearchFilter = new SearchFilter("UserName", username);
            SearchFilterCollection searchFilterCollection = new SearchFilterCollection();
            searchFilterCollection.Add(userNameSearchFilter);

            SearchFilter passwordSearchFilter = new SearchFilter("Password", password);
            searchFilterCollection.Add(passwordSearchFilter);



            //Search for participant with specified username and password or username/security question/security answer
            List<Participant> participant = Participant.List(searchFilterCollection);

            if (participant.Count != 1)
            {
                if (byMobile)
                {
                    SearchFilter mobileSearchFilter = new SearchFilter("PhoneNumber", MobilePhoneTextBox.Text.Trim());
                    searchFilterCollection = new SearchFilterCollection();
                    searchFilterCollection.Add(mobileSearchFilter);
                    participant = Participant.List(searchFilterCollection);
                }
            }

            if (participant.Count != 1)
            {
                //Participant not found
                if (byMobile)
                    ShowErrorMessage("Mobile Number is incorect. Please try again.");
                else 
                    ShowErrorMessage("Username and password combination is incorrect. Please try again.");

            }
            else
            {
                //Participant found

                LoginParticipant(participant[0]);

                //Redirect to landing page if user logged in using username/password
                //Otherwise redirect to MyDetails page if user logged in using username/security question/security answer
                if (PasswordTextBox.Visible)
                {
                    Response.Redirect("~/Landing.aspx");
                }
                else
                {
                    Response.Redirect("~/MyDetails.aspx");
                }
            }
        }
    }
}

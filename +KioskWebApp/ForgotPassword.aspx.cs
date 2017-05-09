using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using Olympic.AutoDataLayer;

namespace KioskApplication
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();

            if(!Page.IsPostBack)
            {
                UsernameTextBox.Focus();

                if (Request.QueryString["Username"] != "")
                    UsernameTextBox.Text = Request.QueryString["Username"];

                LoadSecurityQuestion();
            }
            
        }

        protected override void OnInit(EventArgs e)
        {
            LoginButton.Click += new ImageClickEventHandler(LoginButton_Click);
            RegisterButton.Click += new ImageClickEventHandler(RegisterButton_Click);
            CancelButton.Click += new ImageClickEventHandler(CancelButton_Click);
            LoginButton.Attributes.Add("onmouseover", "src=\"Images/login_btn_over.png\"");
            LoginButton.Attributes.Add("onmouseout", "src=\"Images/login_btn.png\"");
            RegisterButton.Attributes.Add("onmouseover", "src=\"Images/register_now_btn_over.png\"");
            RegisterButton.Attributes.Add("onmouseout", "src=\"Images/register_now_btn.png\"");
            CancelButton.Attributes.Add("onmouseover", "src=\"Images/cancel_btn_over.png\"");
            CancelButton.Attributes.Add("onmouseout", "src=\"Images/cancel_btn.png\"");
            UsernameTextBox.Attributes.Add("onblur", "LoadSecurityQuestion();return false;");

            base.OnInit(e);
        }

        void CancelButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        private void LoadSecurityQuestion()
        {
            if (UsernameTextBox.Text != "")
            {
                SearchFilter userNameSearchFilter = new SearchFilter("UserName", UsernameTextBox.Text.Trim());
                SearchFilterCollection searchFilterCollection = new SearchFilterCollection();
                searchFilterCollection.Add(userNameSearchFilter);

                List<Participant> participant = Participant.List(searchFilterCollection);

                if (participant.Count == 1)
                {
                    //Username is found, send back the security question
                    SecurityQuestionDisplayLabel.Text = participant[0].SecurityQuestion;
                    SecurityAnswerTextBox.Focus();
                }
                else
                {
                    ShowErrorMessage("Please enter valid username.");
                }
            }
        }

        void RegisterButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Privacy.aspx?Previous=Register");
        }

        void LoginButton_Click(object sender, ImageClickEventArgs e)
        {
            //Login the user, redirect to landing page if login ok
            SearchFilter userNameSearchFilter = new SearchFilter("UserName", UsernameTextBox.Text.Trim());
            SearchFilterCollection searchFilterCollection = new SearchFilterCollection();
            searchFilterCollection.Add(userNameSearchFilter);



            //SearchFilter securityQuestionSearchFilter = new SearchFilter("SecurityQuestion", SecurityQuestionDisplayLabel.Text.Trim());
            SearchFilter securityAnswerSearchFilter = new SearchFilter("SecurityAnswer", SecurityAnswerTextBox.Text.Trim());
            //searchFilterCollection.Add(securityQuestionSearchFilter);
            searchFilterCollection.Add(securityAnswerSearchFilter);


            //Search for participant with specified username and password or username/security question/security answer
            List<Participant> participant = Participant.List(searchFilterCollection);

            if (participant.Count != 1)
            {
                //Participant not found


                ShowErrorMessage("Username, security question and security <br>answer combination is incorrect. Please try again.");
            }
            else
            {
                //Participant found

                LoginParticipant(participant[0]);

                //Redirect to MyDetails page if user logged in using username/security question/security answer

                Response.Redirect("~/MyDetails.aspx");
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
    }
}

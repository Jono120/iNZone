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
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                FirstNameTextBox.Focus();
            }
            

            //PasswordTextBox.Attributes.Add("value", PasswordTextBox.Text);

        }

        protected override void OnInit(System.EventArgs e)
        {
            SubmitButton.Click += new ImageClickEventHandler(SubmitButton_Click);
            CancelButton.Click += new ImageClickEventHandler(CancelButton_Click);
            SubmitButton.Attributes.Add("onmouseover", "src=\"Images/submit_btn_over.png\"");
            SubmitButton.Attributes.Add("onmouseout", "src=\"Images/submit_btn.png\"");
            CancelButton.Attributes.Add("onmouseover", "src=\"Images/cancel_btn_over.png\"");
            CancelButton.Attributes.Add("onmouseout", "src=\"Images/cancel_btn.png\"");

            base.OnInit(e);
        }



        void CancelButton_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }



        void SubmitButton_Click(object sender, ImageClickEventArgs e)
        {
            FirstNameRequiredFieldValidator.Validate();
            LastNameRequiredFieldValidator.Validate();
            MobilePhoneRequiredFieldValidator.Validate();
            EmailRegularExpressionValidator.Validate();


            if (!FirstNameRequiredFieldValidator.IsValid)
            {
                FirstNameTextBox.Focus();
                ShowErrorMessage("Please complete all required fields.", true);
            }
            else if (!LastNameRequiredFieldValidator.IsValid)
            {
                LastNameTextBox.Focus();
                ShowErrorMessage("Please complete all required fields.", true);
            }
            else if (!MobilePhoneRequiredFieldValidator.IsValid)
            {
                MobilePhoneTextBox.Focus();
                ShowErrorMessage("Please complete all required fields.", true);
            }
            else if (!EmailRegularExpressionValidator.IsValid)
            {
                EmailTextBox.Focus();
                ShowErrorMessage("Please enter a valid email.");
            }

            else
            {
                SearchFilterCollection searchFilterCollection = new SearchFilterCollection();

                SearchFilter searchFilter = new SearchFilter("PhoneNumber", MobilePhoneTextBox.Text.Trim());
                searchFilterCollection.Add(searchFilter);

                if (Participant.GetCount(searchFilterCollection) == 0)
                {

                    Participant participant = new Participant();
                    participant.DateCreated = System.DateTime.Now;
                   
                    
                    if (!String.IsNullOrWhiteSpace(EmailTextBox.Text))
                        participant.Email = EmailTextBox.Text.Trim();

                    participant.FirstName = FirstNameTextBox.Text.Trim();
                    participant.LastName = LastNameTextBox.Text.Trim();
                    participant.Password = MobilePhoneTextBox.Text.Trim();
                    participant.UserName = MobilePhoneTextBox.Text.Trim();
                    participant.KioskID = System.Environment.MachineName;


                    participant.Save();

                    LoginParticipant(participant);

                    Response.Redirect("~/Landing.aspx");
                }
                else
                {
                    ShowErrorMessage("Mobile Phone already exists.");
                }
            }

        }

        private Participant.MaleFemale GetGender(string MaleFemale)
        {
            return (Participant.MaleFemale)Enum.Parse(typeof(Participant.MaleFemale), MaleFemale);
        }

        private void ShowErrorMessage(string message)
        {
            ShowErrorMessage(message, false);
        }

        private void ShowErrorMessage(string message, bool showStar)
        {
            ErrorMessage.Visible = true;
            ErrorMessage.Text = message;

            StarErrorImage.Visible = showStar;
        }
        private void LoginParticipant(Participant participant)
        {
            Session.Clear();
            Session.Add("ParticipantID", participant.ID);
            Session.Add("FirstName", participant.FirstName);
            Session.Add("LastName", participant.LastName);

            Random random = new Random();

            KioskVideos kioskVideos = Helper.GetKioskVideos(Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/CategoriesConfig.xml"));
            if (kioskVideos != null)
                Session.Add("SelectedTab", random.Next(0, kioskVideos.Category.Count));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using System.Data.SqlClient;
using Olympic.AutoDataLayer;

namespace KioskApplication
{
    public partial class MyDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check user is logged in
            if (Session["ParticipantID"] != null)
            {
                if (!Page.IsPostBack)
                {
                    if (Request.UrlReferrer != null && !Request.UrlReferrer.AbsolutePath.Contains("/Privacy.aspx"))
                        Session["PreviousPage"] = Request.UrlReferrer.PathAndQuery;

                    Session["PrivacyClicked"] = false;

                    FirstNameTextBox.Focus();

                    PopulateFields();
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            SubmitButton.Click += new ImageClickEventHandler(SubmitButton_Click);
            CancelButton.Click += new ImageClickEventHandler(CancelButton_Click);
            PrivacyStatementButton.Click += new ImageClickEventHandler(PrivacyStatementButton_Click);
            FloatingPromptControl.FloatingPromptNoButton.Click += new ImageClickEventHandler(FloatingPromptNoButton_Click);
            FloatingPromptControl.FloatingPromptYesButton.Click += new ImageClickEventHandler(FloatingPromptYesButton_Click);


            SubmitButton.Attributes.Add("onmouseover", "src=\"Images/submit_btn_over.png\"");
            SubmitButton.Attributes.Add("onmouseout", "src=\"Images/submit_btn.png\"");
            CancelButton.Attributes.Add("onmouseover", "src=\"Images/back_btn_over.png\"");
            CancelButton.Attributes.Add("onmouseout", "src=\"Images/back_btn.png\"");
            PrivacyStatementButton.Attributes.Add("onmouseover", "src=\"Images/privacy_statement_btn_over.png\"");
            PrivacyStatementButton.Attributes.Add("onmouseout", "src=\"Images/privacy_statement_btn.png\"");
            //CancelButton.Attributes.Add("onclick", "javascript:history.back();");

            NavigationControl.LogoutButton.Click += new ImageClickEventHandler(LogoutButton_Click);

            NavigationControl.HomeButton.Click += new ImageClickEventHandler(HomeButton_Click);

            base.OnInit(e);
        }

        void HomeButton_Click(object sender, ImageClickEventArgs e)
        {
            if (IsMyDetailChanged())
            {
                FloatingPromptControl.Message = "You have modified your details. Are you sure you don't want to save your changes before navigating to another page?";
                FloatingPromptControl.Show();
            }
            else
            {
                Response.Redirect("~/Landing.aspx");
            }
        }

        void LogoutButton_Click(object sender, ImageClickEventArgs e)
        {
            if (IsMyDetailChanged())
            {
                FloatingPromptControl.Message = "You have modified your details. Are you sure you don't want to save your changes before navigating to another page?";
                FloatingPromptControl.Show();
            }
            else
            {
                Session.Clear();
                Response.Redirect("~/Login.aspx");
            }
        }

        void FloatingPromptYesButton_Click(object sender, ImageClickEventArgs e)
        {
            if (Convert.ToBoolean(Session["PrivacyClicked"]))
            {
                Response.Redirect("~/Privacy.aspx");
            }
            else
            {
                Response.Redirect(Session["PreviousPage"].ToString());
            }
        }

        void FloatingPromptNoButton_Click(object sender, ImageClickEventArgs e)
        {
            FirstNameTextBox.Focus();

            //Response.Redirect("MyDetails.aspx");
        }

        void PrivacyStatementButton_Click(object sender, ImageClickEventArgs e)
        {
            if (IsMyDetailChanged())
            {

                FloatingPromptControl.Message = "You have modified your details. Are you sure you don't want to save your changes before navigating to another page?";
                Session["PrivacyClicked"] = true;
                FloatingPromptControl.Show();
            }
            else
            {
                Response.Redirect("~/Privacy.aspx");
            }
        }

        void CancelButton_Click(object sender, ImageClickEventArgs e)
        {
            if (IsMyDetailChanged())
            {

                FloatingPromptControl.Message = "You have modified your details. Are you sure you want to cancel your changes?";
                FloatingPromptControl.Show();
            }
            else
            {
                //Go back to previous page if no changes are made
                if (Session["PreviousPage"].ToString().Contains("Login.aspx") || Session["PreviousPage"].ToString().Contains("ForgotPassword.aspx"))
                {

                    Response.Redirect("~/Landing.aspx");
                }
                else
                {
                    Response.Redirect(Session["PreviousPage"].ToString());
                }
            }
        }

        void SubmitButton_Click(object sender, ImageClickEventArgs e)
        {
            DisableButtons();

            //BadWordFilterClass badwordfilter = BadWordFilterClass.Get();
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DefaultConnectionString"]);
            SqlCommand command = new SqlCommand("usp_getBadWordFilter_Find", conn);


            FirstNameRequiredFieldValidator.Validate();
            LastNameRequiredFieldValidator.Validate();
            ContactPhoneRequiredValidator.Validate();

            // Phone Number Validation - Glenn
            ContactPhoneRegularExpressionValidator.Validate();

            // Gender Validation - Glenn
            GenderRequiredFieldValidator.Validate();

            // Career Question - Glenn
            KnowCareerRequiredFieldValidator.Validate();


            EmailRegularExpressionValidator.Validate();
            EmailCompareValidator.Validate();
            EmailCompareValidator2.Validate();

            DayRegularExpressionValidator.Validate();
            MonthRegularExpressionValidator.Validate();
            YearRegularExpressionValidator.Validate();

             SearchFilterCollection searchFilterCollection = new SearchFilterCollection();

            
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
            if (!ContactPhoneRequiredValidator.IsValid)
            {
                ContactPhoneTextBox.Focus();
                ShowErrorMessage("Please complete all required fields.", true);
            }
            else if (!EmailRegularExpressionValidator.IsValid)
            {
                EmailTextBox.Focus();
                ShowErrorMessage("Please enter a valid email.");
            }
            else if (!EmailCompareValidator.IsValid || !EmailCompareValidator2.IsValid)
            {
                if (!EmailCompareValidator.IsValid)
                {
                    EmailTextBox.Focus();
                }
                else
                {
                    ConfirmEmailTextBox.Focus();
                }

                ShowErrorMessage("Emails must match.");
            }
            else if (!DayRegularExpressionValidator.IsValid)
            {
                DayTextBox.Focus();
                ShowErrorMessage("Date of birth must be entered in the format: DD/MM/YY, e.g. 15/02/95");
            }
            else if (!MonthRegularExpressionValidator.IsValid)
            {
                MonthTextBox.Focus();
                ShowErrorMessage("Date of birth must be entered in the format: DD/MM/YY, e.g. 15/02/95");
            }
            else if (!YearRegularExpressionValidator.IsValid)
            {
                YearTextBox.Focus();
                ShowErrorMessage("Date of birth must be entered in the format: DD/MM/YY, e.g. 15/02/95");
            }
            else if (Convert.ToDateTime(DayTextBox.Text.Trim() + "/" + MonthTextBox.Text.Trim() + "/" + YearTextBox.Text.Trim()) >= DateTime.Now.AddYears(-5))
            {
                YearTextBox.Focus();
                ShowErrorMessage("Please enter a valid Date of Birth.");
            }
            else if (!ContactPhoneRegularExpressionValidator.IsValid) // Glenn 28/05/2010
            {
                ContactPhoneTextBox.Focus();
                ShowErrorMessage("Contact Phone is invalid or contains letters or symbols.");
            }
            else if (BadWordFilter.FindBadWord(FirstNameTextBox.Text.Trim(), true, 2)) // Glenn 25/03/2010
            {
                FirstNameTextBox.Text = "";
                FirstNameTextBox.Focus();
                ShowErrorMessage("The First Name field contained unsuitable content");
            }
            else if (BadWordFilter.FindBadWord(LastNameTextBox.Text.Trim(), true, 2)) // Glenn 25/03/2010
            {
                LastNameTextBox.Text = "";
                LastNameTextBox.Focus();
                ShowErrorMessage("The Last Name field contained unsuitable content");
            }
            else if (BadWordFilter.FindBadWord(Address1TextBox.Text.Trim(), false, 8)) // Glenn 25/03/2010
            {
                Address1TextBox.Text = "";
                Address1TextBox.Focus();
                ShowErrorMessage("The Address field contained unsuitable content");
            }
            else if (BadWordFilter.FindBadWord(SuburbTextBox.Text.Trim(), false, 2)) // Glenn 25/03/2010
            {
                SuburbTextBox.Text = "";
                SuburbTextBox.Focus();
                ShowErrorMessage("The Suburb field contained unsuitable content");
            }
            else if (BadWordFilter.FindBadWord(TownCityTextBox.Text.Trim(), false, 3)) // Glenn 25/03/2010
            {
                TownCityTextBox.Text = "";
                TownCityTextBox.Focus();
                ShowErrorMessage("The Town/City field contained unsuitable content");
            }
            else
            {
                //Save details to database
                Participant participant = Participant.Get("_id", Session["ParticipantID"]);

                if (participant.PhoneNumber != ContactPhoneTextBox.Text.Trim())
                {
                    SearchFilter searchFilter = new SearchFilter("PhoneNumber", ContactPhoneTextBox.Text.Trim());
                    searchFilterCollection.Add(searchFilter);

                    if (Participant.GetCount(searchFilterCollection) > 0)
                    {
                        ShowErrorMessage("Phone Number already exists");
                        return;
                    }
                }

                participant.Address1 = ToTitleCase(Address1TextBox.Text.Trim());
                //participant.Address2 = Address2TextBox.Text.Trim();
                participant.DateOfBirth = Convert.ToDateTime(DayTextBox.Text.Trim() + "/" + MonthTextBox.Text.Trim() + "/" + YearTextBox.Text.Trim());
                participant.Email = EmailTextBox.Text.Trim();
                participant.FirstName = ToTitleCase(FirstNameTextBox.Text.Trim());
                participant.LastName = ToTitleCase(LastNameTextBox.Text.Trim());
                participant.Gender = GetGender(GenderRadioButtonList.SelectedValue);
                participant.KnowsCareer = Convert.ToBoolean(KnowCareerRadioButtonList.SelectedValue);
                
                participant.Suburb = ToTitleCase(SuburbTextBox.Text.Trim());
                participant.Town = ToTitleCase(TownCityTextBox.Text.Trim());
                if (participant.UserName == participant.PhoneNumber)
                {
                    participant.UserName = ContactPhoneTextBox.Text.Trim();
                    participant.Password = ContactPhoneTextBox.Text.Trim();
                }

                participant.PhoneNumber = ContactPhoneTextBox.Text.Trim();

                participant.Save();

                //Display saved message
                ShowSuccessMessage();
                PopulateFields();
                EnableButtons();
            }
        }

        private bool IsMyDetailChanged()
        {
            Participant participant = Participant.Get("_id", Session["ParticipantID"]);

            if (participant.FirstName != FirstNameTextBox.Text.Trim())
                return true;
            if (participant.LastName != LastNameTextBox.Text.Trim())
                return true;
            if ((participant.Email ?? "") != EmailTextBox.Text.Trim())
                return true;
            if ((participant.PhoneNumber ?? "" )  != ContactPhoneTextBox.Text.Trim())
                return true;
            if (participant.KnowsCareer != Convert.ToBoolean(KnowCareerRadioButtonList.SelectedValue))
                return true;
            if ((participant.Suburb ?? "" ) != SuburbTextBox.Text.Trim())
                return true;
            if ((participant.Address1 ?? "") != Address1TextBox.Text.Trim())
                return true;
            if (participant.Town != TownCityTextBox.Text.Trim())
                return true;
            if (participant.Gender != GetGender(GenderRadioButtonList.SelectedValue))
                return true;
            if (participant.DateOfBirth.HasValue)
            {
                if (participant.DateOfBirth.Value.ToString("dd") != DayTextBox.Text.Trim())
                    return true;
                if (participant.DateOfBirth.Value.ToString("MM") != MonthTextBox.Text.Trim())
                    return true;
                if (participant.DateOfBirth.Value.ToString("yy") != YearTextBox.Text.Trim())
                    return true;
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(DayTextBox.Text.Trim() + MonthTextBox.Text.Trim() + YearTextBox.Text.Trim()))
                    return true;
            }

            return false;
        }

        private void PopulateFields()
        {
            Participant participant = Participant.Get("_id", Session["ParticipantID"]);

            FirstNameTextBox.Text = participant.FirstName;
            LastNameTextBox.Text = participant.LastName;

            EmailTextBox.Text = participant.Email;
            ConfirmEmailTextBox.Text = participant.Email;

            ContactPhoneTextBox.Text = participant.PhoneNumber;
            SetKnowsCareer(participant.KnowsCareer);

            SuburbTextBox.Text = participant.Suburb;
            Address1TextBox.Text = participant.Address1;
            TownCityTextBox.Text = participant.Town;
            SetGender(participant.Gender);
            if (participant.DateOfBirth.HasValue)
            {
                DayTextBox.Text = participant.DateOfBirth.Value.ToString("dd");
                MonthTextBox.Text = participant.DateOfBirth.Value.ToString("MM");
                YearTextBox.Text = participant.DateOfBirth.Value.ToString("yy");
            }

            NavigationControl.SetParticipantName(participant.FirstName, participant.LastName);
        }

        private Participant.MaleFemale GetGender(string MaleFemale)
        {
            return (Participant.MaleFemale)Enum.Parse(typeof(Participant.MaleFemale), MaleFemale);
        }

        private void ShowSuccessMessage()
        {
            DetailsUpdatedImage.Visible = true;
            ErrorMessage.Visible = false;
            StarErrorImage.Visible = false;
        }



        private void SetGender(Participant.MaleFemale gender)
        {
            if (gender == Participant.MaleFemale.M)
            {
                GenderRadioButtonList.SelectedIndex = 0;
            }
            else
            {
                GenderRadioButtonList.SelectedIndex = 1;
            }
        }

        private void SetKnowsCareer(bool knowsCareer)
        {
            if (knowsCareer)
            {
                KnowCareerRadioButtonList.SelectedIndex = 0;
            }
            else
            {
                KnowCareerRadioButtonList.SelectedIndex = 1;
            }
        }

        private void ShowErrorMessage(string message)
        {
            ShowErrorMessage(message, false);
        }

        private void ShowErrorMessage(string message, bool showStar)
        {
            ErrorMessage.Visible = true;
            ErrorMessage.Text = message;

            DetailsUpdatedImage.Visible = false;

            StarErrorImage.Visible = showStar;
            EnableButtons();
        }

        private void DisableButtons()
        {
            ShowErrorMessage("");
            SubmitButton.Enabled = false;
            CancelButton.Enabled = false;
        }

        private void EnableButtons()
        {
            SubmitButton.Enabled = true;
            CancelButton.Enabled = true;
        }

        private static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
    }
}

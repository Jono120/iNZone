using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KioskApplication
{
    public partial class FloatingSecurityHelp : System.Web.UI.UserControl
    {
        #region Javascript
        /// <summary>
        /// The javascript for the confirmation pane
        /// </summary>
        private const string FLOATING_HELP_SCRIPT = @"
        <script language=""javascript"">        
       
        var originalConfirmationDivHTML = """";       
      
        function ShowFloatingHelp()
        {
            document.getElementById('FloatingHelpDimmer').style.width = window.screen.width;
            document.getElementById('FloatingHelpDimmer').style.height = window.screen.height;
            document.getElementById('FloatingHelpDimmer').style.visibility = ""visible"";
            

            
            if (originalConfirmationDivHTML == """")
                originalConfirmationDivHTML = document.getElementById('FloatingHelp').innerHTML;
            
            document.getElementById('FloatingHelp').style.visibility = ""visible"";
           
            document.body.style.overflow = ""hidden"";            

            document.documentElement.style.overflow = ""hidden"";           
        }                 

        
        </script>";
        #endregion

        #region Properties

        /// <summary>
        /// UseQuestion1Button of the floating help
        /// </summary>
        public ImageButton Question1Button
        {
            get { return UseQuestion1Button; }
        }

        /// <summary>
        /// UseQuestion1Button of the floating help
        /// </summary>
        public ImageButton Question2Button
        {
            get { return UseQuestion2Button; }
        }

        /// <summary>
        /// UseQuestion1Button of the floating help
        /// </summary>
        public ImageButton Question3Button
        {
            get { return UseQuestion3Button; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.PreRender += new EventHandler(FloatingSecurityHelp_PreRender);
            CloseButton.Attributes.Add("onmouseover", "src=\"Images/close_icon_over.png\"");
            CloseButton.Attributes.Add("onmouseout", "src=\"Images/close_icon.png\"");
            UseQuestion1Button.Attributes.Add("onmouseover", "src=\"Images/use_question_btn_over.png\"");
            UseQuestion1Button.Attributes.Add("onmouseout", "src=\"Images/use_question_btn.png\"");
            UseQuestion2Button.Attributes.Add("onmouseover", "src=\"Images/use_question_btn_over.png\"");
            UseQuestion2Button.Attributes.Add("onmouseout", "src=\"Images/use_question_btn.png\"");
            UseQuestion3Button.Attributes.Add("onmouseover", "src=\"Images/use_question_btn_over.png\"");
            UseQuestion3Button.Attributes.Add("onmouseout", "src=\"Images/use_question_btn.png\"");
            CreateOwnQuestionButton.Attributes.Add("onmouseover", "src=\"Images/create_own_btn_over.png\"");
            CreateOwnQuestionButton.Attributes.Add("onmouseout", "src=\"Images/create_own_btn.png\"");
        }

        private void FloatingSecurityHelp_PreRender(object sender, EventArgs e)
        {
            // Register the javascript
            if (!Page.IsClientScriptBlockRegistered("clientScriptShowFloatingHelp"))
            {
                Page.RegisterClientScriptBlock("clientScriptShowFloatingHelp", FLOATING_HELP_SCRIPT);
            }

            // Read the appropriate preset security questions from the config file
            SecurityQuestion1.Text = ConfigurationManager.AppSettings["SecurityQuestion1"];
            SecurityQuestion2.Text = ConfigurationManager.AppSettings["SecurityQuestion2"];
            SecurityQuestion3.Text = ConfigurationManager.AppSettings["SecurityQuestion3"];
        }

        /// <summary>
        /// Display the pane on the page 
        /// </summary>
        public void Show()
        {
            this.Visible = true;
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ShowFloatingHelp", "ShowFloatingHelp();", true);
        }
    }
}
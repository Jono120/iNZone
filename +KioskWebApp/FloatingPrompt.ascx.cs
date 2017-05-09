using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KioskApplication
{
    public partial class FloatingAlert : System.Web.UI.UserControl
    {
        #region Properties

        /// <summary>
        /// Yes button of the floating prompt
        /// </summary>
        public ImageButton FloatingPromptYesButton
        {
            get { return YesButton; }
        }

        /// <summary>
        /// No button of hte floating prompt
        /// </summary>
        public ImageButton FloatingPromptNoButton
        {
            get { return NoButton; }
        }

        // <summary>
        /// Message to display on the pane
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        private string _message;

        /// <summary>
        /// Width of the pane
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private int _width;

        /// <summary>
        /// Height of the pane
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private int _height;

        #endregion

        #region Javascript
        /// <summary>
        /// The javascript for the confirmation pane
        /// </summary>
        private const string FLOATING_PROMPT_SCRIPT = @"
        <script language=""javascript"">        
       
        var originalConfirmationDivHTML = """";       
      
        function ShowFloatingPrompt()
        {
            document.getElementById('FloatingPromptDimmer').style.width = window.screen.width;
            document.getElementById('FloatingPromptDimmer').style.height = window.screen.height;
            document.getElementById('FloatingPromptDimmer').style.visibility = ""visible"";
            

            
            if (originalConfirmationDivHTML == """")
                originalConfirmationDivHTML = document.getElementById('FloatingPrompt').innerHTML;
            
            document.getElementById('FloatingPrompt').style.visibility = ""visible"";
           
            document.body.style.overflow = ""hidden"";            

            document.documentElement.style.overflow = ""hidden"";           
        }                 

        
        </script>";
        #endregion
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.PreRender += new EventHandler(FloatingPrompt_PreRender);
            YesButton.Attributes.Add("onmouseover", "src=\"Images/yes_btn_over.png\"");
            YesButton.Attributes.Add("onmouseout", "src=\"Images/yes_btn.png\"");
            NoButton.Attributes.Add("onmouseover", "src=\"Images/no_btn_over.png\"");
            NoButton.Attributes.Add("onmouseout", "src=\"Images/no_btn.png\"");
        }

        private void FloatingPrompt_PreRender(object sender, EventArgs e)
        {
            // Register the javascript
            if (!Page.IsClientScriptBlockRegistered("clientScriptShowFloatingPrompt"))
            {
                Page.RegisterClientScriptBlock("clientScriptShowFloatingPrompt", FLOATING_PROMPT_SCRIPT);
            }

            MessageLabel.Text = Message;
        }

        /// <summary>
        /// Display the pane on the page with the specified heading and message
        /// </summary>
        public void Show()
        {
            this.Visible = true;
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ShowFloatingPrompt", "ShowFloatingPrompt();", true);
        }
    }
}
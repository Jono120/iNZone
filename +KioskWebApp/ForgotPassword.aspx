<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true"
    CodeBehind="ForgotPassword.aspx.cs" Inherits="KioskApplication.ForgotPassword" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        var AjaxEnginePage;

        //Global XMLHTTP Request object
        var XMLHTTP;

        AjaxEnginePage = "AjaxEngine.aspx";

        //Disable text selection on appropriate elements
        window.onload = function () {
            DisableSelection(document.getElementById("wrap"));
        }

        function DisableSelection(target) {
            if (target != null) {
                target.onselectstart = function () { return false; }

            }
        }

        // Disable backspace key navigating back a page - Glenn 20/07/2009
        var inform = false;
        function mykeyhandler() {
            if (inform) return true;
            if (window.event && window.event.keyCode == 8) { // try to cancel the backspace
                window.event.cancelBubble = true;
                window.event.returnValue = false;
                return false;
            }
        }

        document.onkeydown = mykeyhandler;


        //Creating and setting the instance of appropriate XMLHTTP Request object to a “XmlHttp” variable  
        function CreateXMLHTTP() {

            try {
                XMLHTTP = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    XMLHTTP = new ActiveXObject("Microsoft.XMLHTTP");

                }
                catch (oc) {
                    XMLHTTP = null;

                }
            }
            //Creating object in Mozilla and Safari 
            if (!XMLHTTP && typeof XMLHttpRequest != "undefined") {
                XMLHTTP = new XMLHttpRequest();
            }
        }

        function LoadSecurityQuestion() {

            var username = document.getElementById('<%= UsernameTextBox.ClientID %>').value;

            if (username != '') {

                var requestUrl = AjaxEnginePage + "?Action=LoadSecurityQuestion&Username=" + username;

                CreateXMLHTTP();

                // If browser supports XMLHTTPRequest object
                if (XMLHTTP) {

                    //Setting the event handler for the response
                    XMLHTTP.onreadystatechange = CheckSecurityQuestionFound;

                    //Initializes the request object with GET (METHOD of posting), 
                    //Request URL and sets the request as asynchronous.
                    XMLHTTP.open("POST", requestUrl, true);

                    //Sends the request to server
                    XMLHTTP.send(null);
                }
            }
        }

        function CheckSecurityQuestionFound() {

            //Display security question or error message
            if (XMLHTTP.readyState == 4) {

                //Valid Response is received
                if (XMLHTTP.status == 200) {

                    DisplaySecurityQuestion(XMLHTTP.responseXML.documentElement);
                }
            }
        }

        function DisplaySecurityQuestion(response) {

            if (response != null) {
                var usernameFound = GetInnerText((response.getElementsByTagName('UsernameOK'))[0]);

                if (usernameFound == 'False') {
                    //Display error message since username is not found
                    document.getElementById('<%= SecurityQuestionDisplayLabel.ClientID %>').innerHTML = '';
                    document.getElementById('<%= ErrorMessage.ClientID %>').innerHTML = 'Username is incorrect. Please try again.';
                    document.getElementById('<%= ErrorMessage.ClientID %>').style.visibility = "visible";
                }
                else {

                    var securityQuestion = GetInnerText((response.getElementsByTagName('SecurityQuestion'))[0]);
                    document.getElementById('<%= SecurityQuestionDisplayLabel.ClientID %>').innerHTML = securityQuestion;
                    document.getElementById('<%= ErrorMessage.ClientID %>').style.visibility = "hidden";
                }
            }
        }

        function GetInnerText(node) {
            return (node.textContent || node.innerText || node.text);
        }

    </script>

    <div id="wrap">
        <div class="forgot-password-fields">
            <asp:Panel ID="Panel1" DefaultButton="LoginButton" runat="server">
                <table style="position: fixed; width: 900px; left: 190px; top: 300px;">
                    <tr>
                        <td class="login-first-time-column">
                            <asp:Image runat="server" ID="FirstTimeHeader" ImageUrl="Images/first_time_hdr.png" />
                        </td>
                        <td>
                        </td>
                        <td align="left">
                            <asp:Image runat="server" ID="Image1" ImageUrl="Images/returning_hdr.png" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="Image16" runat="server" ImageUrl="Images/shim.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:ImageButton ID="RegisterButton" runat="server" ImageUrl="Images/register_now_btn.png"
                                CausesValidation="false" TabIndex="5" />
                        </td>
                        <td class="login-column">
                            <asp:Label ID="UsernameLabel" runat="server" CssClass="login-label">Username</asp:Label>
                        </td>
                        <td class="login-column">
                            <asp:TextBox runat="server" ID="UsernameTextBox" CssClass="login-textbox" MaxLength="30"
                                TabIndex="1" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="login-column">
                            <asp:Label ID="SecurityQuestionLabel" runat="server" CssClass="login-label">Security Question</asp:Label>
                        </td>
                        <td class="login-column">
                            <asp:Label runat="server" ID="SecurityQuestionDisplayLabel" CssClass="login-label"
                                Text="Your security question will appear here"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="login-column">
                            <asp:Label ID="SecurityAnswerLabel" runat="server" CssClass="login-label">Security Answer</asp:Label>
                        </td>
                        <td class="login-column">
                            <asp:TextBox runat="server" ID="SecurityAnswerTextBox" CssClass="login-textbox" MaxLength="30"
                                TabIndex="2" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td style="text-align: left;">
                            <asp:RequiredFieldValidator ID="UsernameRequiredFieldValidator" ControlToValidate="UsernameTextBox"
                                Display="None" runat="server" ErrorMessage="" CssClass="form-validator"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="SecurityAnswerRequiredFieldValidator" ControlToValidate="SecurityAnswerTextBox"
                                Display="None" runat="server" ErrorMessage="" CssClass="form-validator"></asp:RequiredFieldValidator>
                            <asp:Label ID="ErrorMessage" CssClass="form-validator" runat="server" Visible="true"
                                Text=" "></asp:Label>
                        </td>
                    </tr>
                </table>
        </div>
        <div class="forgot-password-buttons">
            <table cellspacing="8px">
                <tr>
                    <td>
                        <asp:ImageButton ID="LoginButton" runat="server" ImageUrl="Images/login_btn.png"
                            CausesValidation="false" TabIndex="3" d />
                    </td>
                    <td>
                        <asp:Image ID="Image2" runat="server" ImageUrl="Images/shim.gif" />
                    </td>
                    <td>
                        <asp:ImageButton ID="CancelButton" runat="server" ImageUrl="Images/cancel_btn.png"
                            CausesValidation="false" TabIndex="4" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

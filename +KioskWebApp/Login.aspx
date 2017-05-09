<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="KioskApplication.Login" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">


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
    

    </script>

    <div id="wrap">
        <div class="login-fields">
            <asp:Panel ID="Panel1" runat="server" DefaultButton="LoginButton">
                <table style="position: fixed; width: 900px; left: 190px; top: 300px;">
                    <tr>
                        <td class="login-first-time-column">
                            <asp:Image runat="server" ID="FirstTimeHeader" ImageUrl="Images/first_time_hdr.png" />
                        </td>
                        <td class="style3">
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
                        <td rowspan="6">
                            <asp:ImageButton ID="RegisterButton" runat="server" ImageUrl="Images/register_now_btn.png"
                                CausesValidation="false" TabIndex="5" />
                        </td>
                       <td class="style4">
                            <asp:Label ID="Label1" runat="server" CssClass="login-label">Mobile Phone</asp:Label>
                        </td>
                        <td class="login-column">
                            <asp:TextBox runat="server" ID="MobilePhoneTextBox" CssClass="login-textbox" MaxLength="30"
                                TabIndex="1" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr><td colspan="2"></td></tr>
                    <tr><td colspan="2"><span class="login-label">OR</span></td></tr>
                    <tr><td colspan="2"></td></tr>
                    <tr>
                        <td class="style4">
                            <asp:Label ID="UsernameLabel" runat="server" CssClass="login-label">Username</asp:Label>
                        </td>
                        <td class="login-column">
                            <asp:TextBox runat="server" ID="UsernameTextBox" CssClass="login-textbox" MaxLength="30"
                                TabIndex="1" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style4">
                            <asp:Label ID="PasswordLabel" runat="server" CssClass="login-label">Password</asp:Label>
                        </td>
                        <td class="login-column">
                            <asp:TextBox runat="server" ID="PasswordTextBox" TextMode="Password" CssClass="login-textbox"
                                MaxLength="20" TabIndex="2" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="style3">
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="ErrorMessage" CssClass="form-validator" runat="server" Visible="true"
                                Text=" "></asp:Label>
                        </td>
                    </tr>
                </table>
        </div>
        <div class="login-buttons">
            <table cellspacing="8px">
                <tr>
                    <td>
                        <asp:ImageButton ID="LoginButton" runat="server" ImageUrl="Images/login_btn.png"
                            CausesValidation="false" TabIndex="3" />
                    </td>
                    <td>
                        <asp:Image ID="Image2" runat="server" ImageUrl="Images/shim.gif" />
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            </asp:Panel>
        </div>
    </div>
                        <asp:ImageButton ID="ForgotPasswordButton" runat="server" ImageUrl="Images/forgot_password_btn.png"
                            CausesValidation="false" TabIndex="4" Enabled="False" 
		Visible="False" />
                    </asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style1
        {
            width: 82px;
        }
        .style2
        {
            text-align: left;
            width: 82px;
        }
        .style3
        {
            width: 104px;
        }
        .style4
        {
            text-align: left;
            width: 104px;
        }
    </style>
</asp:Content>


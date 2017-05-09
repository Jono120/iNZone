<%@ Page Title="" Language="C#" MasterPageFile="~/Global.Master" AutoEventWireup="true"
    CodeBehind="Registration.aspx.cs" Inherits="KioskApplication.Registration" %>
<%@ Register TagName="FloatingHelp" TagPrefix="UserControl" Src="FloatingSecurityHelp.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">



        //Disable text selection on appropriate elements
        window.onload = function() {
            DisableSelection(document.getElementById("Registration"));
        }

        function DisableSelection(target) {
            if (target != null) {
                target.onselectstart = function() { return false; }

            }
        }

       
    
    </script>

    <div id="Registration">
        <UserControl:FloatingHelp ID="FloatingHelp" runat="server" Visible="false">
        </UserControl:FloatingHelp>  
        <div class="left-margin">
            <asp:Image runat="server" ImageUrl="Images/registration_hdr.png" />
        </div>
        <br />
        <table cellpadding="5" width="600px" class="left-margin">
            <tr class="form-row">
                <td class="form-label-column">
                    <asp:Label ID="Label1" runat="server" CssClass="form-label">First Name</asp:Label>
                </td>
                <td class="form-textbox-column">
                    <asp:Image runat="server" ImageUrl="Images/red_asterix.png" />
                    <asp:TextBox runat="server" ID="FirstNameTextBox" CssClass="form-textbox" TabIndex="1"
                        MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" CssClass="form-label">Last Name</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image3" runat="server" ImageUrl="Images/red_asterix.png" CssClass="required-star" />
                    <asp:TextBox runat="server" ID="LastNameTextBox" CssClass="form-textbox" TabIndex="2"
                        MaxLength="30"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" CssClass="form-label">Email</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image10" runat="server" ImageUrl="Images/shim_20px.gif" />
                    <asp:TextBox runat="server" ID="EmailTextBox" CssClass="form-textbox" TabIndex="3"
                        MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="form-security-label-column">
                    <asp:Label ID="Label9" runat="server" CssClass="form-label">Mobile Phone</asp:Label>
                </td>
                <td class="form-security-textbox-column">
                    <asp:Image ID="Image5" runat="server" ImageUrl="Images/red_asterix.png" CssClass="required-star" />
                    <asp:TextBox runat="server" ID="MobilePhoneTextBox" CssClass="form-security-textbox"
                        TabIndex="15" MaxLength="30"></asp:TextBox>
                </td>
            </tr>

        </table>
        <table class="left-margin">
            <tr>
                <td colspan="2" rowspan="2" style="width: 887px; text-align: left;">
                    <asp:Image ID="StarErrorImage" runat="server" ImageUrl="Images/red_asterix.png" visible="false"/>
                    <asp:Label ID="ErrorMessage" runat="server" Text=" " CssClass="registration-form-validator"></asp:Label>
                    <asp:RequiredFieldValidator ID="FirstNameRequiredFieldValidator" ControlToValidate="FirstNameTextBox"
                        runat="server" ErrorMessage="" CssClass="form-validator" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="LastNameRequiredFieldValidator" ControlToValidate="LastNameTextBox"
                        runat="server" ErrorMessage="" CssClass="form-validator" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="MobilePhoneRequiredFieldValidator" ControlToValidate="MobilePhoneTextBox"
                        Display="None" runat="server" ErrorMessage="" CssClass="form-validator"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="EmailRegularExpressionValidator" runat="server"
                        CssClass="form-validator" ErrorMessage="" Display="None" ControlToValidate="EmailTextBox"
                        ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"></asp:RegularExpressionValidator>
                   <asp:RegularExpressionValidator ID="ContactPhoneRegularExpressionValidator" runat="server" CssClass="form-validator"
						ErrorMessage="" Display="None" EnableClientScript="true" ControlToValidate="MobilePhoneTextBox"
                        ValidationExpression="[0-9]{7,20}"></asp:RegularExpressionValidator>  

                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    &nbsp;
                    <asp:ImageButton ID="SubmitButton" runat="server" ImageUrl="Images/submit_btn.png"
                        TabIndex="20" CausesValidation="false" />
                    &nbsp;
                    <asp:ImageButton ID="CancelButton" runat="server" ImageUrl="Images/cancel_btn.png"
                        CausesValidation="false" TabIndex="21" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
</asp:Content>

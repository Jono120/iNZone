<%@ Page Title="" Language="C#" MasterPageFile="~/Global.Master" AutoEventWireup="true"
    CodeBehind="MyDetails.aspx.cs" Inherits="KioskApplication.MyDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagName="FloatingPrompt" TagPrefix="UserControl" Src="FloatingPrompt.ascx" %>
<%@ Register TagName="FloatingHelp" TagPrefix="UserControl" Src="FloatingSecurityHelp.ascx" %>
<%@ Register Src="NavigationControl.ascx" TagName="Navigation" TagPrefix="UserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        //Disable text selection on appropriate elements
        window.onload = function () {
            DisableSelection(document.getElementById("MyDetails"));

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

    <div id="MyDetails">
   <UserControl:FloatingPrompt ID="FloatingPromptControl" runat="server" Visible="false">
        </UserControl:FloatingPrompt>    
         <UserControl:FloatingHelp ID="FloatingHelp" runat="server" Visible="false">
        </UserControl:FloatingHelp>  
        <div class="left-margin">
            <asp:Image ID="Image1" runat="server" ImageUrl="Images/my_details_hdr.png" />
        </div>
        <br />
        <table cellpadding="5" width="98%" class="left-margin">
            <tr class="form-row">
                <td class="form-label-column">
                    <asp:Label ID="Label1" runat="server" CssClass="form-label">First Name</asp:Label>
                </td>
                <td class="form-textbox-column">
                    <asp:Image ID="Image2" runat="server" ImageUrl="Images/red_asterix.png" />
                    <asp:TextBox runat="server" ID="FirstNameTextBox" CssClass="form-textbox" TabIndex="1"
                        MaxLength="30" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td class="form-label-column2">
                    <asp:Label ID="Label2" runat="server" CssClass="form-label">Address</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image3" runat="server" ImageUrl="Images/shim_20px.gif" />
                    <asp:TextBox runat="server" ID="Address1TextBox" CssClass="form-textbox" TabIndex="11"
                        MaxLength="50" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" CssClass="form-label">Last Name</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image4" runat="server" ImageUrl="Images/red_asterix.png" CssClass="required-star" />
                    <asp:TextBox runat="server" ID="LastNameTextBox" CssClass="form-textbox" TabIndex="2"
                        MaxLength="30" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td>
                    
                    <asp:Label ID="Label5" runat="server" CssClass="form-label">Suburb</asp:Label>
                    
                </td>
                <td>
                    <asp:Image ID="Image13" runat="server" ImageUrl="Images/shim_20px.gif" />
                    <asp:TextBox runat="server" ID="SuburbTextBox" CssClass="form-textbox" TabIndex="13"
                        MaxLength="50" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" CssClass="form-label">Email</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image10" runat="server" ImageUrl="Images/shim_20px.gif" />
                    <asp:TextBox runat="server" ID="EmailTextBox" CssClass="form-textbox" TabIndex="3"
                        MaxLength="50" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" CssClass="form-label">Town/City</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image5" runat="server" ImageUrl="Images/shim_20px.gif" />
                    <asp:TextBox runat="server" ID="TownCityTextBox" CssClass="form-textbox" TabIndex="14"
                        MaxLength="30" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label6" runat="server" CssClass="form-label">Confirm Email</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image11" runat="server" ImageUrl="Images/shim_20px.gif" />
                    <asp:TextBox runat="server" ID="ConfirmEmailTextBox" CssClass="form-textbox" TabIndex="4"
                        MaxLength="50" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                </td>
                <td>
                    
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label10" runat="server" CssClass="form-label">Date of Birth (E.g. 15/02/95)</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image7" runat="server" ImageUrl="Images/shim_20px.gif" />
                    <asp:TextBox ID="DayTextBox" runat="server" MaxLength="2" CssClass="form-dob"  TabIndex="5" onfocus="inform=true" onblur="inform=false"></asp:TextBox><asp:Label ID="Label17"
                        runat="server" CssClass="form-dob-label" Text="/"></asp:Label>
                    <asp:TextBox ID="MonthTextBox" runat="server" MaxLength="2" CssClass="form-dob"  TabIndex="6" onfocus="inform=true" onblur="inform=false"></asp:TextBox><asp:Label
                        ID="Label18" runat="server" CssClass="form-dob-label" Text="/"></asp:Label>
                    <asp:TextBox ID="YearTextBox" runat="server" MaxLength="2" CssClass="form-dob" TabIndex="7" onfocus="inform=true" onblur="inform=false"></asp:TextBox>   
                </td>
                <td colspan="3">
                   
                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label ID="Label8" runat="server" CssClass="form-label">Gender</asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="GenderRadioButtonList" runat="server" RepeatDirection="Horizontal"
                        CssClass="form-radio" TabIndex="8">
                        <asp:ListItem Text="Male" Selected="True" Value="M"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="F"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>   
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label15" runat="server" CssClass="form-label">Mobile Phone</asp:Label>
                </td>
                <td>
                    <asp:Image ID="Image6" runat="server" ImageUrl="Images/red_asterix.png" />
                    <asp:TextBox runat="server" ID="ContactPhoneTextBox" CssClass="form-textbox" TabIndex="9"
                        MaxLength="30" onfocus="inform=true" onblur="inform=false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label16" runat="server" CssClass="form-label">Do you know what career you want?</asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="KnowCareerRadioButtonList" runat="server" RepeatDirection="Horizontal"
                        CssClass="form-radio" TabIndex="10">
                        <asp:ListItem Text="Yes" Selected="True" Value="true"></asp:ListItem>
                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                &nbsp;
                </td>
            </tr>
             
            
        </table>
        <table class="left-margin">
            <tr>
                <td style="width: 635px; text-align: left;">
                    <asp:Image runat="server" ID="DetailsUpdatedImage" ImageUrl="Images/details_updated.png"
                        Visible="false" ImageAlign="Left" />
                    <asp:Image ID="StarErrorImage" runat="server" ImageUrl="Images/red_asterix.png" visible="false"/>
                    <asp:Label ID="ErrorMessage" runat="server" Text=" " CssClass="registration-form-validator"></asp:Label>
                    <asp:RequiredFieldValidator ID="FirstNameRequiredFieldValidator" ControlToValidate="FirstNameTextBox"
                        runat="server" ErrorMessage="" CssClass="form-validator" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="LastNameRequiredFieldValidator" ControlToValidate="LastNameTextBox"
                        runat="server" ErrorMessage="" CssClass="form-validator" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="EmailRegularExpressionValidator" runat="server"
                        CssClass="form-validator" ErrorMessage="" Display="None" ControlToValidate="EmailTextBox"
                        ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"></asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="EmailCompareValidator" runat="server" ControlToCompare="ConfirmEmailTextBox"
                        Display="None" ControlToValidate="EmailTextBox" ErrorMessage="" CssClass="form-validator"></asp:CompareValidator>
                    <asp:CompareValidator ID="EmailCompareValidator2" runat="server" ControlToCompare="EmailTextBox"
                        Display="None" ControlToValidate="ConfirmEmailTextBox" ErrorMessage="" CssClass="form-validator"></asp:CompareValidator>
                    <asp:RegularExpressionValidator ID="DayRegularExpressionValidator" runat="server"
                        CssClass="form-validator" ErrorMessage="" Display="None" EnableClientScript="true"
                        ControlToValidate="DayTextBox" ValidationExpression="(0[1-9]|[12][0-9]|3[01])"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="MonthRegularExpressionValidator" runat="server" CssClass="form-validator"
                        ErrorMessage="" Display="None" EnableClientScript="true" ControlToValidate="MonthTextBox"
                        ValidationExpression="(0[1-9]|1[012])"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="YearRegularExpressionValidator" runat="server" CssClass="form-validator"
                        ErrorMessage="" Display="None" EnableClientScript="true" ControlToValidate="YearTextBox"
                        ValidationExpression="\d\d"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="ContactPhoneRegularExpressionValidator" runat="server" CssClass="form-validator"
						ErrorMessage="" Display="None" EnableClientScript="true" ControlToValidate="ContactPhoneTextBox"
                        ValidationExpression="[0-9]{7,20}"></asp:RegularExpressionValidator>  
                    <asp:RequiredFieldValidator ID="ContactPhoneRequiredValidator" ControlToValidate="ContactPhoneTextBox"
                        runat="server" ErrorMessage="" CssClass="form-validator" Display="None"></asp:RequiredFieldValidator>
                   
                        
                        <asp:RequiredFieldValidator ID="GenderRequiredFieldValidator" ControlToValidate="GenderRadioButtonList"
                       Display="None" runat="server" ErrorMessage="" CssClass="form-validator"></asp:RequiredFieldValidator>
                       
                       <asp:RequiredFieldValidator ID="KnowCareerRequiredFieldValidator" ControlToValidate="KnowCareerRadioButtonList"
                       Display="None" runat="server" ErrorMessage="" CssClass="form-validator"></asp:RequiredFieldValidator>
                </td>
                <td align="right">
                    <asp:ImageButton ID="SubmitButton" runat="server" ImageUrl="Images/submit_btn.png"
                        CausesValidation="false" TabIndex="20" />
                    &nbsp
                    <asp:ImageButton ID="CancelButton" runat="server" ImageUrl="Images/back_btn.png"
                        CausesValidation="false" TabIndex="21" />
                    &nbsp
                    <asp:ImageButton ID="PrivacyStatementButton" runat="server" ImageUrl="Images/privacy_statement_btn.png"
                        CausesValidation="false" TabIndex="22"/>
                </td>
            </tr>
        </table>
        <div class="navigation-control">
            <UserControl:Navigation runat="server" ID="NavigationControl"></UserControl:Navigation>
        </div>
    </div>
</asp:Content>

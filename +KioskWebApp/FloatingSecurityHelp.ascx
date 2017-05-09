<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FloatingSecurityHelp.ascx.cs"
    Inherits="KioskApplication.FloatingSecurityHelp" %>
<div class="floating-help" style="visibility: hidden;" id="FloatingHelp">
    <table>
        <tr>
            <td>
                <asp:Image runat="server" ID="HeadingImage" ImageUrl="Images/kiosk_help_hdr.png"/>
            </td>
            <td class="floating-help-close-column">
                <asp:ImageButton runat="server" ID="CloseButton" ImageUrl="Images/close_icon.png"  CausesValidation="false"/>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label runat="server" ID="QuestionLabel" Text="What is a security question?" class="floating-help-explain-question"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">            
                <asp:Label runat="server" ID="DescriptionLabel" Text="It's the question we prompt you with if you forget your password when you login to the kiosk. Here are a few examples:"></asp:Label>
            </td>
        </tr>        
    </table>
    <br />
    <table>
        <tr>
            <td class="floating-help-question-column">
                <asp:Label runat="server" ID="SecurityQuestion1"></asp:Label>
            </td>
            <td class="floating-help-question-button-column">
                <asp:ImageButton runat="server" ID="UseQuestion1Button" ImageUrl="Images/use_question_btn.png"  CausesValidation="false"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="SecurityQuestion2"></asp:Label>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="UseQuestion2Button" ImageUrl="Images/use_question_btn.png"  CausesValidation="false"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="SecurityQuestion3"></asp:Label>
            </td>
            <td>
                <asp:ImageButton runat="server" ID="UseQuestion3Button" ImageUrl="Images/use_question_btn.png"  CausesValidation="false"/>
            </td>
        </tr>
    
    </table>
    
    <div class="floating-help-button">
        <asp:ImageButton runat="server" ID="CreateOwnQuestionButton" ImageUrl="Images/create_own_btn.png"  CausesValidation="false"></asp:ImageButton>
    </div>
</div>
<div id="FloatingHelpDimmer" class="dimmer">
</div>

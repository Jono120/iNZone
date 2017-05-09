<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FloatingPrompt.ascx.cs" Inherits="KioskApplication.FloatingAlert" %>
<div class="floating-prompt" style="visibility: hidden" id="FloatingPrompt">
    <div class="floating-prompt-message">
        <asp:Literal ID="MessageLabel" runat="server"></asp:Literal>
    </div>
    
    <div class="floating-prompt-buttons">
        <asp:ImageButton ID="YesButton" runat="server" ImageUrl="Images/yes_btn.png" CausesValidation="false"></asp:ImageButton>
        <asp:ImageButton ID="NoButton" runat="server" ImageUrl="Images/no_btn.png" CausesValidation="false"></asp:ImageButton>
    </div>
</div>
<div id="FloatingPromptDimmer" class="dimmer">
</div>
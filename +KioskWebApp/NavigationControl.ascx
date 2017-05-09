<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavigationControl.ascx.cs"
    Inherits="KioskApplication.NavigationControl" %>
<table cellpadding="5px" >
    <tr >
        <td colspan="3">
            <asp:Label ID="LoggedInAsLabel" runat="server" Text="You are logged in as " CssClass="navigation-logged-in-as"></asp:Label>
            <asp:Label ID="ParticipantLabel" runat="server" CssClass="navigation-participant-name"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:ImageButton ID="HomeImageButton" runat="server" ImageUrl="Images/home_btn.png" />
        </td>
        <td>
            <asp:ImageButton ID="MyDetailsImageButton" runat="server" ImageUrl="Images/my_details_btn.png" />
        </td>
        <td>
            <asp:ImageButton ID="LogoutImageButton" runat="server" ImageUrl="Images/log_out_btn.png" />
        </td>
    </tr>
</table>

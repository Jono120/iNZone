<%@ Page Language="C#" MasterPageFile="~/Video.Master" AutoEventWireup="true" CodeBehind="JobSearch.aspx.cs" Inherits="KioskApplication.JobSearch" %>
<%@ Register Src="NavigationControl.ascx" TagName="Navigation" TagPrefix="UserControl" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:PlaceHolder ID="IFramePlaceHolder" runat="server" />

 <div class="navigation-control">
            <UserControl:Navigation runat="server" ID="NavigationControl1"></UserControl:Navigation>
</div>
</asp:Content>

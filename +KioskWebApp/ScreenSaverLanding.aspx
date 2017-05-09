<%@ Page Title="" Language="C#" MasterPageFile="~/Global.Master" AutoEventWireup="true"
    CodeBehind="ScreenSaverLanding.aspx.cs" Inherits="KioskApplication.ScreenSaverLanding" %>

<%--<%@ Register Src="NavigationControl.ascx" TagName="Navigation" TagPrefix="UserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript">

        //Disable text selection on appropriate elements
        window.onload = function() {
        DisableSelection(document.getElementById("ScreenSaverLanding"));
        }

        function DisableSelection(target) {
            if (target != null) {
                target.onselectstart = function() { return false; }
                
            }
        }     
    
    </script>
<div id="Landing">
    <table>
        <tr>
            <td>
                <div class="landing-top">
                    <div class="landing-top-right">
                        <asp:Menu ID="TabMenu" runat="server" Orientation="Horizontal" StaticEnableDefaultPopOutImage="false"
                            CssClass="landing-tabs">
                            <Items>
                            </Items>
                        </asp:Menu>
                        <div class="landing-bottom">
                            <div class="landing-left">
                                <div class="landing-right">
                                    <div class="landing-bottom-left">
                                        <div class="landing-bottom-right">
                                            <asp:MultiView runat="server" ID="LandingMultiView">
                                            </asp:MultiView>
                                            <div class="space">
                                            
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="ErrorMessage" Visible="false" CssClass="form-validator"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
 </div>
</asp:Content>

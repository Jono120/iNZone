<%@ Page Title="" Language="C#" MasterPageFile="~/ScreenSaverVideo.Master" AutoEventWireup="true"
    CodeBehind="ScreenSaverVideos.aspx.cs" Inherits="KioskApplication.ScreenSaverVideos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="NavigationControl.ascx" TagName="Navigation" TagPrefix="UserControl" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
    	var AjaxEnginePage;

    	var XMLHTTP;

    	AjaxEnginePage = "AjaxEngine.aspx";

    	//Disable text selection on appropriate elements
    	window.onload = function() {
    		DisableSelection(document.getElementById("Videos"));
    	}

    	function DisableSelection(target) {
    		if (target != null) {
    			target.onselectstart = function() { return false; }

    		}
    	}

    	//Global XMLHTTP Request object

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




    </script>

    <div id="Videos">
        <table class="video-header-section" cellspacing="5px" cellpadding="10px">
            <tr>
                <td class="video-header-logo-cell">
                    <asp:Image ID="PartnerLogoImage" runat="server" CssClass="video-header-logo-image" />
                </td>
            </tr>
        </table>
        <br />
        <table class="video-rating-section">
            <tr>
            </tr>
            <tr valign="top">
                <td class="video-player-cell" colspan="2">
                    <asp:Literal ID="VideoPlayerLiteral" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
        <div class="video-footer-position">
        </div>
        <div id="FloatingDimmer" class="video-page-dimmer">
        </div>

        <script language="javascript" type="text/javascript">
        	function pageLoad() {
        		//Find all the RatingBehavior and attach mouse over event handler, so we can control
        		//what is displayed on the tool tip when mouse over
        		var currentBehavior = null;
        		var allBehaviors = Sys.Application.getComponents();
        		for (var loopIndex = 0; loopIndex < allBehaviors.length; loopIndex++) {
        			currentBehavior = allBehaviors[loopIndex];
        			if (currentBehavior.get_name() == "RatingBehavior") {
        				currentBehavior.add_MouseOver(ratingOnMouseOver);
        			}
        		}
        	}
        	function ratingOnMouseOver(sender, eventArgs) {
        		var elt = sender.get_element();
        		//Set the tool tip to nothing         
        		$get(elt.id + "_A").title = "";
        	}    
        </script>

    </div>
</asp:Content>

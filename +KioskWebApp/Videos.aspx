<%@ Page Title="" Language="C#" MasterPageFile="~/Video.Master" AutoEventWireup="true"
    CodeBehind="Videos.aspx.cs" Inherits="KioskApplication.Videos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="NavigationControl.ascx" TagName="Navigation" TagPrefix="UserControl" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var AjaxEnginePage;

        var XMLHTTP;

        AjaxEnginePage = "AjaxEngine.aspx";

        //Disable text selection on appropriate elements
        window.onload = function () {
            DisableSelection(document.getElementById("Videos"));
        }

        function DisableSelection(target) {
            if (target != null) {
                target.onselectstart = function () { return false; }

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

        function SubscribeYes_OnClick(shortPartnerName) {
            //Handles subscribe yes button click          

            // construct the URL
            var requestUrl = AjaxEnginePage + "?Action=Subscribe&Subscribe=true&ShortPartnerName=" + shortPartnerName;

            CreateXMLHTTP();

            // If browser supports XMLHTTPRequest object
            if (XMLHTTP) {

                //Setting the event handler for the response
                XMLHTTP.onreadystatechange = CheckSubscribeCompleted;

                //Initializes the request object with GET (METHOD of posting), 
                //Request URL and sets the request as asynchronous.
                XMLHTTP.open("POST", requestUrl, true);

                //Sends the request to server
                XMLHTTP.send(null);
            }
        }

        function SubscribeNo_OnClick(shortPartnerName) {
            //Handles subscribe no button click

            // construct the URL
            var requestUrl = AjaxEnginePage + "?Action=Subscribe&Subscribe=false&ShortPartnerName=" + shortPartnerName;

            CreateXMLHTTP();

            // If browser supports XMLHTTPRequest object
            if (XMLHTTP) {

                //Setting the event handler for the response
                XMLHTTP.onreadystatechange = CheckSubscribeCompleted;

                //Initializes the request object with GET (METHOD of posting), 
                //Request URL and sets the request as asynchronous.
                XMLHTTP.open("POST", requestUrl, true);

                //Sends the request to server
                XMLHTTP.send(null);
            }
        }

        function SuggestedMovie_OnClick(currentPartnerID, partnerID, partnerName, category, videoID) {

            //Handles suggested movie click

            var requestUrl = AjaxEnginePage + "?Action=Check&CurrentPartnerID=" + currentPartnerID + "&PartnerID=" + partnerID + "&Cat=" + category + "&VideoID=" + videoID + "&PartnerName=" + partnerName;

            CreateXMLHTTP();

            // If browser supports XMLHTTPRequest object
            if (XMLHTTP) {

                //Setting the event handler for the response
                XMLHTTP.onreadystatechange = CheckCompleted;

                //Initializes the request object with GET (METHOD of posting), 
                //Request URL and sets the request as asynchronous.
                XMLHTTP.open("POST", requestUrl, true);

                //Sends the request to server
                XMLHTTP.send(null);
            }
        }

        function Navigation_OnClick(currentPartnerID, toPage) {

            //Handles suggested movie click

            var requestUrl = AjaxEnginePage + "?Action=CheckForNavigation&CurrentPartnerID=" + currentPartnerID + "&ToPage=" + toPage;

            CreateXMLHTTP();

            // If browser supports XMLHTTPRequest object
            if (XMLHTTP) {

                //Setting the event handler for the response
                XMLHTTP.onreadystatechange = CheckNavigationCompleted;

                //Initializes the request object with GET (METHOD of posting), 
                //Request URL and sets the request as asynchronous.
                XMLHTTP.open("POST", requestUrl, true);

                //Sends the request to server
                XMLHTTP.send(null);
            }
        }

        function CheckNavigationCompleted() {

            // Highlight the "Would you like more information" label as appropriate

            if (XMLHTTP.readyState == 4) {

                //Valid Response is received
                if (XMLHTTP.status == 200) {
                    HighlightLabelOrNavigate(XMLHTTP.responseXML.documentElement);
                }
            }
        }

        function CheckCompleted() {
            // Highlight the "Would you like more information" label as appropriate

            if (XMLHTTP.readyState == 4) {

                //Valid Response is received
                if (XMLHTTP.status == 200) {

                    HighlightLabelOrRedirect(XMLHTTP.responseXML.documentElement);
                }
            }
        }

        //Subscribe answer has been saved, so unhighlight the question
        function CheckSubscribeCompleted() {
            if (XMLHTTP.readyState == 4) {

                //Valid Response is received
                if (XMLHTTP.status == 200) {

                    UnhighlightQuestion(XMLHTTP.responseXML.documentElement);
                }
            }
        }

        function HighlightLabelOrRedirect(response) {

            if (response != null) {
                var answered = response.getElementsByTagName('Answered');
                var answer = GetInnerText(answered[0]);

                if (answer == 'False') {

                    HighlightQuestion();
                }
                else {

                    var partnerID = GetInnerText((response.getElementsByTagName('PartnerID'))[0]);
                    var partnerName = GetInnerText((response.getElementsByTagName('PartnerName'))[0]);
                    var category = GetInnerText((response.getElementsByTagName('Cat'))[0]);
                    var videoID = GetInnerText((response.getElementsByTagName('VideoID'))[0]);

                    window.location = "Videos.aspx?PartnerName=" + partnerName + "&Cat=" + category + "&PartnerID=" + partnerID + "&VideoID=" + videoID;

                }
            }
        }

        function Redirect(partnerName, category, partnerID, videoID) {
            window.location = "Videos.aspx?PartnerName=" + partnerName + "&Cat=" + category + "&PartnerID=" + partnerID + "&VideoID=" + videoID;
        }

        function HighlightLabelOrNavigate(response) {

            if (response != null) {
                var answered = response.getElementsByTagName('Answered');
                var answer = GetInnerText(answered[0]);

                if (answer == 'False') {

                    HighlightQuestion();
                }
                else {

                    var toPage = GetInnerText((response.getElementsByTagName('ToPage'))[0]);
                    window.location = toPage;
                }
            }
        }

        function HighlightQuestion() {
            document.getElementById('<%= SubscribeLabel.ClientID %>').style.color = '#dee648';
            document.getElementById('<%= SubscribeHiddenField.ClientID %>').value = 'Highlight';
            document.getElementById('FloatingDimmer').style.width = window.screen.width;
            document.getElementById('FloatingDimmer').style.height = window.screen.height;
            document.getElementById('FloatingDimmer').style.visibility = "visible";
        }

        function UnhighlightQuestion(response) {
            if (document.getElementById('<%= SubscribeHiddenField.ClientID %>').value != '') {
                document.getElementById('<%= SubscribeHiddenField.ClientID %>').value = '';
                document.getElementById('<%= SubscribeLabel.ClientID %>').style.color = '#FFFFFF';
                document.getElementById('FloatingDimmer').style.visibility = "hidden";

                if (response != null) {

                    var toPage = GetInnerText((response.getElementsByTagName('ToPage'))[0]);

                    if (toPage != 'None') {
                        window.location = toPage;
                    }
                    else {

                        var partnerID = GetInnerText((response.getElementsByTagName('PartnerID'))[0]);
                        var partnerName = GetInnerText((response.getElementsByTagName('PartnerName'))[0]);
                        var category = GetInnerText((response.getElementsByTagName('Cat'))[0]);
                        var videoID = GetInnerText((response.getElementsByTagName('VideoID'))[0]);

                        window.location = "Videos.aspx?PartnerName=" + partnerName + "&Cat=" + category + "&PartnerID=" + partnerID + "&VideoID=" + videoID;
                    }
                }
            }
            else {

                //Redirect to the landing page instead of changing the label 
                window.location = "Landing.aspx";


                //                var shortPartnerName = GetInnerText((response.getElementsByTagName('ShortPartnerName'))[0]);
                //                var subscribeOption = GetInnerText((response.getElementsByTagName('SubscribeOption'))[0]);

                //                document.getElementById('<%= SubscribeLabel.ClientID %>').style.color = '#dee648';

                //               
                //                if (subscribeOption == 'True')
                //                   document.getElementById('<%= SubscribeLabel.ClientID %>').innerHTML = shortPartnerName + " will get in touch with you shortly";
                //                else if (subscribeOption == 'False')
                //                    document.getElementById('<%= SubscribeLabel.ClientID %>').innerHTML = "You will not receive any information from " + shortPartnerName;

            }
        }

        function GetInnerText(node) {
            return (node.textContent || node.innerText || node.text);
        }
    </script>

    <div id="Videos">
        <table class="video-header-section" cellspacing="5px" cellpadding="10px">
            <tr>
                <td class="video-header-logo-cell">
                    <asp:Image ID="PartnerLogoImage" runat="server" CssClass="video-header-logo-image" />
                </td>
                <td runat="server" id="HeaderVideoCell1" class="video-header-thumbnail-cell">
                    <asp:ImageButton ID="Movie1ImageButton" runat="server" CssClass="video-header-thumbnail"
                        Visible="false" /><br />
                    <asp:Label ID="Movie1Label" runat="server" CssClass="video-header-thumbnail-label"
                        Visible="false"></asp:Label>
                </td>
                <td runat="server" id="HeaderVideoCell2" class="video-header-thumbnail-cell">
                    <asp:ImageButton ID="Movie2ImageButton" runat="server" CssClass="video-header-thumbnail"
                        ImageAlign="Middle" Visible="false" /><br />
                    <asp:Label ID="Movie2Label" runat="server" CssClass="video-header-thumbnail-label"
                        Visible="false"></asp:Label>
                </td>
                <td runat="server" id="HeaderVideoCell3" class="video-header-thumbnail-cell">
                    <asp:ImageButton ID="Movie3ImageButton" runat="server" CssClass="video-header-thumbnail"
                        Visible="false" /><br />
                    <asp:Label ID="Movie3Label" runat="server" CssClass="video-header-thumbnail-label"
                        Visible="false"></asp:Label>
                </td>
                <td runat="server" id="HeaderVideoCell4" class="video-header-thumbnail-cell">
                    <asp:ImageButton ID="Movie4ImageButton" runat="server" CssClass="video-header-thumbnail"
                        Visible="false" /><br />
                    <asp:Label ID="Movie4Label" runat="server" CssClass="video-header-thumbnail-label"
                        Visible="false"></asp:Label>
                </td>
                <td runat="server" id="HeaderVideoCell5" class="video-header-thumbnail-cell">
                    <asp:ImageButton ID="Movie5ImageButton" runat="server" CssClass="video-header-thumbnail"
                        Visible="false" /><br />
                    <asp:Label ID="Movie5Label" runat="server" CssClass="video-header-thumbnail-label"
                        Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table class="video-rating-section">
            <tr>
                <td class="video-rating-label-cell">
                    <asp:Label runat="server" ID="RatingLabel" Text="Rate this Video" 
						CssClass="video-rating-label" Height="24px"></asp:Label>
                </td>
                <td class="video-rating-cell">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <cc1:Rating ID="StarRating" runat="server" BehaviorID="RatingBehaviour1" CurrentRating="0"
                                HorizontalAlign="Right" MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="waitingRatingStar"
                                FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" Style="float: left;"
                                OnChanged="StarRating_Changed">
                            </cc1:Rating>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr valign="top">
                <td class="video-player-cell" colspan="2">
                    <asp:Literal ID="VideoPlayerLiteral" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
        <div class="video-subscribe-label-position">
            <asp:Label ID="SubscribeLabel" runat="server" Text="Would you like more information from ....?"
                CssClass="video-subscribe-label" ></asp:Label>
        </div>
        <div class="video-subscribe-yes-position">
            <asp:ImageButton runat="server" ID="SubscribeYesImageButton" ImageUrl="Images/yes_btn.png" />
        </div>
        <div class="video-subscribe-no-position">
            <asp:ImageButton runat="server" ID="SubscribeNoImageButton" ImageUrl="Images/no_btn.png" />
        </div>
        <div class="video-footer-position">
            <table class="video-footer-section">
                <tr>
                    <td>
                        <asp:Image ID="Image16" runat="server" ImageUrl="Images/shim.gif" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%;">
                    </td>
                    <td class="video-suggested-label-cell">
                        <asp:Label runat="server" ID="SuggestedVideosLabel" Text="Check out<br> these videos:"
                            CssClass="video-suggested-label"></asp:Label>
                    </td>
                    <td class="video-footer-thumbnail-cell" id="SuggestedMovie1Cell" runat="server">
                        <asp:ImageButton runat="server" ID="SuggestedMovie1ImageButton" CssClass="video-footer-thumbnail"
                            Visible="false" /><br />
                        <asp:Label ID="SuggestedMovie1Label" runat="server" CssClass="video-footer-thumbnail-label"
                            Visible="false"></asp:Label>
                    </td>
                    <td class="video-footer-thumbnail-cell" id="SuggestedMovie2Cell" runat="server">
                        <asp:ImageButton runat="server" ID="SuggestedMovie2ImageButton" CssClass="video-footer-thumbnail"
                            Visible="false" /><br />
                        <asp:Label ID="SuggestedMovie2Label" runat="server" CssClass="video-footer-thumbnail-label"
                            Visible="false"></asp:Label>
                    </td>
                    <td class="video-footer-thumbnail-cell" id="SuggestedMovie3Cell" runat="server">
                        <asp:ImageButton runat="server" ID="SuggestedMovie3ImageButton" CssClass="video-footer-thumbnail"
                            Visible="false" /><br />
                        <asp:Label ID="SuggestedMovie3Label" runat="server" CssClass="video-footer-thumbnail-label"
                            Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="navigation-control">
            <UserControl:Navigation runat="server" ID="NavigationControl1"></UserControl:Navigation>
        </div>
        <div id="FloatingDimmer" class="video-page-dimmer">
        </div>
        <asp:HiddenField ID="SubscribeHiddenField" runat="server" Value="" />

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

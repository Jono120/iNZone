using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CommonLibrary;
using Olympic.AutoDataLayer;

namespace KioskApplication
{
    public partial class Videos : System.Web.UI.Page
    {
        private const string VIDEO_CELL = @"  
                <div style=""position:absolute; z-index:1; left:140px;"">          
                <object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0""
                    width=""1000px"" height=""630px"" id=""VideoPlayer"">                              
                    <param name=""flashvars"" value=""src=[VIDEO]&path=[SKIN_PATH]"" />         
                    <param name=""movie"" value=""Flash/VideoSkin.swf"" />
                    <param name=""autoStart"" value=""true"" />
                    <param name=""wmode"" value=""opaque"">
                    <param name=""quality"" value=""best"">
                </object> </div>           
            ";
        private const int MAX_NO_OF_SUGGESTED_VIDEOS = 3;

        protected void Page_Load(object sender, EventArgs e)
        {

            //Check user is logged in
            if (Session["ParticipantID"] != null)
            {
                if (!Page.IsPostBack)
                {
                    //Create interaction record
                    LogInteraction();

                    //Create interaction video record
                    LogInteractionVideo();

                    SetupSubscriptionQuestion();

                    SetupSessionVariables();

                    PageSetup();
                }

            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        protected override void OnInit(EventArgs e)
        {

            NavigationControl1.PartnerID = Request.QueryString["PartnerID"];

            SubscribeYesImageButton.Attributes.Add("onmouseover", "src=\"Images/yes_btn_over.png\"");
            SubscribeYesImageButton.Attributes.Add("onmouseout", "src=\"Images/yes_btn.png\"");
            SubscribeNoImageButton.Attributes.Add("onmouseover", "src=\"Images/no_btn_over.png\"");
            SubscribeNoImageButton.Attributes.Add("onmouseout", "src=\"Images/no_btn.png\"");

            string partnerConfigFilePath = Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"]) + "/" + Request.QueryString["PartnerName"] + "/Config.xml";

            SubscribeYesImageButton.OnClientClick = "SubscribeYes_OnClick(\"" + Helper.GetPartnerNameForSubscribeYes(partnerConfigFilePath) + "\"); return false;";
            SubscribeNoImageButton.OnClientClick = "SubscribeNo_OnClick(\"" + Helper.GetPartnerNameForSubscribeNo(partnerConfigFilePath) + "\");return false;";

            base.OnInit(e);
        }

        private void PageSetup()
        {
            SetupHeader();
            SetupFooter();
        }

        private void SetupFooter()
        {
            //Setup the suggested movie thumbnails at the bottom

            //Find all the interactions of this participant
            SearchFilter participantIDSearchFilter = new SearchFilter("ParticipantID", Session["ParticipantID"]);
            SearchFilterCollection searchFilterCollection = new SearchFilterCollection();

            searchFilterCollection.Add(participantIDSearchFilter);
            List<Interaction> interactions = Interaction.List(searchFilterCollection);

            //List all the partners, ordered by premier partners first
            Helper helper = new Helper();
            List<Partner> partners = helper.GetListOfPartners();

            foreach (Interaction interaction in interactions)
            {
                //Remove the partner from the partners list that the participant has seen
                partners.RemoveAll(delegate(Partner partner) { return partner.ID == interaction.PartnerID; });
            }

            //Select the first 3 partners on the list
            if (partners.Count == 0)
            {
                //Randomly select 3 premier partners, since participant has seen all the partners
                List<Partner> premierpartners = helper.GetListOfPremierPartners();

                //Remove current displayed partner from the list,
                //since we don't want the same one displaying on the suggested videos section
                premierpartners.RemoveAll(
                    delegate(Partner partner) { return partner.ID == Convert.ToInt32(Request.QueryString["PartnerID"]); });

                //There are 3 or less premier partners, 
                //so just put all the premier partners on suggested videos section
                SetupSuggestedMovies(premierpartners);
            }
            else
            {
                //Return the top 3 partners in partners list
                SetupSuggestedMovies(partners);
            }
        }

        private void SetupHeader()
        {
            //Obtain the catergory, partner name and video id to find the partner logo and partner videos
            //Using that information we setup the appropriate controls on the page
            string partnerName = Request.QueryString["PartnerName"];
            int selectedVideoID = Convert.ToInt32(Request.QueryString["VideoID"]);
            string category = Request.QueryString["Cat"];
            string partnerID = Request.QueryString["PartnerID"];

            string videosFolder = ConfigurationManager.AppSettings["VideosFolderName"];

            string thumbnailExtension =
                System.Configuration.ConfigurationManager.AppSettings["MovieThumbnailsFileExtension"];

            Helper helper = new Helper();

            string configFilePath = Server.MapPath("~/" + videosFolder + "/" + partnerName + "/Config.xml");

            //Get a list of valid partner movies (i.e. the movie folders contain all the necessary files)
            List<int> validMovies = helper.GetValidPartnerMovies(partnerName);

            EnableAllMovieThumbnails();

            for (int i = 0; i < validMovies.Count; i++)
            {
                int movieID = validMovies[i];

                switch (i)
                {
                    case 0:
                        Movie1ImageButton.ImageUrl = videosFolder + "/" + partnerName + "/Movie" + movieID + "/Thumbnail" + movieID + thumbnailExtension;
                        Movie1Label.Text = Helper.GetVideoName(configFilePath, movieID);
                        Movie1ImageButton.Visible = true;
                        Movie1Label.Visible = true;

                        if (selectedVideoID == movieID)
                        {
                            Movie1ImageButton.Enabled = false;
                            HeaderVideoCell1.Style["background-image"] = "Images/video_selection_frame.png";
                        }
                        else
                        {
                            Movie1ImageButton.OnClientClick = "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;";
                            HeaderVideoCell1.Attributes.Add("onmouseover", "this.style.backgroundImage='url(Images/video_selection_frame.png)';");
                            HeaderVideoCell1.Attributes.Add("onmouseout", "this.style.backgroundImage='none';");
                            HeaderVideoCell1.Attributes.Add("onclick", "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;");
                        }

                        break;
                    case 1:
                        Movie2ImageButton.ImageUrl = videosFolder + "/" + partnerName + "/Movie" + movieID + "/Thumbnail" + movieID + thumbnailExtension;
                        Movie2Label.Text = Helper.GetVideoName(configFilePath, movieID);
                        Movie2ImageButton.Visible = true;
                        Movie2Label.Visible = true;

                        if (selectedVideoID == movieID)
                        {
                            Movie2ImageButton.Enabled = false;
                            HeaderVideoCell2.Style["background-image"] = "Images/video_selection_frame.png";
                        }
                        else
                        {
                            Movie2ImageButton.OnClientClick = "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;";
                            HeaderVideoCell2.Attributes.Add("onmouseover",
                                                            "this.style.backgroundImage='url(Images/video_selection_frame.png)';");
                            HeaderVideoCell2.Attributes.Add("onmouseout", "this.style.backgroundImage='none';");
                            HeaderVideoCell2.Attributes.Add("onclick", "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;");

                        }
                        break;
                    case 2:
                        Movie3ImageButton.ImageUrl = videosFolder + "/" + partnerName + "/Movie" + movieID + "/Thumbnail" + movieID + thumbnailExtension;
                        Movie3Label.Text = Helper.GetVideoName(configFilePath, movieID);
                        Movie3ImageButton.Visible = true;
                        Movie3Label.Visible = true;
                        if (selectedVideoID == movieID)
                        {
                            Movie3ImageButton.Enabled = false;
                            HeaderVideoCell3.Style["background-image"] = "Images/video_selection_frame.png";
                        }
                        else
                        {
                            Movie3ImageButton.OnClientClick = "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;";
                            HeaderVideoCell3.Attributes.Add("onmouseover",
                                                            "this.style.backgroundImage='url(Images/video_selection_frame.png)';");
                            HeaderVideoCell3.Attributes.Add("onmouseout", "this.style.backgroundImage='none';");
                            HeaderVideoCell3.Attributes.Add("onclick", "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;");

                        }
                        break;
                    case 3:
                        Movie4ImageButton.ImageUrl = videosFolder + "/" + partnerName + "/Movie" + movieID + "/Thumbnail" + movieID + thumbnailExtension;
                        Movie4Label.Text = Helper.GetVideoName(configFilePath, movieID);
                        Movie4ImageButton.Visible = true;
                        Movie4Label.Visible = true;
                        if (selectedVideoID == movieID)
                        {
                            Movie4ImageButton.Enabled = false;
                            HeaderVideoCell4.Style["background-image"] = "Images/video_selection_frame.png";
                        }
                        else
                        {
                            Movie4ImageButton.OnClientClick = "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;";
                            HeaderVideoCell4.Attributes.Add("onmouseover",
                                                            "this.style.backgroundImage='url(Images/video_selection_frame.png)';");
                            HeaderVideoCell4.Attributes.Add("onmouseout", "this.style.backgroundImage='none';");
                            HeaderVideoCell4.Attributes.Add("onclick", "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;");

                        }
                        break;
                    case 4:
                        Movie5ImageButton.ImageUrl = videosFolder + "/" + partnerName + "/Movie" + movieID + "/Thumbnail" + movieID + thumbnailExtension;
                        Movie5Label.Text = Helper.GetVideoName(configFilePath, movieID);
                        Movie5ImageButton.Visible = true;
                        Movie5Label.Visible = true;
                        if (selectedVideoID == movieID)
                        {
                            Movie5ImageButton.Enabled = false;
                            HeaderVideoCell5.Style["background-image"] = "Images/video_selection_frame.png";
                        }
                        else
                        {
                            Movie5ImageButton.OnClientClick = "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;";
                            HeaderVideoCell5.Attributes.Add("onmouseover",
                                                            "this.style.backgroundImage='url(Images/video_selection_frame.png)';");
                            HeaderVideoCell5.Attributes.Add("onmouseout", "this.style.backgroundImage='none';");
                            HeaderVideoCell5.Attributes.Add("onclick", "Redirect('" + partnerName + "','" + category + "','" + partnerID + "','" + movieID + "'); return false;");
                        }
                        break;
                }
            }

            //Setup the logo image
            PartnerLogoImage.ImageUrl = "Videos/" + partnerName + "/BigLogo" +
                                        ConfigurationManager.AppSettings["LogoFileExtension"];

            //Setup the source video for the video player skin
            string videoCell = VIDEO_CELL.Replace("[VIDEO]", "../" + videosFolder + "/" + partnerName + "/Movie" + selectedVideoID + "/Movie" + selectedVideoID + ".flv").Replace("[SKIN_PATH]", "Flash/SkinUnderPlayVol2.swf");


            VideoPlayerLiteral.Text = videoCell;
        }

        private void EnableAllMovieThumbnails()
        {
            Movie1ImageButton.Enabled = true;
            Movie2ImageButton.Enabled = true;
            Movie3ImageButton.Enabled = true;
            Movie4ImageButton.Enabled = true;
            Movie5ImageButton.Enabled = true;
        }

        private void HideSuggestedMovies()
        {
            SuggestedMovie1ImageButton.Visible = false;
            SuggestedMovie2ImageButton.Visible = false;
            SuggestedMovie3ImageButton.Visible = false;

        }

        private void SetupSuggestedMovies(List<Partner> partners)
        {
            HideSuggestedMovies();

            for (int i = 0; i < partners.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        SuggestedMovie1ImageButton.ImageUrl = partners[i].Movie1ThumbnailPath;
                        SuggestedMovie1ImageButton.OnClientClick = "SuggestedMovie_OnClick(" + Session["PartnerID"] + "," + partners[i].ID + ",'" + partners[i].Name + "','" + partners[i].Category + "',1); return false;";
                        SuggestedMovie1Cell.Attributes.Add("onmouseover", "this.style.backgroundImage='url(Images/video_selection_frame_small.png)';");
                        SuggestedMovie1Cell.Attributes.Add("onmouseout", "this.style.backgroundImage='none';");
                        SuggestedMovie1Cell.Attributes.Add("onclick", "SuggestedMovie_OnClick(" + Session["PartnerID"] + "," + partners[i].ID + ",'" + partners[i].Name + "','" + partners[i].Category + "',1); return false;");
                        SuggestedMovie1ImageButton.Visible = true;
                        SuggestedMovie1Label.Text = partners[i].AbbreviatedName;
                        SuggestedMovie1Label.Visible = true;
                        break;
                    case 1:
                        SuggestedMovie2ImageButton.ImageUrl = partners[i].Movie1ThumbnailPath;
                        SuggestedMovie2ImageButton.OnClientClick = "SuggestedMovie_OnClick(" + Session["PartnerID"] + "," + partners[i].ID + ",'" + partners[i].Name + "','" + partners[i].Category + "',1); return false;";
                        SuggestedMovie2Cell.Attributes.Add("onmouseover", "this.style.backgroundImage='url(Images/video_selection_frame_small.png)';");
                        SuggestedMovie2Cell.Attributes.Add("onmouseout", "this.style.backgroundImage='none';");
                        SuggestedMovie2Cell.Attributes.Add("onclick", "SuggestedMovie_OnClick(" + Session["PartnerID"] + "," + partners[i].ID + ",'" + partners[i].Name + "','" + partners[i].Category + "',1); return false;");
                        SuggestedMovie2ImageButton.Visible = true;
                        SuggestedMovie2Label.Text = partners[i].AbbreviatedName;
                        SuggestedMovie2Label.Visible = true;
                        break;
                    case 2:
                        SuggestedMovie3ImageButton.ImageUrl = partners[i].Movie1ThumbnailPath;
                        SuggestedMovie3ImageButton.OnClientClick = "SuggestedMovie_OnClick(" + Session["PartnerID"] + "," + partners[i].ID + ",'" + partners[i].Name + "','" + partners[i].Category + "',1); return false;";
                        SuggestedMovie3Cell.Attributes.Add("onmouseover", "this.style.backgroundImage='url(Images/video_selection_frame_small.png)';");
                        SuggestedMovie3Cell.Attributes.Add("onmouseout", "this.style.backgroundImage='none';");
                        SuggestedMovie3Cell.Attributes.Add("onclick", "SuggestedMovie_OnClick(" + Session["PartnerID"] + "," + partners[i].ID + ",'" + partners[i].Name + "','" + partners[i].Category + "',1); return false;");
                        SuggestedMovie3ImageButton.Visible = true;
                        SuggestedMovie3Label.Text = partners[i].AbbreviatedName;
                        SuggestedMovie3Label.Visible = true;
                        break;
                }

                if (i >= MAX_NO_OF_SUGGESTED_VIDEOS)
                    break;
            }
        }

        /// <summary>
        /// Creates an Interaction record when participant selects a partner or just logged in
        /// </summary>
        private void LogInteraction()
        {
            if (Session["PartnerID"] == null || Convert.ToString(Session["PartnerID"]) != Request.QueryString["PartnerID"])
            {
                //Participant either just logged on or selected a different partner
                //So we create a new interaction record

                Interaction interaction = new Interaction();
                interaction.DateCreated = System.DateTime.Now;
                interaction.ParticipantID = (Guid)Session["ParticipantID"];
                interaction.PartnerID = Convert.ToInt32(Request.QueryString["PartnerID"]);


                //Check if participant answered the subscribe question for this partner before
                //, if so copy the subscription preference 
                SearchFilterCollection searchFilterCollection = new SearchFilterCollection(new SearchFilter("PartnerID", Convert.ToInt32(Request.QueryString["PartnerID"])), new SearchFilter("ParticipantID", Session["ParticipantID"]));

                OrderByClauseCollection orderByClauseCollection = new OrderByClauseCollection();
                orderByClauseCollection.Add(new OrderByClause("DateCreated", OrderType.Descending));

                List<Interaction> previousInteractions = Interaction.List(searchFilterCollection, orderByClauseCollection);

                if (previousInteractions.Count != 0)
                {
                    interaction.Subscribed = previousInteractions[0].Subscribed;
                }
                else
                {
                    interaction.Subscribed = null;
                }

                interaction.Save();

                //Interaction ID
                Session["InteractionID"] = interaction.ID;
            }
        }

        private int GetLatestRating()
        {
            int partnerID = Convert.ToInt32(Request.QueryString["PartnerID"]);
            string participantID = Session["ParticipantID"].ToString();
            int videoID = Convert.ToInt32(Request.QueryString["VideoID"]);

            //Find all interactions of this participant corresponding to the current partner
            SearchFilterCollection searchFilterCollection = new SearchFilterCollection();
            SearchFilter partnerIDSearchFilter = new SearchFilter("PartnerID", partnerID);
            searchFilterCollection.Add(partnerIDSearchFilter);

            SearchFilter participantIDSearchFilter = new SearchFilter("ParticipantID", participantID);
            searchFilterCollection.Add(participantIDSearchFilter);

            OrderByClauseCollection orderByClauseCollection = new OrderByClauseCollection();
            orderByClauseCollection.Add(new OrderByClause("DateCreated", OrderType.Descending));

            List<Interaction> interactions = Interaction.List(searchFilterCollection, orderByClauseCollection);

            foreach (Interaction interaction in interactions)
            {
                //Go through each interaction and find the one that contains InteractionVideo record for the 
                //currently selected video
                searchFilterCollection = new SearchFilterCollection();
                SearchFilter interactionIDSearchFilter = new SearchFilter("InteractionID", interaction.ID);
                searchFilterCollection.Add(interactionIDSearchFilter);

                SearchFilter videoIDSearchFilter = new SearchFilter("VideoID", videoID);
                searchFilterCollection.Add(videoIDSearchFilter);

                orderByClauseCollection = new OrderByClauseCollection();
                orderByClauseCollection.Add(new OrderByClause("DateCreated", OrderType.Descending));

                List<InteractionVideo> interactionVideos = InteractionVideo.List(searchFilterCollection, orderByClauseCollection);
                if (interactionVideos.Count != 0)
                {
                    return interactionVideos[0].VideoRating;
                }
            }

            return 0;
        }

        /// <summary>
        /// Creates an InteractionVideo record
        /// </summary>
        private void LogInteractionVideo()
        {
            if (Session["InteractionVideoID"] == null || (Convert.ToString(Session["VideoID"]) != Request.QueryString["VideoID"] || Convert.ToString(Session["PartnerID"]) != Request.QueryString["PartnerID"] || Convert.ToString(Session["Category"]) != Request.QueryString["Cat"]))
            {
                //Log the InteractonVideo if participant goes to watch a different video
                InteractionVideo interactionVideo = new InteractionVideo();
                interactionVideo.InteractionID = (Guid)Session["InteractionID"];
                interactionVideo.DateCreated = System.DateTime.Now;
                interactionVideo.VideoRating = GetLatestRating();
                interactionVideo.VideoID = Convert.ToInt32(Request.QueryString["VideoID"]);

                string configFilePath =
                    Server.MapPath("~/Videos/" + Request.QueryString["PartnerName"] +
                                   "/Config.xml");

                interactionVideo.VideoName = Helper.GetVideoName(configFilePath, Convert.ToInt32(Request.QueryString["VideoID"]));

                interactionVideo.Save();

                //Stores interaction video ID so that it can be updated when user rate the video
                Session["InteractionVideoID"] = interactionVideo.ID;
            }

            StarRating.CurrentRating = GetLatestRating();
        }

        private void SetupSubscriptionQuestion()
        {
            string partnerConfigFilePath = Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"]) + "/" + Request.QueryString["PartnerName"] + "/Config.xml";
            SubscribeLabel.Text = "Would you like more information from " + Helper.GetPartnerNameForQuestion(partnerConfigFilePath) + "?";

        }

        private void SetupSessionVariables()
        {
            //ID of Partner in database
            Session["PartnerID"] = Request.QueryString["PartnerID"];

            //Name of partner folder
            Session["PartnerName"] = Request.QueryString["PartnerName"];

            //Category of partner
            Session["Category"] = Request.QueryString["Cat"];

            //Video ID
            Session["VideoID"] = Request.QueryString["VideoID"];
        }

        protected void StarRating_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
        {
            InteractionVideo currentInteractionVideo = InteractionVideo.Get("_id", Session["InteractionVideoID"]);

            currentInteractionVideo.VideoRating = Convert.ToInt32(e.Value);

            currentInteractionVideo.Save();
        }
    }
}

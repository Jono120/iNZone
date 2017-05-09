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
	public partial class ScreenSaverNZFilms : System.Web.UI.Page
	{
		private const string VIDEO_CELL = @"  
                <div style=""position:absolute; z-index:1; left:140px;"">          
                <object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0""
                    width=""1000px"" height=""630px"" id=""VideoPlayer"">                              
                    <param name=""flashvars"" value=""src=[VIDEO]&path=[SKIN_PATH]"" />         
                    <param name=""movie"" value=""Flash/VideoSkin.swf"" />
                    <param name=""autoStart"" value=""true"" />
                    <param name=""wmode"" value=""opaque"" />
                    <param name=""quality"" value=""best"" />
					<param name=""loop"" value=""false"" />
                </object> </div>           
            ";
		//private const int MAX_NO_OF_SUGGESTED_VIDEOS = 3;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				SetupSessionVariables();
				PageSetup();
			}
		}

		protected override void OnInit(EventArgs e)
		{
			string partnerConfigFilePath = Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"]) + "/" + "SSNZFilm" + "/Config.xml";
			base.OnInit(e);
		}

		private void PageSetup()
		{
			SetupHeader();
		}

		private void SetupHeader()
		{
			//Obtain the catergory, partner name and video id to find the partner logo and partner videos
			//Using that information we setup the appropriate controls on the page
			string partnerName = "SSNZFilm";
			int selectedVideoID = 1;

			string videosFolder = ConfigurationManager.AppSettings["VideosFolderName"];

			string thumbnailExtension =
				System.Configuration.ConfigurationManager.AppSettings["MovieThumbnailsFileExtension"];

			Helper helper = new Helper();

			string configFilePath = Server.MapPath("~/" + videosFolder + "/" + partnerName + "/Config.xml");
						

			//Setup the logo image
			PartnerLogoImage.ImageUrl = "Videos/" + partnerName + "/BigLogo" +
										ConfigurationManager.AppSettings["LogoFileExtension"];

			//Setup the source video for the video player skin
			string videoCell = VIDEO_CELL.Replace("[VIDEO]", "../" + videosFolder + "/" + partnerName + "/Movie" + selectedVideoID + "/Movie" + selectedVideoID + ".flv").Replace("[SKIN_PATH]", "Flash/SkinUnderPlayVol2.swf");

			//string videoCell = VIDEO_CELL.Replace("[VIDEO]", "../" + videosFolder + "/" + partnerName + "/Movie" + selectedVideoID + "/Movie" + selectedVideoID + ".flv").Replace("[SKIN_PATH]", "Flash/VideosNoControlSkin.swf");


			VideoPlayerLiteral.Text = videoCell;
		}

		private void SetupSessionVariables()
		{
			//ID of Partner in database
			//Session["PartnerID"] = Request.QueryString["PartnerID"];
			Session["PartnerID"] = "993";

			//Name of partner folder
			//Session["PartnerName"] = Request.QueryString["PartnerName"];
			Session["PartnerName"] = "SSNZFilm";

			//Category of partner
			//Session["Category"] = Request.QueryString["Cat"];
			Session["Category"] = "Motivation";

			//Video ID
			//Session["VideoID"] = Request.QueryString["VideoID"];
			Session["VideoID"] = "1";
		}
	}
}

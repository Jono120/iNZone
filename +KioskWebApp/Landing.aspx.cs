using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;

namespace KioskApplication
{
    public partial class Landing : System.Web.UI.Page
    {
        private const int MAX_NO_OF_VIDEOS = 20;

        //Template for the tag that puts in the video thumbnail
        private const string VIDEO_CELL = @"
            
                <img src=""[LOGO]"" width=""120px"" height=""55px"" />   <br>         
                <object classid=""clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0""
                    width=""112"" height=""80"">
                    <param name=""flashvars"" value=""src=[VIDEO]&videoPage=[VIDEO_PAGE]&param1=[PARAM1]&param2=[PARAM2]&param3=[PARAM3]&param4=[PARAM4]"" />                   
                    <param name=""movie"" value=""Flash/VideosNoControlSkin.swf"" />
                    <param name=""autoStart"" value=""true"" />
                </object>            
            ";

        private const string JOBSEARCH_CELL = @"
            
                <img src=""[LOGO]"" width=""[WIDTH]px"" height=""[HEIGHT]px"" />   <br>    ";

        //Template for the table that's going to align everything nicely
        private const string VIDEO_TABLE = @"

            <tr>
                <td class=""landing-video-cell"" [ONCLICK1] [ONMOUSEOVER1] [ONMOUSEOUT1]>
                    [VIDEO_CELL1]
                </td>
                <td class=""landing-video-cell"" [ONCLICK2] [ONMOUSEOVER2] [ONMOUSEOUT2]>
                    [VIDEO_CELL2]
                </td>
                <td class=""landing-video-cell"" [ONCLICK3] [ONMOUSEOVER3] [ONMOUSEOUT3]>
                    [VIDEO_CELL3]
                </td>
                <td class=""landing-video-cell"" [ONCLICK4] [ONMOUSEOVER4] [ONMOUSEOUT4]>
                    [VIDEO_CELL4]
                </td>
                <td class=""landing-video-cell"" [ONCLICK5] [ONMOUSEOVER5] [ONMOUSEOUT5]>
                    [VIDEO_CELL5]
                </td>
           </tr>
           <tr>
                <td class=""landing-video-cell"" [ONCLICK6] [ONMOUSEOVER6] [ONMOUSEOUT6]>
                    [VIDEO_CELL6]
                </td>
                <td class=""landing-video-cell"" [ONCLICK7] [ONMOUSEOVER7] [ONMOUSEOUT7]>
                    [VIDEO_CELL7]
                </td>
                <td class=""landing-video-cell"" [ONCLICK8] [ONMOUSEOVER8] [ONMOUSEOUT8]>
                    [VIDEO_CELL8]
                </td>
                <td class=""landing-video-cell"" [ONCLICK9] [ONMOUSEOVER9] [ONMOUSEOUT9]>
                    [VIDEO_CELL9]
                </td>
                <td class=""landing-video-cell"" [ONCLICK10] [ONMOUSEOVER10] [ONMOUSEOUT10]>
                    [VIDEO_CELL10]
                </td>
           </tr>
           <tr>
                <td class=""landing-video-cell"" [ONCLICK11] [ONMOUSEOVER11] [ONMOUSEOUT11]>
                    [VIDEO_CELL11]
                </td>
                <td class=""landing-video-cell"" [ONCLICK12] [ONMOUSEOVER12] [ONMOUSEOUT12]>
                    [VIDEO_CELL12]
                </td>
                <td class=""landing-video-cell"" [ONCLICK13] [ONMOUSEOVER13] [ONMOUSEOUT13]>
                    [VIDEO_CELL13]
                </td>
                <td class=""landing-video-cell"" [ONCLICK14] [ONMOUSEOVER14] [ONMOUSEOUT14]>
                    [VIDEO_CELL14]
                </td>
                <td class=""landing-video-cell"" [ONCLICK15] [ONMOUSEOVER15] [ONMOUSEOUT15]>
                    [VIDEO_CELL15]
                </td>
           </tr>
           <tr>
                <td class=""landing-video-cell"" [ONCLICK16] [ONMOUSEOVER16] [ONMOUSEOUT16]>
                    [VIDEO_CELL16]
                </td>
                <td class=""landing-video-cell"" [ONCLICK17] [ONMOUSEOVER17] [ONMOUSEOUT17]>
                    [VIDEO_CELL17]
                </td>
                <td class=""landing-video-cell"" [ONCLICK18] [ONMOUSEOVER18] [ONMOUSEOUT18]>
                    [VIDEO_CELL18]
                </td>
                <td class=""landing-video-cell"" [ONCLICK19] [ONMOUSEOVER19] [ONMOUSEOUT19]>
                    [VIDEO_CELL19]
                </td>
                <td class=""landing-video-cell"" [ONCLICK20] [ONMOUSEOVER20] [ONMOUSEOUT20]>
                    [VIDEO_CELL20]
                </td>
           </tr> 
        ";

        //Template for javascript that's executed when the video cell is clicked
        private const string ONCLICK_SCRIPT = @" onclick=""window.location = '[PAGE]';"" ";

        //Template for javascript that swaps the image when mouse over
        private const string ONMOUSEOVER_SCRIPT = @" onmouseover=""this.style.backgroundImage='url(Images/selected_cell.png)';"" ";

        //Template for javascript that swaps the image when mouse out
        private const string ONMOUSEOUT_SCRIPT = @" onmouseout=""this.style.backgroundImage='none';"" ";


        protected void Page_Load(object sender, EventArgs e)
        {
            //Check user is logged in
            if (Session["ParticipantID"] != null)
            {
                if (!Page.IsPostBack)
                {
                    SetupMenus();

                    SetupActiveViewIndex();
                }

            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);


            SetupMultiView();
        }

        private void SetupActiveViewIndex()
        {
            LandingMultiView.ActiveViewIndex = Convert.ToInt32(Session["SelectedTab"]);

            TabMenu.Items[LandingMultiView.ActiveViewIndex].Selected = true;

            KioskVideos kioskVideos = Helper.GetKioskVideos(Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/CategoriesConfig.xml"));

            //Set the appropriate image
            int i = 0;
            for (i = 0; i < kioskVideos.Category.Count; i++)
            {
                if (TabMenu.Items[i].Selected)
                {
                    TabMenu.Items[i].ImageUrl = "~/Images/" + kioskVideos.Category[i].SelectedTabImageName;
                }
                else
                {
                    TabMenu.Items[i].ImageUrl = "~/Images/" + kioskVideos.Category[i].UnselectedTabImageName;
                }
            }
            if (TabMenu.Items[i].Selected)
            {
                TabMenu.Items[i].ImageUrl = "Images/tab_jobnow_over.png";
            }
            else
            {
                TabMenu.Items[i].ImageUrl = "Images/tab_jobnow.png";
            }
        }

        private void SetupMenus()
        {
            if (TabMenu.Items.Count == 0)
            {
                KioskVideos kioskVideos = Helper.GetKioskVideos(Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/CategoriesConfig.xml"));
                int i = 0;
                if (kioskVideos != null)
                {
                    
                    foreach (Category category in kioskVideos.Category)
                    {
                        MenuItem menuItem = new MenuItem("", i.ToString());
                        menuItem.ImageUrl = "Images/" + category.UnselectedTabImageName;
                        TabMenu.Items.Add(menuItem);
                        TabMenu.CssClass = "landing-videos-tab ";
                        i++;
                    }
                }

                MenuItem jobSearchItem = new MenuItem("", i.ToString());
                jobSearchItem.ImageUrl = "Images/tab_jobnow.png";
                TabMenu.Items.Add(jobSearchItem);

            }
        }

        private void SetupMultiView()
        {
            if (Session["LandingPageVideosOrder"] == null)
            {
                Session["LandingPageVideosOrder"] = Helper.GenerateRandomVideosOrder(Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/CategoriesConfig.xml"));
            }

            KioskVideos kioskVideos = Helper.GetKioskVideos(Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/CategoriesConfig.xml"));

            string videosPathAtServer = Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"]);

            Helper helper = new Helper();

            if (kioskVideos != null && LandingMultiView.Views.Count != kioskVideos.Category.Count())
            {
                string videosOrder = Session["LandingPageVideosOrder"].ToString();

                string[] videosOrdersInEachCat = videosOrder.Split('|');

                for (int i = 0; i < videosOrdersInEachCat.Length - 1; i++)
                {
                    string[] videosOrderInEachCat = videosOrdersInEachCat[i].Split(',');


                    View view = new View();

                    Literal literal = new Literal();

                    literal.Text = @"<table class=""landing-video-page"">
                                            
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    [VIDEOS]
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>";

                    string categoryName = kioskVideos.Category[i].Name.Trim();

                    int videoIndex = 0;

                    //Go through each Partner folder
                    string htmlForCurrentCategory = VIDEO_TABLE;

                    for (int m = 0; m < videosOrderInEachCat.Length; m++)
                    {
                        Customer customer = kioskVideos.Category[i].Partner[Convert.ToInt32(videosOrderInEachCat[m])];

                        //Check if all files exists in Partner Movie1 Folder. Only display the partner if all necessary files are there.
                        if (helper.CheckRequiredMasterFilesExist(customer.Name))
                        {
                            videoIndex++;

                            string partnerConfigFilePath = videosPathAtServer + "/" + customer.Name + "/Config.xml";
                            string partnerLogoPath = ConfigurationManager.AppSettings["VideosFolderName"] + "/" + customer.Name + "/SmallLogo" + ConfigurationManager.AppSettings["LogoFileExtension"];

                            //This path should be relative to the skin swf file, otherwise it won't show if you deploy it on the server
                            string partnerMasterThumbMoviePath = "../" + ConfigurationManager.AppSettings["VideosFolderName"] + "/" + customer.Name + "/Movie1/ThumbMovie1.flv";

                            //ID of partner
                            int partnerID = Helper.GetPartnerID(partnerConfigFilePath);

                            string htmlForVideo = VIDEO_CELL.Replace("[LOGO]", partnerLogoPath).Replace("[VIDEO]", partnerMasterThumbMoviePath).Replace("[VIDEO_PAGE]", "Videos.aspx").Replace("[PARAM1]", "PartnerName=" + customer.Name).Replace("[PARAM2]", "Cat=" + categoryName).Replace("[PARAM3]", "PartnerID=" + partnerID).Replace("[PARAM4]", "VideoID=1");

                            string onClickScript = ONCLICK_SCRIPT.Replace("[PAGE]",
                                                                          "Videos.aspx?PartnerName=" + customer.Name + "&Cat=" +
                                                                          categoryName + "&PartnerID=" + partnerID + "&VideoID=1");

                            //Build the html for the literal control
                            htmlForCurrentCategory =
                                htmlForCurrentCategory.Replace("[VIDEO_CELL" + videoIndex + "]", htmlForVideo).Replace(
                                    "[ONCLICK" + videoIndex + "]", onClickScript).Replace("[ONMOUSEOUT" + videoIndex + "]",
                                                                                          ONMOUSEOUT_SCRIPT).Replace(
                                    "[ONMOUSEOVER" + videoIndex + "]", ONMOUSEOVER_SCRIPT);
                        }
                    }

                    for (int k = videoIndex + 1; k <= MAX_NO_OF_VIDEOS; k++)
                    {
                        htmlForCurrentCategory = htmlForCurrentCategory.Replace("[ONCLICK" + k + "]", "").Replace("[ONMOUSEOUT" + k + "]", "").Replace("[ONMOUSEOVER" + k + "]", "").Replace("[VIDEO_CELL" + k + "]", @"&nbsp;");
                    }

                    literal.Text = literal.Text.Replace("[VIDEOS]", htmlForCurrentCategory);

                    view.Controls.Add(literal);

                    LandingMultiView.Views.Add(view);
                }
            }

            LandingMultiView.Views.Add(CreateJobSearchView());
        }

        /// <summary>
        /// Handles event when the tab is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TabMenu_MenuItemClicked(object sender, MenuEventArgs e)
        {
            if (TabMenu.Items.Count != 1)
            {
                //Set active view index according to which tab is selected
                LandingMultiView.ActiveViewIndex = Convert.ToInt32(e.Item.Value);

                //Select the tab menu
                TabMenu.Items[LandingMultiView.ActiveViewIndex].Selected = true;

                Session["SelectedTab"] = LandingMultiView.ActiveViewIndex;

                KioskVideos kioskVideos = Helper.GetKioskVideos(Server.MapPath("~/" + ConfigurationManager.AppSettings["VideosFolderName"] + "/CategoriesConfig.xml"));

                //Set the appropriate image
                int i = 0;
                for (i = 0; i < kioskVideos.Category.Count; i++)
                {
                    if (TabMenu.Items[i].Selected)
                    {
                        TabMenu.Items[i].ImageUrl = "~/Images/" + kioskVideos.Category[i].SelectedTabImageName;
                    }
                    else
                    {
                        TabMenu.Items[i].ImageUrl = "~/Images/" + kioskVideos.Category[i].UnselectedTabImageName;
                    }
                }
                if (TabMenu.Items[i].Selected)
                {
                    TabMenu.Items[i].ImageUrl = "Images/tab_jobnow_over.png";
                }
                else
                {
                    TabMenu.Items[i].ImageUrl = "Images/tab_jobnow.png";
                }
            }
        }

        private View CreateJobSearchView()
        {
            View jobSearchView = new View();
            Literal l = new Literal();
            l.Text = @"<table class=""landing-video-page"">
                                            
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    [JOBSEARCH]
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>";
            string tableStructure = VIDEO_TABLE;

            int index = 1;
            JobSearchServiceProviders providers = Helper.GetJobSearchServiceProviders(Server.MapPath("~/" + ConfigurationManager.AppSettings["JobSearchProvidersFolderName"] + "/JobSearchServiceProviders.xml"));
            while (index <= providers.Provider.Count)
            {
                string htmlForJobSearch = JOBSEARCH_CELL.Replace("[LOGO]", "Images/" + providers.Provider[index - 1].ImageName).Replace("[WIDTH]", providers.Provider[index - 1].ImageWidth).Replace("[HEIGHT]", providers.Provider[index - 1].ImageHeight);
                string jobSearchOnClickScript = ONCLICK_SCRIPT.Replace("[PAGE]", "JobSearch.aspx?Provider="+ index);
                tableStructure =
                                    tableStructure.Replace("[VIDEO_CELL" + index + "]", htmlForJobSearch).Replace(
                                        "[ONCLICK" + index + "]", jobSearchOnClickScript).Replace("[ONMOUSEOUT" + index + "]", ONMOUSEOUT_SCRIPT).Replace(
                                        "[ONMOUSEOVER" + index + "]", ONMOUSEOVER_SCRIPT);
                index++;
            }
            l.Text = l.Text.Replace("[JOBSEARCH]", tableStructure);
            jobSearchView.Controls.Add(l);
            return jobSearchView;
        }
    }
}

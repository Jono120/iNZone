using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonLibrary;
using Olympic.AutoDataLayer;

namespace KioskApplication
{
    public partial class AjaxEngine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request.QueryString["Action"];


            switch (action)
            {
                case "Subscribe":
                    Subscribe();
                    break;
                case "Check":
                    CheckSubscription();
                    break;
                case "CheckForNavigation":
                    CheckForNavigation();
                    break;
                case "LoadSecurityQuestion":
                    LoadSecurityQuestion();
                    break;
            }


        }

        private void Subscribe()
        {
            bool subscribe = Convert.ToBoolean(Request.QueryString["Subscribe"]);

            //Get the current interaction ID and save whether the participant choose to subscribe 
            Interaction currentInteraction = Interaction.Get("_id", Session["InteractionID"]);

            currentInteraction.Subscribed = subscribe;

            currentInteraction.Save();

            if (Session["RedirectTo"] != null && (Session["RedirectTo"].ToString().Contains("Landing.aspx") || Session["RedirectTo"].ToString().Contains("MyDetails.aspx") || Session["RedirectTo"].ToString().Contains("Login.aspx") || Session["RedirectTo"].ToString().Contains("ForgotPassword.aspx")))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                sb.Append("<Subscribe>");
                sb.Append("<Saved>True</Saved>");
                sb.Append("<ToPage>" + Session["RedirectTo"] + "</ToPage>");
                sb.Append("<SubscribeOption>" + subscribe + "</SubscribeOption>");
                sb.Append("<ShortPartnerName>" + Request.QueryString["ShortPartnerName"] + "</ShortPartnerName>");
                sb.Append("</Subscribe>");
                Response.ContentType = "text/xml";
                Response.Write(sb.ToString());
            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                sb.Append("<Subscribe>");
                sb.Append("<Saved>True</Saved>");
                sb.Append("<ToPage>None</ToPage>");
                sb.Append("<PartnerID>" + Session["JavascriptPartnerID"] + "</PartnerID>");
                sb.Append("<PartnerName>" + Session["JavascriptPartnerName"] + "</PartnerName>");
                sb.Append("<Cat>" + Session["JavascriptCategory"] + "</Cat>");
                sb.Append("<VideoID>" + Session["JavascriptVideoID"] + "</VideoID>");
                sb.Append("<SubscribeOption>" + subscribe + "</SubscribeOption>");
                sb.Append("<ShortPartnerName>" + Request.QueryString["ShortPartnerName"] + "</ShortPartnerName>");
                sb.Append("</Subscribe>");
                Response.ContentType = "text/xml";
                Response.Write(sb.ToString());
            }
        }

        private void CheckSubscription()
        {
            int currentPartnerID = Convert.ToInt32(Request.QueryString["CurrentPartnerID"]);
            int partnerID = Convert.ToInt32(Request.QueryString["PartnerID"]);
            string category = Request.QueryString["Cat"];
            int videoID = Convert.ToInt32(Request.QueryString["VideoID"]);
            string partnerName = Request.QueryString["PartnerName"];
            string toPage = Request.QueryString["ToPage"];

            Session["RedirectTo"] = "Videos.aspx?PartnerID=" + partnerID + "&Cat=" + category + "&VideoID=" + videoID + "&PartnerName=" + partnerName;

            Session["JavascriptPartnerID"] = partnerID;
            Session["JavascriptCategory"] = category;
            Session["JavascriptPartnerName"] = partnerName;
            Session["JavascriptVideoID"] = videoID;

            //Find the latest interaction of this participant that correspond to the partner
            //Then check if there one that has Subscribed = True or Subscribed = False

            SearchFilterCollection searchFilterCollection = new SearchFilterCollection(new SearchFilter("PartnerID", currentPartnerID), new SearchFilter("ParticipantID", Session["ParticipantID"]));

            OrderByClauseCollection orderByClauseCollection = new OrderByClauseCollection();
            orderByClauseCollection.Add(new OrderByClause("DateCreated", OrderType.Descending));

            List<Interaction> previousInteractions = Interaction.List(searchFilterCollection, orderByClauseCollection);

            if (previousInteractions.Count != 0)
            {
                if (previousInteractions[0].Subscribed != null)
                {
                    //Response.Redirect("Videos.aspx?Cat=" + category + "&PartnerID=" + partnerID + "&PartnerName=" + partnerName + "&VideoID=" + videoID);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                    sb.Append("<CheckSubscription>");
                    sb.Append("<PartnerID>" + partnerID + "</PartnerID>");
                    sb.Append("<PartnerName>" + partnerName + "</PartnerName>");
                    sb.Append("<Cat>" + category + "</Cat>");
                    sb.Append("<VideoID>" + videoID + "</VideoID>");
                    sb.Append("<Answered>True</Answered>");
                    sb.Append("</CheckSubscription>");
                    Response.ContentType = "text/xml";
                    Response.Write(sb.ToString());

                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                    sb.Append("<CheckSubscription>");
                    sb.Append("<Answered>False</Answered>");
                    sb.Append("</CheckSubscription>");
                    Response.ContentType = "text/xml";
                    Response.Write(sb.ToString());
                }
            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                sb.Append("<CheckSubscription>");
                sb.Append("<Answered>False</Answered>");
                sb.Append("</CheckSubscription>");
                Response.ContentType = "text/xml";
                Response.Write(sb.ToString());
            }
        }

        private void CheckForNavigation()
        {
            int currentPartnerID = Convert.ToInt32(Request.QueryString["CurrentPartnerID"]);
            string toPage = Request.QueryString["ToPage"];

            Session["RedirectTo"] = toPage;

            //Find the latest interaction of this participant that correspond to the partner
            //Then check if there one that has Subscribed = True or Subscribed = False

            SearchFilterCollection searchFilterCollection = new SearchFilterCollection(new SearchFilter("PartnerID", currentPartnerID), new SearchFilter("ParticipantID", Session["ParticipantID"]));

            OrderByClauseCollection orderByClauseCollection = new OrderByClauseCollection();
            orderByClauseCollection.Add(new OrderByClause("DateCreated", OrderType.Descending));

            List<Interaction> previousInteractions = Interaction.List(searchFilterCollection, orderByClauseCollection);

            if (previousInteractions.Count != 0)
            {
                if (previousInteractions[0].Subscribed != null)
                {
                    //Response.Redirect("Videos.aspx?Cat=" + category + "&PartnerID=" + partnerID + "&PartnerName=" + partnerName + "&VideoID=" + videoID);
                    System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                    sb.Append("<CheckSubscription>");
                    sb.Append("<ToPage>" + toPage + "</ToPage>");
                    sb.Append("<Answered>True</Answered>");
                    sb.Append("</CheckSubscription>");
                    Response.ContentType = "text/xml";
                    Response.Write(sb.ToString());

                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                    sb.Append("<CheckSubscription>");
                    sb.Append("<Answered>False</Answered>");
                    sb.Append("</CheckSubscription>");
                    Response.ContentType = "text/xml";
                    Response.Write(sb.ToString());
                }
            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                sb.Append("<CheckSubscription>");
                sb.Append("<Answered>False</Answered>");
                sb.Append("</CheckSubscription>");
                Response.ContentType = "text/xml";
                Response.Write(sb.ToString());
            }
        }

        private void LoadSecurityQuestion()
        {
            string username = Request.QueryString["Username"];

            SearchFilter userNameSearchFilter = new SearchFilter("UserName", username.Trim());
            SearchFilterCollection searchFilterCollection = new SearchFilterCollection();
            searchFilterCollection.Add(userNameSearchFilter);

            List<Participant> participant = Participant.List(searchFilterCollection);

            if(participant.Count != 1)
            {
                //Username is incorrect
                System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                sb.Append("<LoadSecurityQuestion>");
                sb.Append("<UsernameOK>False</UsernameOK>");
                sb.Append("</LoadSecurityQuestion>");
                Response.ContentType = "text/xml";
                Response.Write(sb.ToString());

            }
            else
            {
                //Username is found, send back the security question
                System.Text.StringBuilder sb = new System.Text.StringBuilder("<?xml version=\"1.0\"?>");
                sb.Append("<LoadSecurityQuestion>");
                sb.Append("<UsernameOK>True</UsernameOK>");
                sb.Append("<SecurityQuestion>"+participant[0].SecurityQuestion+"</SecurityQuestion>");
                sb.Append("</LoadSecurityQuestion>");
                Response.ContentType = "text/xml";
                Response.Write(sb.ToString());
            }
        }
    }
}

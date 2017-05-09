<%@ WebHandler Language="C#" Class="redirect" %>
using System;
using System.Web;
using System.Linq;
using System.Data.Linq;

public class redirect : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {


        // make sure QueryString is OK
        if (context.Request.QueryString.Count == 0)
        {
            showMessage(context, "(InZone Careers Kiosk) We're were unable to process your request due to insufficient request information. (1)");
            return;
        }
        if (context.Request.QueryString[0] == null)
        {
            showMessage(context, "(InZone Careers Kiosk) We're were unable to process your request due to insufficient request information. (2)");
            return;
        }

        // try and parse the querystring
        int mailoutId;
        try
        {
            string temp = Webstream.UrlEncoding.Base64UrlDecode(
                context.Request.QueryString[0]);
            mailoutId = Convert.ToInt32(temp);
        }
        catch
        {
            showMessage(context, "(InZone Careers Kiosk) We're were unable to process your request due to insufficient request information. (3)");
            return;
        }

        // connect to db and get mailout record
        Inzone.MailService.Kiosk.InZoneKioskDAL db = null;
        Inzone.MailService.Kiosk.Mailout m;
        try
        {
            db = new Inzone.MailService.Kiosk.InZoneKioskDAL();
            m = db.Mailouts.First(mo => mo.ID == mailoutId);
        }
        catch (Exception dbEx)
        {
            if (db != null) db.Dispose();
            showMessage(context, "(InZone Careers Kiosk) Sorry, we could not redirect you. Database error (1): " + dbEx.Message + ", " + dbEx.StackTrace);
            return;
        }

        // we have the mailout record, so we get the redirect URL
        string redirectUrl = m.Partner.WebsiteURL;
        // update the mailout to say the participant has
        // click the redirect link (has responded basically)
        if (m.DateResponded == null)
        {
            m.DateResponded = DateTime.Now;
            db.SubmitChanges(ConflictMode.FailOnFirstConflict);
        }
        db.Dispose();

        // finally, redirect them...
        context.Response.Redirect(redirectUrl);

    }

    private void showMessage(HttpContext context, string message)
    {
        context.Response.Write(message);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
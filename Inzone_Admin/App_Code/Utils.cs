using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Inzone {
	public class Utils {
		public static string GetAppSetting(string key) { return ConfigurationManager.AppSettings[key]; }
		public static void writeToErrorLog(string text, string filename) {
			try {
				System.IO.File.AppendAllText(filename, DateTime.Now.ToString("ddd dd-MMM-yyyy hh:mm:ss") + " - " + text + "\r\n");
			} catch (Exception ex) {
				Debug.Print("Couldn't write to error log: " + ex.Message);
			}
		}
		public static void writeToErrorLog(string text) {
			writeToErrorLog(text, "C:\\InZone_Backend_Errors\\errors.txt");
		}
	}
}
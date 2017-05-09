using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;

using Inzone;
using Webstream;

/// <summary>
/// Summary description for Syncronisation
/// </summary>
[WebService(Namespace = "InZone.DataZone")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Syncronisation: System.Web.Services.WebService {

	public Syncronisation() {
	}

	[WebMethod]
	public int SyncCustomers(DataSet customers, out int[] badIds, out int[] goodIds, string password) {
		if (password != UrlEncoding.HashBase64UrlEncode(Utils.GetAppSetting("WebServicePassword"))) {
			// authentication failed
			badIds = new int[1] { -1 };
			goodIds = new int[1] { -1 };
			return 0;
		}
		InzoneData data = new InzoneData("InZoneMirrorConnectionString");
		return data.SyncCustomers(customers, out badIds, out goodIds);
		data.Kill();
	}
	[WebMethod]
	public int SyncEvents(DataSet events, out int[] badIds, out int[] goodIds, string password) {
		if (password != UrlEncoding.HashBase64UrlEncode(Utils.GetAppSetting("WebServicePassword"))) {
			// authentication failed
			badIds = new int[1] { -1 };
			goodIds = new int[1] { -1 };
			return 0;
		}
		InzoneData data = new InzoneData("InZoneMirrorConnectionString");
		return data.SyncEvents(events, out badIds, out goodIds);
		data.Kill();
	}
	[WebMethod]
	public int SyncParticipants(DataSet participants, out int[] badIds, out int[] goodIds, string password) {
		if (password != UrlEncoding.HashBase64UrlEncode(Utils.GetAppSetting("WebServicePassword"))) {
			// authentication failed
			badIds = new int[1] { -1 };
			goodIds = new int[1] { -1 };
			return 0;
		}
		InzoneData data = new InzoneData("InZoneMirrorConnectionString");
		return data.SyncParticipants(participants, out badIds, out goodIds);
		data.Kill();
	}
	[WebMethod]
	public int SyncInteractions(DataSet interactions, out int[] badIds, out int[] goodIds, string password) {
		if (password != UrlEncoding.HashBase64UrlEncode(Utils.GetAppSetting("WebServicePassword"))) {
			// authentication failed
			badIds = new int[1] { -1 };
			goodIds = new int[1] { -1 };
			return 0;
		}
		InzoneData data = new InzoneData("InZoneMirrorConnectionString");
		return data.SyncInteractions(interactions, out badIds, out goodIds);
		data.Kill();
	}

}


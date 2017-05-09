using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.Diagnostics;

using System.Xml;
using Inzone;

namespace Inzone {
	/// <summary>
	/// Summary description for Participant
	/// </summary>
	[WebService(Namespace = "")]
	//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class Participant: System.Web.Services.WebService {
		public Participant() {
		}

		/// <summary>
		/// Participant Login
		/// </summary>
		/// <param name="xmlString">XML data containing the registration code and ID of the pod station</param>
		/// <returns>XML data containing participant ID, Firstname, Lastname</returns>
		[WebMethod]
		public XmlDocument LogIn(string xmlString) {
			Debug.Print("Login called, XML = " + xmlString);

			InzoneData data = new InzoneData("InZoneConnectionString");
			XmlDocument returnData = new XmlDocument();
			try {
				// parse XML for relevant info
				XmlDocument xmlDoc = getXmlDocument(xmlString);
				string registrationCode = getParticipantXmlFieldValue(xmlDoc, "RegistrationCode");
				string podId = getParticipantXmlFieldValue(xmlDoc, "PodID");
				// attempt to log the user in
				int participantId = data.ParticipantLogIn(Convert.ToInt32(registrationCode), Convert.ToInt32(podId));
				if (participantId > 0) {	// no errors
					Debug.Print("\tLogin successful");
					// with newly aqquired participant ID from login process,
					// attempt to get info about the participant
					ParticipantInfo pi = new ParticipantInfo();
					if (!data.ParticipantGetInfo(participantId, out pi)) {
						Debug.Print("\tLogin failed to get participant info");
						// we have to error out here since receiving
						// application expects a certain amount of data
						Utils.writeToErrorLog("Webservice.Login(): Unable to retreive participant data (this should never happen!) XML: " + xmlString);
						returnData.LoadXml(getXmlErrorString("Unable to retreive participant data (this should never happen!)"));
						return returnData;
					} else {
						// register the interaction
						int interactionResult = data.InteractionInsert(pi.ID, Convert.ToInt32(podId));
						if (interactionResult == 0) {
							Debug.Print("\tLogin failed to insert an Interaction");
							// if this call failed, it's likely that there is no current Event
							Utils.writeToErrorLog("Webservice.Login(): Could not register interaction (No current event?) XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("Could not register interaction (No current event?)"));
							return returnData;
						} else {
							string xml =
									"<RegisteredParticipant>" +
									"<RegistrationCode>" + pi.RegistrationCode.ToString() + "</RegistrationCode>" +
									"<ParticipantRegistrationID>" + pi.ID.ToString() + "</ParticipantRegistrationID>" +
									"<FirstName>" + pi.Firstname + "</FirstName>" +
									"<LastName>" + pi.Lastname + "</LastName>" +
									"<LoggedIn>" + (pi.LoggedIn ? "1" : "0") + "</LoggedIn>" +
									"<LastPodNo>" + pi.LastCustomerID.ToString() + "</LastPodNo>" +
									"</RegisteredParticipant>";
							Debug.Print("\tSending data back to client: " + xml);
							// send back participant data
							returnData.LoadXml(xml);
							return returnData;
						}
					}
				} else {
					// handle errors
					switch (participantId) {
						case 0:
							Debug.Print("\tWebservice.Login(): Authentication failed: unspecified reason, occurred in ParticipantLogIn() XML: " + xmlString);
							Utils.writeToErrorLog("Webservice.Login(): Authentication failed: unspecified reason, occurred in ParticipantLogIn() XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("Authentication failed: unspecified reason, occurred in ParticipantLogIn()"));
							break;
						case -1:
							Debug.Print("\tWebservice.Login(): Authentication failed XML: " + xmlString);
							Utils.writeToErrorLog("Webservice.Login(): Authentication failed XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("Authentication failed"));
							break;
						case -2:
							Debug.Print("\tWebservice.Login(): Authentication failed: you are already logged in somewhere else XML: " + xmlString);
							Utils.writeToErrorLog("Webservice.Login(): Authentication failed: you are already logged in somewhere else XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("Authentication failed: you are already logged in somewhere else"));
							break;
						case -3:
							Debug.Print("\tWebservice.Login(): Authentication failed: access denied from your client XML: " + xmlString);
							Utils.writeToErrorLog("Webservice.Login(): Authentication failed: access denied from your client XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("Authentication failed: access denied from your client"));
							break;
						default:
							Debug.Print("\tWebservice.Login(): Unexpected return code from query = " + participantId.ToString() + " XML: " + xmlString);
							Utils.writeToErrorLog("Webservice.Login(): Unexpected return code from query = " + participantId.ToString() + " XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("Authentication failed (Unspecified return code from query = " +
									participantId.ToString() + ")"));
							break;
					}
					return returnData;
				}
			} catch (Exception ex) {
				Debug.Print("\tWebservice.Login(): Unspecified error: " + ex.Message + ",  XML: " + xmlString);
				Utils.writeToErrorLog("Webservice.Login(): Unspecified error: " + ex.Message + ",  XML: " + xmlString);
				returnData.LoadXml(getXmlErrorString("Unspecified error: " + ex.Message));
				return returnData;
			} finally {
				data.Kill();
			}
		}

		/// <summary>
		/// Participant Logout
		/// </summary>
		/// <param name="xmlString">XML data containing customer ID and participant ID to logout</param>
		/// <returns>XML data containing the result of the logout proceedure</returns>
		[WebMethod]
		public XmlDocument LogOut(string xmlString) {
			Debug.Print("Logout called, XML = " + xmlString);

			InzoneData data = new InzoneData("InZoneConnectionString");
			XmlDocument returnData = new XmlDocument();
			try {
				// parse incoming XML data
				XmlDocument xmlDoc = getXmlDocument(xmlString);
				string participantId = getParticipantXmlFieldValue(xmlDoc, "RegistrationID");
				string customerId = getParticipantXmlFieldValue(xmlDoc, "PodID");
				string info = getParticipantXmlFieldValue(xmlDoc, "Info");	//used for promo purposes
				// attempt to logout
				bool logout = data.ParticipantLogOut(Convert.ToInt32(participantId), (info.Trim() == "1" ? true : false));
				if (!logout) {
					Debug.Print("\tLogout failed");
					// fail
					Utils.writeToErrorLog("Webservice.Logout(): Failed XML: " + xmlString);
					returnData.LoadXml("<Logout>Failed</Logout>");
					return returnData;
				} else {
					Debug.Print("\tLogout success");
					// success!
					// - updated to set the SendInfo field needed for new mailer system
					if (!data.InteractionSubscribe(Convert.ToInt32(participantId), Convert.ToInt32(customerId), (info.Trim() == "1" ? true : false))) {
						Debug.Print("\tError setting subscribe bit field for participant: " + participantId);
						Utils.writeToErrorLog("Error setting subscribe bit field for participant: " + participantId);
					}
					returnData.LoadXml("<Logout>Success</Logout>");
					return returnData;
				}
			} catch (Exception ex) {
				Debug.Print("\tLogout failed: " + ex.Message);
				returnData.LoadXml("<Logout>Failed</Logout>");
				Utils.writeToErrorLog("Webservice.Logout(): Exception: " + ex.Message + " XML: " + xmlString);
				return returnData;
			} finally {
				data.Kill();
			}
		}

		/// <summary>
		/// Participant registration
		/// </summary>
		/// <param name="xmlString">XML data containing information about the participant</param>
		/// <returns>XML data containing a unique registration code for the participant (used to login later)</returns>
		[WebMethod]
		public XmlDocument RegisterParticipant(string xmlString) {
			Utils.writeToErrorLog("Register called: " + xmlString);
			Debug.Print("Register called, XML = " + xmlString);

			InzoneData data = new InzoneData("InZoneConnectionString");
			XmlDocument returnData = new XmlDocument();
			try {
				// parse XML data
				XmlDocument xmlDoc = getXmlDocument(xmlString);
				string firstname = getParticipantXmlFieldValue(xmlDoc, "Firstname");
				string lastname = getParticipantXmlFieldValue(xmlDoc, "Lastname");
				string email = getParticipantXmlFieldValue(xmlDoc, "Email");
				string address1 = getParticipantXmlFieldValue(xmlDoc, "Address1");
				string address2 = getParticipantXmlFieldValue(xmlDoc, "Address2");
				string suburb = getParticipantXmlFieldValue(xmlDoc, "Suburb");
				string town = getParticipantXmlFieldValue(xmlDoc, "Town");
				string association = getParticipantXmlFieldValue(xmlDoc, "Association");
				string age = getParticipantXmlFieldValue(xmlDoc, "Age");
				string dob = getParticipantXmlFieldValue(xmlDoc, "DOB");		// new date of birth field
				string gender = getParticipantXmlFieldValue(xmlDoc, "Gender");
				string contactPhone = getParticipantXmlFieldValue(xmlDoc, "ContactPhone");
				string careerDecided = getParticipantXmlFieldValue(xmlDoc, "CareerDecided");
				string podId = getParticipantXmlFieldValue(xmlDoc, "PodID");

				// insert participant data - get ID of participant
				int participantId = data.ParticipantInsert(firstname, lastname, email, address1, address2,
						suburb, town, association, age, dob, gender, contactPhone, careerDecided, podId);
				if (participantId > 0) {		// no errors
					Debug.Print("\tRegister - Participant inserted OK");
					// generate registration code
					int registrationCode = data.GenerateRegistrationCodeForParticipant(participantId);
					// register the interaction
					//data.InteractionInsert(participantId, Convert.ToInt32(podId));
					// send XML containing registration code back to client
					string xml =
							"<RegisteredParticipant>" +
							"<RegistrationCode>" + registrationCode.ToString() + "</RegistrationCode>" +
							"<ParticipantRegistrationID>" + participantId.ToString() + "</ParticipantRegistrationID>" +
							"<FirstName>" + firstname + "</FirstName>" +
							"<LastName>" + lastname + "</LastName>" +
							"<LoggedIn>0</LoggedIn>" +
							"<LastPodNo>" + podId + "</LastPodNo>" +
							"</RegisteredParticipant>";
					Debug.Print("\tSending data to client: " + xml);
					returnData.LoadXml(xml);
					return returnData;
				} else {
					// process errors
					switch (participantId) {
						case 0:
							Utils.writeToErrorLog("Webservice.Register(): Unhandled data-layer exception XML: " + xmlString);
							Debug.Print("\tWebservice.Register(): Unhandled data-layer exception XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("Unhandled data-layer exception"));
							break;
						case -1:
							Utils.writeToErrorLog("Webservice.Register(): SQL Error - POD client not registered with system? XML: " + xmlString);
							Debug.Print("\tWebservice.Register(): SQL Error - POD client not registered with system? XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("SQL Error - POD client not registered with system?"));
							break;
						default:
							Utils.writeToErrorLog("Webservice.Register(): Unspecified error (" + participantId.ToString() + ") XML: " + xmlString);
							Debug.Print("\tWebservice.Register(): Unspecified error (" + participantId.ToString() + ") XML: " + xmlString);
							returnData.LoadXml(getXmlErrorString("Unspecified error (" + participantId.ToString() + ")"));
							break;
					}
					return returnData;
				}
			} catch (Exception ex) {
				Utils.writeToErrorLog("Webservice.Register(): Unspecified error (" + ex.Message + ") XML: " + xmlString);
				Debug.Print("\tWebservice.Register(): Unspecified error (" + ex.Message + ") XML: " + xmlString);
				returnData.LoadXml(getXmlErrorString("Unable to process input data from client"));
				return returnData;
			} finally {
				data.Kill();
			}
		}


		private string getXmlErrorString(string str) {
			return "<Error><ErrorMessage>" + str + "</ErrorMessage></Error>";
		}

		private XmlDocument getXmlDocument(string xml) {
			XmlDocument xmlDoc = new XmlDocument();
			xml = xml.Replace("xmlString=", "");
			xmlDoc.LoadXml(xml);
			return xmlDoc;
		}

		private string getParticipantXmlFieldValue(XmlDocument xmlDoc, string fieldName) {
			fieldName = fieldName.ToLower().Trim();
			for (int i = 0; i < xmlDoc.ChildNodes[0].ChildNodes.Count; i++) {
				if (xmlDoc.ChildNodes[0].ChildNodes[i].Name.ToLower().Trim() == fieldName)
					return xmlDoc.ChildNodes[0].ChildNodes[i].InnerText.Trim();
			}
			return String.Empty;
		}

	}

}
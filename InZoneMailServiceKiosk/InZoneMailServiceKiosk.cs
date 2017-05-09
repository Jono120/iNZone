using System;

using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Data.Linq;
using System.Net.Mail;
using InZone;
using System.Threading;
using System.Net;

namespace InZoneKioskMailService {
	public partial class InZoneMailService: ServiceBase {

		InZoneKioskDAL db = null;
		Dictionary<string, XmlDocument> serviceParameters = null;

		public InZoneMailService() {
			InitializeComponent();
		}

		protected override void OnStart(string[] args) {
			try {
				// load settings from app.config
				tmrMain.Interval = Properties.Settings.Default.MailoutCheckFrequency * 1000;

				// create database connection
				try {
                    db = new InZoneKioskDAL(Properties.Settings.Default.KioskMasterConnectionString);
					if (!db.DatabaseExists()) {
						this.EventLog.WriteEntry("db.DatabaseExists() returned false, "
						+ "indicating the connection could not be opened.", EventLogEntryType.Error);
						db = null;
					}
				} catch (Exception dbEx) {
					db = null;
					this.EventLog.WriteEntry("There was an error creating the database connection: "
						+ dbEx.Message, EventLogEntryType.Error);
				}

				// write 
				this.EventLog.WriteEntry(
					"Database Connection: " + (db == null ? "Error" : "OK") + "\r\n\r\n" +
					"Properties loaded from app.config:\r\n\r\n" +
					"Interval between new mailout checks: " + tmrMain.Interval.ToString() + " millisecond(s)\r\n" +
					"Database: " + db.Connection.DataSource.ToLower() + " [" + db.Connection.Database + "]\r\n" +
					"SMTP Host: " + Properties.Settings.Default.SMTPHost + "\r\n" +
					"SMTP Port: " + Properties.Settings.Default.SMTPPort.ToString() + "\r\n" +
					"SMTP Timeout: " + Properties.Settings.Default.SMTPTimeout.ToString() + " seconds\r\n" +
					"Sender name: " + Properties.Settings.Default.SenderName + "\r\n" +
					"Sender address: " + Properties.Settings.Default.SenderAddress + "\r\n" +
					"Default Subject: " + Properties.Settings.Default.DefaultSubject + "\r\n" +
					"Time between sends: " + Properties.Settings.Default.TimeBetweenSends.ToString() + " seconds",
					EventLogEntryType.Information);

				// begin the checking timer
				tmrMain.Enabled = true;
			} catch (Exception ex) {
				this.EventLog.WriteEntry(
					"Error during startup: " + ex.Message + "\r\n" +
					"Stack: " + ex.StackTrace, EventLogEntryType.Error);
			}

		}

		protected override void OnStop() {
			tmrMain.Enabled = false;
			if (db != null) db.Dispose();
			this.EventLog.WriteEntry("InZone Mail Service stopped successfully", EventLogEntryType.Information);
		}

		private void tmrMain_Tick(object osender, EventArgs e) {
			try {

				// check if we have some mailout records 
				// that need processing
				int count = (
					from m in db.Mailouts
					where m.DateSent == null
					select m.ID
				).Count();
				if (count == 0) {
					//this.EventLog.WriteEntry("Timer elapsed, no Mailouts to process");
					return;
				}

				/* looks like we have some mailout
					 records to process */
				this.EventLog.WriteEntry("Timer elapsed, found " + count.ToString() + " Mailout(s) to process",
					EventLogEntryType.Information);

				// turn this off before processing...
				tmrMain.Enabled = false;

				// get any mailouts that have not yet been sent
				IQueryable<Mailout> query =
					from m in db.Mailouts
					where m.DateSent == null
					select m;
				int sent = 0;

				// set up SMTP objects
				SmtpClient smtp = new SmtpClient(
					Properties.Settings.Default.SMTPHost,
					Properties.Settings.Default.SMTPPort);
				smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
				smtp.Timeout = Properties.Settings.Default.SMTPTimeout * 1000;
				MailMessage message;
				MailAddress sender, recipient;
				//string messageSubject;



				// process each mailout
				foreach (Mailout mo in query) {
					// sleep
					Thread.Sleep(Properties.Settings.Default.TimeBetweenSends * 1000);

					// for error checking
					bool recipientOk = false, senderOk = false;

					// pull down recipient data
					string rName, rAddress;
					rAddress = mo.Participant.Email;
					rName = mo.Participant.FirstName + " " + mo.Participant.LastName;

					try {

						// check the participant info for bad data
						if (rAddress.Trim().Length == 0) {
							// no email address, log this
							// and move on
							mo.DateSent = DateTime.Now;		// flag as sent so we don't process this again
							logMailoutException(mo.ID, rAddress, "Email address was 0 length");
							continue;
						}

						// get the service parameters (XML), load the required values
						XmlDocument serviceParams = getServiceParameters(mo.ServiceID, mo.PartnerID);
						string subject = getXmlParameterValue(serviceParams, "EmailSubject");
						string bodyHtml = getXmlParameterValue(serviceParams, "EmailTemplate");
						if (bodyHtml != null)
							bodyHtml = System.IO.File.ReadAllText(bodyHtml);
						// load any defaults needed if certain params are left out
						if (subject == null) subject = Properties.Settings.Default.DefaultSubject;
						if (bodyHtml == null) bodyHtml = "<html><body>InZone Mail System Error. Please ignore this message.</body></html>";
						// replace tokens with database field values
						subject = subject.Replace("@CUSTOMER@", mo.Partner.DisplayName);
						subject = subject.Replace("@PARTICIPANT_FIRSTNAME@", mo.Participant.FirstName);
						subject = subject.Replace("@PARTICIPANT_LASTNAME@", mo.Participant.LastName);
						bodyHtml = bodyHtml.Replace("@CUSTOMER@", mo.Partner.DisplayName);
						bodyHtml = bodyHtml.Replace("@REDIRECT@",
							"http://www.inzone.co.nz/mail_Kiosk/redirect?" + Webstream.UrlEncoding.Base64UrlEncode(mo.ID.ToString()));
						bodyHtml = bodyHtml.Replace("@PARTICIPANT_FIRSTNAME@", mo.Participant.FirstName);
						bodyHtml = bodyHtml.Replace("@PARTICIPANT_LASTNAME@", mo.Participant.LastName);
						bodyHtml = bodyHtml.Replace("@UNSUBSCRIBE_LINK@",
							"http://www.inzone.co.nz/mail_Kiosk/unsubscribe?" + Webstream.UrlEncoding.Base64UrlEncode(mo.ID.ToString()));
						bodyHtml = bodyHtml.Replace("@CUSTOMER_LOGO@", mo.Partner.LogoURL);
						bodyHtml = bodyHtml.Replace("@TITLE@", subject);

						// establish senders and recipients - exceptions thrown here for malformed email addresses
						sender = new MailAddress(Properties.Settings.Default.SenderAddress,
							Properties.Settings.Default.SenderName, Encoding.UTF8);
						senderOk = true;
						recipient = new MailAddress(rAddress, rName, Encoding.UTF8);
						recipientOk = true;

						// setup the mail message
						message = new MailMessage();
						message.BodyEncoding = Encoding.UTF8;
						message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
						message.Priority = MailPriority.Normal;
						message.ReplyTo = sender;
						message.From = sender;
						message.IsBodyHtml = true;
						message.Subject = subject;
						message.SubjectEncoding = Encoding.UTF8;
						message.To.Add(recipient);

						// send it!
						message.Body = bodyHtml;
						smtp.Send(message);
						mo.DateSent = DateTime.Now;
						sent++;	//increment send count

					} catch (FormatException) {
						if (!senderOk) {
							this.EventLog.WriteEntry("The sender's address ["
								+ Properties.Settings.Default.SenderAddress + "] is malformed. Processing halted.",
								EventLogEntryType.Error);
							break;
						}
						if (!recipientOk) {
							mo.DateSent = DateTime.Now;	// flag as sent so we don't try again later
							logMailoutException(mo.ID, rAddress, "Recipient address is malformed");
						}
					} catch (SmtpFailedRecipientsException sfrEx) {
						mo.DateSent = DateTime.Now;	// flag as sent
						logMailoutException(mo.ID, rAddress, "Mail could not be delivered: " + sfrEx.Message);
					} catch (SmtpException smtpEx) {
						mo.DateSent = DateTime.Now; // flag as sent
						logMailoutException(mo.ID, rAddress, "SMTP connection failure/timeout: " + smtpEx.Message);
						this.EventLog.WriteEntry("SMTP connection failure/timeout: " + smtpEx.Message, EventLogEntryType.Error);
					} catch (InvalidOperationException iopEx) {
						mo.DateSent = DateTime.Now; // flag as sent
						logMailoutException(mo.ID, rAddress, "Invalid Operation: " + iopEx.Message);
					} catch (Exception ex) {
						mo.DateSent = DateTime.Now;
						logMailoutException(mo.ID, rAddress, "System exception: " + ex.Message);
						this.EventLog.WriteEntry("System exception: " + ex.Message, EventLogEntryType.Error);
					} finally {
						// attempt to process the update straight away
						try {
							db.SubmitChanges(ConflictMode.FailOnFirstConflict);
						} catch (Exception dbUpdateException) {
							// any database update exception is trapped here.
							// we treat this very seriously and after logging
							// the error, we will completely stop everything
							// including the service
							tmrMain.Enabled = false;
							this.EventLog.WriteEntry(
								"An error occurred during db.SubmitChanges() " +
								"[SQL UPDATE]: " + dbUpdateException.Message +
								".\r\n\r\nThe entire process has been halted, " +
								"and the service stopped. This is to prevent " +
								"customers being spammed.", EventLogEntryType.Error);
							this.Stop();
						} 
					}
				}

				// processing finished!
				smtp = null;
				serviceParameters.Clear();
				db.Dispose();
				db = new InZoneKioskDAL(Properties.Settings.Default.KioskMasterConnectionString);
				tmrMain.Enabled = true;
				this.EventLog.WriteEntry("All Mailouts processed\r\nTotal: "
					+ count.ToString() + "\r\nSent: " + sent.ToString() + "\r\n"
					+ "Failed: " + (count - sent).ToString(),
					EventLogEntryType.Information);

			} catch (Exception majorException) {
				// make sure we catch all errors and report them
				this.EventLog.WriteEntry("There was a major exception during a Timer Elapse: "
					+ majorException.Message + "\r\nStack" + majorException.StackTrace, EventLogEntryType.Error);
				tmrMain.Enabled = true;	// we should renable this
			}
		}

		private void logMailoutException(int mailoutId, string emailAddress, string exceptionMessage) {
			try {
				MailoutException e = new MailoutException();
				e.EmailAddress = emailAddress;
				e.ErrorMessage = exceptionMessage;
				e.MailoutID = mailoutId;
				db.MailoutExceptions.InsertOnSubmit(e);
				db.SubmitChanges(ConflictMode.FailOnFirstConflict);
			} catch (Exception ex) {
				this.EventLog.WriteEntry("Exception in logMailoutException(mailoutId: "
					+ mailoutId.ToString() + ", emailAddress: " + emailAddress + ", exceptionMessage: "
					+ exceptionMessage + "): " + ex.Message, EventLogEntryType.Error);
			}
		}

		private XmlDocument getServiceParameters(int serviceId, int customerId) {
			try {
				string key = serviceId.ToString() + "," + customerId.ToString();

				// dict not created
				if (serviceParameters == null)
					serviceParameters = new Dictionary<string, XmlDocument>();

				if (serviceParameters.ContainsKey(key))
					// we don't need to query the DB since we have
					// this in memory already
					return serviceParameters[key];

				// query the database for the service parameters,
				// store them in the dictionary
				IQueryable<ServiceSubscription> query =
					from s in db.ServiceSubscriptions
					where s.ServiceID == serviceId && s.PartnerID == customerId
					select s;

				foreach (ServiceSubscription s in query) {
					if (s.Parameters != null) {
						// load the XML
						XmlDocument xml = new XmlDocument();
						xml.LoadXml(s.Parameters.ToString());
						// store in dict
						serviceParameters.Add(key, xml);
					}
				}

				// finally, return the XmlDocument requested
				if (serviceParameters.ContainsKey(key))
					return serviceParameters[key];
				else
					return null;
			} catch (Exception ex) {
				// not really expecting this
				this.EventLog.WriteEntry("Serious error in getServiceParameters(serviceId: "
					+ serviceId.ToString() + ", customerId: " + customerId.ToString() + "): " + ex.Message + "\r\nStack: "
					+ ex.StackTrace, EventLogEntryType.Error);
				return null;
			}
		}

		/// <summary>
		/// get the HTML template for a given service ID
		/// </summary>
		/// <param name="serviceId"></param>
		/// <returns></returns>
		/*
		private string getHtmlTemplate(int serviceId, int customerId) {
			try {
				string key = serviceId.ToString() + "," + customerId.ToString();

				// dictionary not created
				if (htmlTemplates == null)
					htmlTemplates = new Dictionary<string, string>();

				if (htmlTemplates.ContainsKey(key))
					// we don't need to query the DB since we have
					// this in memory already
					return htmlTemplates[key];

				// query the database for the template,
				// load it to the dictionary
				IQueryable<ServiceSubscription> query =
					from s in db.ServiceSubscriptions
					where s.ServiceID == serviceId && s.CustomerID == customerId
					select s;
				foreach (ServiceSubscription s in query) {
					// load the XML
					XmlDocument xDoc = new XmlDocument();
					xDoc.LoadXml(s.Parameters.ToString());
					// get the required param from the XML parameters
					string templateLocation = getXmlParameterValue(xDoc, "EmailTemplate");
					//this.EventLog.WriteEntry("templateLocation = " + templateLocation, EventLogEntryType.Information);
					if (templateLocation != null) {
						// read in the HTML from a file

						string html = System.IO.File.ReadAllText(templateLocation);
						//WebClient c = new WebClient();
						//string html = c.DownloadString(templateLocation);
						//c.Dispose();
						htmlTemplates.Add(key, html);
					}
				}

				// finally we send back the HTML
				return htmlTemplates[key];
			} catch (Exception ex) {
				// not really expecting this
				this.EventLog.WriteEntry("Serious error in getHtmlTemplate(serviceId: "
					+ serviceId.ToString() + ", customerId: " + customerId.ToString() + "): " + ex.Message + "\r\nStack: "
					+ ex.StackTrace, EventLogEntryType.Error);
				// return some default html
				return "<html><body>InZone Mail System Error. Please ignore this message.</body></html>";
			}
		}
		 */

		/// <summary>
		/// Parses an XMLDocument for a Parameter node with
		/// a name attribute equal to 'paramName' and returns it's innerText
		/// </summary>
		/// <param name="doc">XmlDocument</param>
		/// <param name="paramName">Parameter name</param>
		/// <returns>null if param is not found</returns>
		private string getXmlParameterValue(XmlDocument doc, string paramName) {
			try {
				paramName = paramName.ToLower();
				foreach (XmlNode n in doc.ChildNodes) {
					if (n.Name.ToLower() == "parameters") {
						foreach (XmlNode cn in n.ChildNodes) {
							if (cn.Name.ToLower() == "parameter") {
								foreach (XmlAttribute a in cn.Attributes) {
									if (a.Name.ToLower() == "name" && a.Value.ToLower() == paramName) {
										return cn.InnerText;
									}
								}
							}
						}
					}
				}
				return null;
			} catch (Exception ex) {
				this.EventLog.WriteEntry("Error in getXmlParameterValue(doc: " + doc.ToString()
					+ ", paramName: " + paramName + "): " + ex.Message + "\r\nStack: " + ex.StackTrace);
				return null;
			}
		}


	}
}

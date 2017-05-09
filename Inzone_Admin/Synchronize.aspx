<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Synchronize.aspx.cs" Inherits="Synchronize" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<title>InZone</title>
	<meta content="text/html; charset=windows-1252" http-equiv="Content-Type" />
	<style type="text/css">
<!--
 /* Font Definitions */
 @font-face
	{font-family:"Arial";
	panose-1:2 4 5 3 5 4 6 3 2 4;}
@font-face
	{font-family:Arial;
	panose-1:2 15 5 2 2 2 4 3 2 4;}
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:10.0pt;
	margin-left:0in;
	line-height:115%;
	font-size:11.0pt;
	font-family:"Arial","Arial";}
p.MsoFootnoteText, li.MsoFootnoteText, div.MsoFootnoteText
	{mso-style-link:"Footnote Text Char";
	margin:0in;
	margin-bottom:.0001pt;
	font-size:10.0pt;
	font-family:"Arial","Arial";}
span.MsoSubtleEmphasis
	{font-family:"Arial","Arial";
	color:gray;
	font-style:italic;}
span.FootnoteTextChar
	{mso-style-name:"Footnote Text Char";
	mso-style-link:"Footnote Text";
	font-family:"Arial","Arial";}
p.DecimalAligned, li.DecimalAligned, div.DecimalAligned
	{mso-style-name:"Decimal Aligned";
	margin-top:0in;
	margin-right:0in;
	margin-bottom:10.0pt;
	margin-left:0in;
	line-height:115%;
	font-size:11.0pt;
	font-family:"Arial","Arial";}
.MsoChpDefault
	{font-size:10.0pt;}
@page Section1
	{size:8.5in 11.0in;
	margin:1.0in 1.0in 1.0in 1.0in;}
div.Section1
	{page:Section1;}
-->
</style>
</head>
<body>
	<form id="form1" runat="server">
		<table border="1" cellpadding="0" cellspacing="0" style='width: 100.0%; border: 0px'
			width="100%">
			<tr>
				<td style='width: 100.0%; border-top: solid windowtext 2.25pt; border-left: none;
					border-bottom: solid windowtext 2.25pt; border-right: none; background: #4BACC6;
					padding: 0in 5.4pt 0in 5.4pt' valign="top">
					<p class="MsoNormal" style='margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal'>
						<b><span style='color: white'>Synchronize local data with mirror server</span></b></p>
				</td>
			</tr>
			<tr>
				<td style='border: 0px;'>
					&nbsp;
				</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					Records out of sync:</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					&nbsp;</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					<table border="0" cellpadding="5" cellspacing="0" style="border-right: #008b8b 1px solid;
						border-top: #008b8b 1px solid; border-left: #008b8b 1px solid; border-bottom: #008b8b 1px solid">
						<tr>
							<td style="width: 99px">
								Customers</td>
							<td align="center">
								<asp:Label ID="lblCustomerCount" runat="server" Font-Bold="True" ForeColor="Teal"
									Text="Label"></asp:Label></td>
						</tr>
						<tr>
							<td style="width: 99px">
								Events</td>
							<td align="center">
								<asp:Label ID="lblEventCount" runat="server" Font-Bold="True" ForeColor="Teal" Text="Label"></asp:Label></td>
						</tr>
						<tr>
							<td style="width: 99px">
								Participants</td>
							<td align="center">
								<asp:Label ID="lblParticipantCount" runat="server" Font-Bold="True" ForeColor="Teal"
									Text="Label"></asp:Label></td>
						</tr>
						<tr>
							<td style="width: 99px">
								Interactions</td>
							<td align="center">
								<asp:Label ID="lblInteractionCount" runat="server" Font-Bold="True" ForeColor="Teal"
									Text="Label"></asp:Label></td>
						</tr>
						<tr>
							<td style="width: 99px">
								<strong>Total</strong></td>
							<td align="center">
								<asp:Label ID="lblTotalCount" runat="server" Font-Bold="True" ForeColor="Teal" Text="Label"></asp:Label></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					&nbsp;</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					<asp:Button ID="btnSyncNow" runat="server" OnClick="btnSyncNow_Click" OnClientClick="return confirm('You should know...\n\n\t* This action can not be undone\n\t* The process may take some time\n\t* This should not be interupted once started\n\nContinue with synchronization?');"
						Text="Synchronize now" /></td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					&nbsp;</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					<asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></td>
			</tr>
		</table>
	</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
		<table border="1" cellpadding="0" cellspacing="0" style='width: 100.0%; border: 0px' width="100%">
			<tr>
				<td style='width: 100.0%; border-top: solid windowtext 2.25pt; border-left: none;
					border-bottom: solid windowtext 2.25pt; border-right: none; background: #4BACC6;
					padding: 0in 5.4pt 0in 5.4pt' valign="top">
					<p class="MsoNormal" style='margin-bottom: 0in; margin-bottom: .0001pt; line-height: normal'>
						<b><span style='color: white'>Tasks</span></b></p>
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
					<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/RegisterCustomer.aspx">Add Customer</asp:HyperLink></td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in;
					border-top-style: none; padding-top: 0in; border-right-style: none; border-left-style: none;
					border-bottom-style: none" valign="top">
					&nbsp;</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					<asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/RegisterEvent.aspx">Add Event</asp:HyperLink></td>
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
					<asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Customers.aspx">View Customers</asp:HyperLink></td>
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
					<asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Events.aspx">View Events / Set active Event</asp:HyperLink></td>
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
					<asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/Participants.aspx">View Participants</asp:HyperLink></td>
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
					<asp:HyperLink ID="hlSync" runat="server" NavigateUrl="~/Synchronize.aspx">Synchronize local data with mirror server</asp:HyperLink></td>
			</tr>
		</table>
		<br />
		<br />
		<asp:Button ID="btnTest" runat="server" OnClick="btnTest_Click" Text="Test" Visible="False" />
		<asp:Label ID="lblTestResult" runat="server"></asp:Label>
	</form>
</body>
</html>

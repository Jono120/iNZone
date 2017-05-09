<%@ Page Language="C#" AutoEventWireup="true" CodeFile="unsubscribe.aspx.cs" Inherits="mail_unsubscribe_unsubscribe" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>InZone</title>
	<link href="../../css/inzone.css" rel="stylesheet" type="text/css" />
</head>
<body>
	<form id="unsub" runat="server">
		<div>
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td align="left" style="background-image: url(../../images/bkg_header.gif); height: 128px;"
						valign="top">
						<img alt="InZone DataZone" height="128" src="../../images/inzone_logo.gif" width="534" />
					</td>
				</tr>
			</table>
			<div id="divPrompt" runat="server" visible="false">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td align="center" colspan="2" style="height: 465px;" valign="middle">
							<span style="color: #ffffff">This will unsubscribe you from all<br />
								<b>InZone Career Unit</b> emails regarding:<br />
								<br />
								<asp:Label ID="lblCustomerName" runat="server" Font-Bold="True"></asp:Label><br />
								<br />
								Are you sure?</span><br />
							<br />
							<asp:Button ID="btnYes" runat="server" Text="Yes" Width="80px" BackColor="#333333"
								BorderColor="White" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" OnClick="btnYes_Click" />&nbsp;
							<asp:Button ID="btnNo" runat="server" Text="No" Width="80px" BackColor="#333333"
								BorderColor="White" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" OnClick="btnNo_Click" />
						</td>
					</tr>
				</table>
			</div>
			<div id="divError" runat="server" visible="false">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td align="center" colspan="2" style="height: 465px;" valign="middle">
							<span style="color: #ffffff"><b>Error: </b>
								<asp:Label ID="lblError" runat="server"></asp:Label></span>
						</td>
					</tr>
				</table>
			</div>
			<div id="divOK" runat="server" visible="false">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td align="center" colspan="2" style="height: 465px;" valign="middle">
							<span style="color: #ffffff"><b>You were unsubscribed successfully!</b></span>
						</td>
					</tr>
				</table>
			</div>
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td align="left" style="background-image: url(../../images/bkg_header.gif); width: 24px;
						height: 24px;" valign="middle">
						<img alt="" height="24" src="../../images/0.gif" width="24" />
					</td>
					<td align="left" style="background-image: url(../../images/bkg_header.gif);" valign="middle">
						<span class="style1">Inzone owns all rights and content accessible online from the datazone.
							Any use of this information is subject to the Privacy Principles provided by the
							Privacy Act 1993.</span>
					</td>
				</tr>
			</table>
		</div>
	</form>
</body>
</html>

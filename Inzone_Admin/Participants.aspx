<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Participants.aspx.cs" Inherits="Participants" %>

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
						<b><span style='color: white'>Participants -
							<asp:Label ID="lblCurrentEventName" runat="server"></asp:Label></span></b></p>
				</td>
			</tr>
			<tr>
				<td style='border: 0px;'>
					&nbsp;&nbsp;
				</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					<ul>
						<li>This is a list of participants who have shown activity or have registered during
							the current event</li>
						<li>You may log participants in or out</li>
						<li>You may delete participants if required</li>
					</ul>
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
                    <asp:HyperLink ID="hlRefresh" runat="server" NavigateUrl="~/Participants.aspx">Refresh</asp:HyperLink></td>
            </tr>
            <tr>
                <td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
                    padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
                    valign="top">
                    &nbsp;</td>
            </tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none;
					height: 19px;" valign="top">
					<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
						CellPadding="8" DataSourceID="sdsParticipants" ForeColor="#333333" GridLines="None"
						AutoGenerateColumns="False" DataKeyNames="ID" PageSize="15">
						<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
						<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
						<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
						<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
						<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
						<EditRowStyle BackColor="#999999" />
						<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
						<Columns>
							<asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
								SortExpression="ID">
								<HeaderStyle HorizontalAlign="Center" />
								<ItemStyle HorizontalAlign="Center" />
							</asp:BoundField>
							<asp:BoundField DataField="Firstname" HeaderText="Firstname" ReadOnly="True" SortExpression="Firstname" />
							<asp:BoundField DataField="Lastname" HeaderText="Lastname" ReadOnly="True" SortExpression="Lastname" />
							<asp:BoundField DataField="Association" HeaderText="Association" ReadOnly="True"
								SortExpression="Association" />
							<asp:BoundField DataField="Gender" HeaderText="Gender" ReadOnly="True" SortExpression="Gender" />
							<asp:BoundField DataField="RegistrationCode" HeaderText="RegistrationCode" ReadOnly="True"
								SortExpression="RegistrationCode" />
							<asp:CheckBoxField DataField="LoggedIn" HeaderText="LoggedIn" SortExpression="LoggedIn">
								<HeaderStyle HorizontalAlign="Center" />
								<ItemStyle HorizontalAlign="Center" />
							</asp:CheckBoxField>
							<asp:BoundField DataField="LastActivity" HeaderText="LastActivity" ReadOnly="True"
								SortExpression="LastActivity" >
								<HeaderStyle HorizontalAlign="Left" />
								<ItemStyle HorizontalAlign="Left" />
							</asp:BoundField>
							<asp:CommandField ShowEditButton="True" />
							<asp:TemplateField>
								<AlternatingItemTemplate>
									<asp:LinkButton ID="lbDelete" runat="server" CommandName="Delete" ForeColor="#284775"
										OnClientClick="return confirm('This will remove the participant and all interaction history associated with him/her permanently. Do you wish to continue?');"
										Text="Delete"></asp:LinkButton>
								</AlternatingItemTemplate>
								<ItemTemplate>
									<asp:LinkButton ID="lbDelete" Text="Delete" runat="server" CommandName="Delete" OnClientClick="return confirm('This will remove the participant and all interaction history associated with him/her permanently. Do you wish to continue?');"
										ForeColor="#333333"></asp:LinkButton>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
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
					<asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink>&nbsp;</td>
			</tr>
		</table>
		<asp:SqlDataSource ID="sdsParticipants" runat="server" ConnectionString="<%$ ConnectionStrings:InZoneConnectionString %>"
			SelectCommand="SELECT DISTINCT Participants.ID, Participants.Firstname, Participants.Lastname, Participants.Association, Participants.Gender, Participants.RegistrationCode, Participants.LoggedIn, Customers.Name AS LastActivity&#13;&#10;FROM Events INNER JOIN Interactions ON Events.ID = Interactions.EventID INNER JOIN Participants ON Interactions.ParticipantID = Participants.ID INNER JOIN Customers ON Participants.LastCustomerID = Customers.ID&#13;&#10;WHERE Events.ID = (SELECT ID FROM Events WHERE ([Current] = 1))"
			DeleteCommand="DELETE FROM Interactions WHERE (ParticipantID = @ID)&#13;&#10;DELETE FROM Participants WHERE (ID = @ID)"
			UpdateCommand="UPDATE [Participants] SET [LoggedIn] = @LoggedIn WHERE ([ID] = @ID)">
			<UpdateParameters>
				<asp:Parameter Name="LoggedIn" />
				<asp:Parameter Name="ID" />
			</UpdateParameters>
			<DeleteParameters>
				<asp:Parameter Name="ID" />
			</DeleteParameters>
		</asp:SqlDataSource>
	</form>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Events.aspx.cs" Inherits="Events" %>

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
						<b><span style='color: white'>Events</span></b></p>
				</td>
			</tr>
			<tr>
				<td style='border: 0px;'>
					&nbsp;
				</td>
			</tr>
			<tr>
				<td style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;
					border-right-width: 0px">
					<ul>
						<li><em>The active Event is always shown at the top of the list, the rest of the list
							is sorted in alphabetical order</em></li>
						<li>You may make any Event the active event by clicking the 'Edit' link next to it and
							toggling it's checkbox. Click 'Update' to save the changes.</li>
						<li>You may sort by any individual column</li>
					</ul>
				</td>
			</tr>
			<tr>
				<td style="border-top-width: 0px; border-left-width: 0px; border-bottom-width: 0px;
					border-right-width: 0px">
					&nbsp;</td>
			</tr>
			<tr>
				<td style="padding-right: 5.4pt; padding-left: 5.4pt; padding-bottom: 0in; border-top-style: none;
					padding-top: 0in; border-right-style: none; border-left-style: none; border-bottom-style: none"
					valign="top">
					<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
						AutoGenerateColumns="False" CellPadding="10" DataKeyNames="ID" DataSourceID="sdsEvents"
						ForeColor="#333333" GridLines="None" PageSize="15" Width="800px">
						<PagerSettings Position="TopAndBottom" />
						<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
						<RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
						<Columns>
							<asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
								SortExpression="ID">
								<HeaderStyle HorizontalAlign="Center" />
								<ItemStyle HorizontalAlign="Center" />
							</asp:BoundField>
							<asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" SortExpression="Name">
								<HeaderStyle HorizontalAlign="Left" Width="200px" />
                                <ItemStyle Width="200px" />
							</asp:BoundField>
							<asp:BoundField DataField="EventDate" HeaderText="Date" 
								SortExpression="EventDate" DataFormatString="{0:dd} {0:MMM} {0:yyyy}" />
							<asp:CheckBoxField DataField="Current" HeaderText="Current" SortExpression="Current">
								<HeaderStyle HorizontalAlign="Center" />
								<ItemStyle HorizontalAlign="Center" />
							</asp:CheckBoxField>
							<asp:CommandField ShowEditButton="True" />
						</Columns>
						<PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
						<SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
						<HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
						<EditRowStyle BackColor="#999999" />
						<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
					<asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/Default.aspx">Home</asp:HyperLink></td>
			</tr>
		</table>
		<asp:SqlDataSource ID="sdsEvents" runat="server" ConnectionString="<%$ ConnectionStrings:InZoneConnectionString %>"
			DeleteCommand="DELETE FROM [Events] WHERE [ID] = @ID" InsertCommand="INSERT INTO [Events] ([Name], [Current]) VALUES (@Name, @Current)"
			ProviderName="<%$ ConnectionStrings:InZoneConnectionString.ProviderName %>" SelectCommand="SELECT ID, Name, [Current], EventDate FROM Events ORDER BY [Current] DESC, Name"
			
			UpdateCommand="UPDATE [Events] SET [Current] = 0&#13;&#10;UPDATE [Events] SET [Current] = @Current WHERE [ID] = @ID">
			<DeleteParameters>
				<asp:Parameter Name="ID" Type="Int32" />
			</DeleteParameters>
			<UpdateParameters>
				<asp:Parameter Name="Name" Type="String" />
				<asp:Parameter Name="Current" Type="Boolean" />
				<asp:Parameter Name="ID" Type="Int32" />
			</UpdateParameters>
			<InsertParameters>
				<asp:Parameter Name="Name" Type="String" />
				<asp:Parameter Name="Current" Type="Boolean" />
			</InsertParameters>
		</asp:SqlDataSource>
	</form>
</body>
</html>

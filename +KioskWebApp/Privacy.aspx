<%@ Page Title="" Language="C#" MasterPageFile="~/Global.Master" AutoEventWireup="true"
    CodeBehind="Privacy.aspx.cs" Inherits="KioskApplication.Privacy" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 

     <script type="text/javascript">

        //Disable text selection on appropriate elements
        window.onload = function() {
        DisableSelection(document.getElementById("Privacy"));
        }

        function DisableSelection(target) {
            if (target != null) {
                target.onselectstart = function() { return false; }
                
            }
        }     
    
    </script>
<div id="Privacy">

<br />
<br />
<div id="privacy-policy"><span class="style1">Privacy Agreement</span><br /><br /><br />
    PRIVACY POLICY<br />
    <br />This privacy policy discloses Inzone's information and dissemination practices. It is designed to protect individual privacy in accordance with the New Zealand Privacy Act 1993 legislation.

    <br />
    <br />
    1. Information data about Inzone's members will be collected by Inzone and held on an Inzone server.
    <br />
    <br />
    2. Data will be used and disclosed by Inzone and its partners to provide career related information and training
services to members. These services include;
    <br />
    - the provisions of employment opportunities and recruitment services.
    <br />
    - industry related news and training information and market research.<br />
    <br />
    Information from the database may also be used by Inzone and/or its partners for planning, product development,
research and marketing purposes.
    <br />
    <br />
    3.By joining Inzone’s Careers database you consent to the collection and use of personal information by Inzone's
partners for the provision of career related services.
    <br />
    <br />
    4.Information about Inzone members will not be provided by Inzone or its partners to any person except as
provided in this Privacy Policy.
    <br />
    <br />
    5.Inzone and its partners will take reasonable steps to ensure that all information collected is stored in a secure
environment accessed only by authorized persons.
    <br />
    <br />
    6.Under the provisions of the Privacy Act 1993, requested access to personal information held by Inzone will be
made available to all members of the public for viewing and/or amendments to any information incorrectly
recorded. Inzone may decline any request if another member’s privacy policy is breached by the request.
    <br />
    <br />
    To request this information, please email<span class="style2"> peter@inzone.co.nz

&nbsp;</span></div>
    <table width="90%" cellpadding="5px" class="privacy-buttons">
       <tr>
            <td >
                <asp:ImageButton ID="AcceptButton" runat="server" ImageUrl="Images/accept_btn.png"
                    TabIndex="1" />
            &nbsp;
                <asp:ImageButton ID="DeclineButton" runat="server" ImageUrl="Images/decline_btn.png"
                    TabIndex="2" />
             &nbsp;
                </td>
        </tr>
    </table>
<div style="margin-left: 150px; font-size: small; font-family: Arial, Helvetica, sans-serif; font-weight: 700;">
            <span class="style3">Inzone’s Partners are :
</span>
            <br />
            <span class="style4">The New Zealand Defence Forces, The Ministry of Social Development, The Ministry of Health, The NZ District
Health Board, The Motor Industry ITO, Massey University, The Forestry ITO, Outwardbound, The Extractives
Industry ITO, The Seafishing Industry ITO,
</span>
</div>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style1
        {
            color: #33CCCC;
            font-size: large;
        }
        .style2
        {
            text-decoration: underline;
        }
        .style3
        {
            color: #FFFFFF;
            text-decoration: underline;
        }
        .style4
        {
            color: #FFFF00;
        }
    </style>
</asp:Content>


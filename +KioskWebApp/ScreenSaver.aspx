<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScreenSaver.aspx.cs" Inherits="KioskApplication.ScreenSaver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="global.css" rel="stylesheet" type="text/css" />
</head>
<body class="screen-saver" >
    <form id="form1" runat="server">
    
    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                    width="100%" height="100%">                    
                    <param name="movie" value="Flash/ScreenSaver.swf" />
                    <param name="autoStart" value="true" />
                </object>  
    
    </form>
</body>
</html>

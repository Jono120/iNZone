function $(i){return document.getElementById(i);}
function redirectHome(){setTimeout(window.location.href='home.aspx',2000);}
function processLogin(f){
	$('<%=lblResult.ClientID%>').innerHTML='Authenticating...';
	$('hidePassword').value=hex_sha1($('password').value);
	$('password').value='';
	return true;
}
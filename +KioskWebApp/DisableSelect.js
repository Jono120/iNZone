window.onload = function() {
    document.onselectstart = function() { return false; }

}

/* You can attach the events to any element. In the following example
I'll disable selecting text in an element with the id 'content'. */

window.onload = function() {

    var element = document.getElementById('<%= UsernameLabel.ClientID %>');
    alert(element);
    element.onselectstart = function() { return false; }
}

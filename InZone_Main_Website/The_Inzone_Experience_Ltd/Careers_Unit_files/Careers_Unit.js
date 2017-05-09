// Created by iWeb 3.0.1 local-build-20090409

function createMediaStream_id2()
{return IWCreatePhotocast("http://www.inzone.co.nz/The_Inzone_Experience_Ltd/Careers_Unit_files/rss.xml",true);}
function initializeMediaStream_id2()
{createMediaStream_id2().load('http://www.inzone.co.nz/The_Inzone_Experience_Ltd',function(imageStream)
{var entryCount=imageStream.length;var headerView=widgets['widget1'];headerView.setPreferenceForKey(imageStream.length,'entryCount');NotificationCenter.postNotification(new IWNotification('SetPage','id2',{pageIndex:0}));});}
function layoutMediaGrid_id2(range)
{createMediaStream_id2().load('http://www.inzone.co.nz/The_Inzone_Experience_Ltd',function(imageStream)
{if(range==null)
{range=new IWRange(0,imageStream.length);}
IWLayoutPhotoGrid('id2',new IWPhotoGridLayout(4,new IWSize(143,143),new IWSize(143,32),new IWSize(153,190),27,27,0,new IWSize(4,5)),new IWPhotoFrame([IWCreateImage('Careers_Unit_files/green_linen_ul.png'),IWCreateImage('Careers_Unit_files/green_linen_top.png'),IWCreateImage('Careers_Unit_files/green_linen_ur.png'),IWCreateImage('Careers_Unit_files/green_linen_right.png'),IWCreateImage('Careers_Unit_files/green_linen_lr.png'),IWCreateImage('Careers_Unit_files/green_linen_bottom.png'),IWCreateImage('Careers_Unit_files/green_linen_ll.png'),IWCreateImage('Careers_Unit_files/green_linen_left.png')],null,0,0.750000,13.000000,12.000000,13.000000,13.000000,15.000000,15.000000,15.000000,15.000000,15.000000,15.000000,15.000000,15.000000,null,null,null,0.400000),imageStream,range,null,null,1.000000,{backgroundColor:'rgb(0, 0, 0)',reflectionHeight:100,reflectionOffset:2,captionHeight:100,fullScreen:0,transitionIndex:2},'Media/slideshow.html','widget1','widget2','widget3')});}
function relayoutMediaGrid_id2(notification)
{var userInfo=notification.userInfo();var range=userInfo['range'];layoutMediaGrid_id2(range);}
function onStubPage()
{var args=window.location.href.toQueryParams();parent.IWMediaStreamPhotoPageSetMediaStream(createMediaStream_id2(),args.id);}
if(window.stubPage)
{onStubPage();}
setTransparentGifURL('Media/transparent.gif');function hostedOnDM()
{return false;}
function onPageLoad()
{IWRegisterNamedImage('comment overlay','Media/Photo-Overlay-Comment.png')
IWRegisterNamedImage('movie overlay','Media/Photo-Overlay-Movie.png')
loadMozillaCSS('Careers_Unit_files/Careers_UnitMoz.css')
adjustLineHeightIfTooBig('id1');adjustFontSizeIfTooBig('id1');NotificationCenter.addObserver(null,relayoutMediaGrid_id2,'RangeChanged','id2')
adjustLineHeightIfTooBig('id3');adjustFontSizeIfTooBig('id3');adjustLineHeightIfTooBig('id4');adjustFontSizeIfTooBig('id4');adjustLineHeightIfTooBig('id5');adjustFontSizeIfTooBig('id5');adjustLineHeightIfTooBig('id6');adjustFontSizeIfTooBig('id6');adjustLineHeightIfTooBig('id7');adjustFontSizeIfTooBig('id7');Widget.onload();fixAllIEPNGs('Media/transparent.gif');initializeMediaStream_id2()
performPostEffectsFixups()}
function onPageUnload()
{Widget.onunload();}

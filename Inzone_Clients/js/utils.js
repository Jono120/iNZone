var resultCount, maxResults = 1000;
function confirmExport() {
	if(resultCount > maxResults) {
		return confirm(	"This result set exceeds " + maxResults + " results, which will take longer to build\n" +
						"by our server. Click 'Ok' to continue, but please be patient while we\n" +
						"send you the data.");
	} else return true;
}
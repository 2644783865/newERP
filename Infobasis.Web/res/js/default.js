function Url(url) {
    this.queryString = {};
    this.hash = '';
    var cutUrl = url;

    var hashPos = cutUrl.indexOf("#");
    if (hashPos > -1) {
        this.hash = cutUrl.substring(hashPos + 1, cutUrl.length);
        cutUrl = cutUrl.substring(0, hashPos);
    }

    var queryPos = cutUrl.indexOf("?");

    if (queryPos > -1) {
        var query = cutUrl.substring(queryPos + 1, cutUrl.length);
        cutUrl = cutUrl.substring(0, queryPos);

        var namedValues = query.split("&");
        for (var i in namedValues) {
            var pair = namedValues[i].split("=");
            this.queryString[pair[0]] = pair[1];
        }
    }

    this.baseUrl = cutUrl;

}

Url.prototype.toString = function () {
    var url = this.baseUrl;

    var isFirst = true;
    for (var key in this.queryString) {
        var value = this.queryString[key];
        if (isFirst)
            url += "?";
        else
            url += "&";
        url += key + (value != null && value.toString().length > 0 ? "=" + value : "");
        isFirst = false;
    }
    if (this.hash.length > 0) {
        url = url + '#' + this.hash;
    }
    return url;
};

// "Static" utility method to add a name-value pair to querystring
Url.setValue = function (urlStr, name, value) {
    var url = new Url(urlStr);
    url.queryString[name] = value;
    return url.toString();
};

//=======================================================================
// Merge queystring to the given url
// querystring format "name1=val1&name2=val2"
function mergeQuerystring(url, queryString) {
    if (queryString != null && queryString.length > 0) {
        var newUrl = new Url(url);
        var nameValuePairs = queryString.split('&');
        for (var queryString in nameValuePairs) {
            var nameValuePair = nameValuePairs[queryString].split('=');
            newUrl.queryString[nameValuePair[0]] = nameValuePair[1];
        }
    }
    return newUrl.toString();
}

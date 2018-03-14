var infobasisService = (function(){
    var infobasisServiceRtn = {};
    var jwtToken = getCookie('JWTToken');
    var _getAjaxInstance = axios.create({
        baseURL: pageSetting.apiUrl,
        timeout: 30000,
        headers: { 'Authorization': 'Bearer ' + jwtToken }
    });

    function getCookie(name) {
        var findCookies = new RegExp(name + "=([^;]*)");
        var submatches = findCookies.exec(document.cookie);
        if (submatches != null && submatches.length > 1)
            return submatches[1];
        else
            return null;
    }
    function setCookie(name, value) {
        document.cookie = name + "=" + value
            + "; path=" + pageSetting.siteRootPath; // note path is case-sensitive to browser
    }
    function deleteCookie(name) {
        var currentValue = getCookie(name);
        if (currentValue != null && currentValue != "")
            document.cookie = name + "=; expires=" + new Date(0).toGMTString() + "; path=" + pageSetting.siteRootPath;
    }

    infobasisServiceRtn.getAjaxInstance = _getAjaxInstance;

    return infobasisServiceRtn;
})();

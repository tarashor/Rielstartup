function Cookie(name, value, expireDate) {
    this.name = name;
    this.value = value;
    this.expireDate = expireDate;
};

Cookie.setCookie = function (cookie) {
    var c_value = escape(cookie.value) + ((cookie.expireDate == null) ? "" : "; expires=" + cookie.expireDate.toUTCString()) + "; path=/";
    document.cookie = cookie.name + "=" + c_value;

};

Cookie.getCookie = function (name) {
    var i, x, y, ARRcookies = document.cookie.split(";");
    for (i = 0; i < ARRcookies.length; i++) {
        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == name) {
            return unescape(y);
        }
    }
    return undefined;
};

Cookie.deleteCookie = function (name) {
    var date = new Date();
    date.setTime(date.getTime() + (-1 * 24 * 60 * 60 * 1000));
    var expires = "; expires=" + date.toGMTString();
    document.cookie = name + "= " + expires + "; path=/";
};
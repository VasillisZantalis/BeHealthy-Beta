function getLocale() {
    var locale = document.getElementById("currentCulture").value;

    var lang = 'en';
    if (locale == 'el-GR') {
        lang = 'el';
    }
    else if (locale == 'en-GB') {
        lang = 'en';
    }

    return lang;
}
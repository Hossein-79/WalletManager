// DARK MODE
// set darkmode cookie
function setDarkModeCookie(val) {
    var d = new Date();
    d.setTime(d.getTime() + (365 * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = 'darkmode=' + val + ';' + expires + ';path=/';
}
// get darkmode cookie
function getDarkModeCookie() {
    var name = "darkmode=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length) == 'true';
        }
    }
    return "";
}

if (getDarkModeCookie() == true) {
    $('.dark-mode').each(function (i, elem) {
        $(elem).prop('checked', true);
    });
    $('.light-mode').each(function(i, elem) {
        $(elem).prop('checked', false);
    });
} else if(getDarkModeCookie() == false) {
    $('.dark-mode').each(function (i, elem) {
        $(elem).prop('checked', false);
    });
    $('.light-mode').each(function(i, elem) {
        $(elem).prop('checked', true);
    });
} else {
    $('.dark-mode').each(function (i, elem) {
        $(elem).prop('checked', false);
    });
    $('.light-mode').each(function(i, elem) {
        $(elem).prop('checked', true);
    });
}
$(document).on('change', '[name="dark-mode"], [name="dark-mode-desktop"]', function() {
    var checkedRadio = $(this);
    if (checkedRadio.hasClass('dark-mode')) {
        setDarkModeCookie(true);
        $('html').addClass('dark');
    } else {
        setDarkModeCookie(false);
        $('html').removeClass('dark');
    }
});
// MODAL
$(document).on('click', '[data-modal]', function() {
    var modalId = $(this).attr('data-modal');
    $('#' + modalId).addClass('active');
    $('body').addClass('modal-active');
});
$(document).on('click', '.modal-close', function() {
    $(this).closest('.modal').removeClass('active');
    $('body').removeClass('modal-active');
});
// HIDE MODAL ON CLICK OUTSIDE
$(document).on('click', '.modal', function(e) {
    if (e.target !== this) {
        return;
    }
    $(this).removeClass('active');
    $('body').removeClass('modal-active');
});
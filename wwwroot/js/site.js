// DARK MODE
if (localStorage.getItem('darkmode') === 'true') {
    $('html').addClass('dark');
    $('#dark-mode-dark').prop('checked', true);
    $('#dark-mode-light').prop('checked', false)
} else if(localStorage.getItem('darkmode') === 'false') {
    $('html').removeClass('dark');
    $('#dark-mode-dark').prop('checked', false);
    $('#dark-mode-light').prop('checked', true);
} else {
    $('#dark-mode-dark').prop('checked', false);
    $('#dark-mode-light').prop('checked', true);
}
$(document).on('change', '[name="dark-mode"]', function() {
    var checkedRadio = $(this);
    if (checkedRadio.attr('data-mode') === 'dark') {
        localStorage.setItem('darkmode', true);
        $('html').addClass('dark');
    } else {
        localStorage.setItem('darkmode', false);
        $('html').removeClass('dark');
    }
});
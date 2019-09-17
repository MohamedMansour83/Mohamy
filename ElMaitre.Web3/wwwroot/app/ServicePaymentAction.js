function AjaxBegin() {
     
    if (localStorage.getItem('token') === null) {
        alert('تحتاج إلى تسجيل الدخول أولا');
        return;
    }
    var $form = $('form').first();
    var result = $form.valid();
    if (result == true) {
        $('#cover-spin').show(0);
        return true;
    }
    else {
        return false;
    }
}
function AjaxComplete(response) {
     
    if (response.responseJSON.ptype == 1) {
        location.href = 'http://accept.paymobsolutions.com/api/acceptance/iframes/7127?payment_token='
            + response.responseJSON.token;
    }
    else {
        location.href = 'http://accept.paymobsolutions.com/api/acceptance/iframes/7126?payment_token='
        location.href = response.responseJSON.redirect_url;
    }
}
$("header").removeClass("navbar-dark");
$("#menu_navbar").addClass("shadow");
$("#top_bar").addClass("shadow-sm");
function getval1(sel) {
    alert(sel.value);
    $('#frm1').attr('asp-route-price', sel.value);
}
function getval2(sel) {
    alert(sel.value);
    $('#frm2').attr('asp-route-price', sel.value);
}
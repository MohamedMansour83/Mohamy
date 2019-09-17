$(function () {
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll >= 97) {
            $("#menu_navbar").addClass('shrink');
            $("#top_bar").addClass('grayColor shadow-sm');
            $("header").removeClass("navbar-dark");
            $("header").addClass("navbar-light");
            $("#menu_navbar").addClass('shadow');
            $("#top_bar .nav-item").addClass('Border');

        } else {
            $("#menu_navbar").removeClass("shrink");
            $("header").removeClass("navbar-light");
            $("header").addClass("navbar-dark");
            $("#top_bar").removeClass('grayColor shadow-sm');
            $("#menu_navbar").removeClass('shadow');
            $("#top_bar .nav-item").removeClass('Border');
        }
    });
});




// scroll section

$(function () {
    var $window = $(window);
    $(".move").on("click", function () {
        $(".section").each(function () {
            var pos = $(this).offset().top;
             // //console.log($window.scrollTop(), pos);
            if ($window.scrollTop() < pos) {
                $("html, body").animate({
                    scrollTop: pos-110
                },
                        500
                        );
                return false;
            }
        });
    });

    function check() {
        if ($(".move").scrollTop() == 0) {
            $("#scrollup").hide();
            $(".next").show();
        } else if ($(".move").scrollTop() == height - 100) {
            $("#scrolldown").hide();
            $(".next").show();
        }
    }
});

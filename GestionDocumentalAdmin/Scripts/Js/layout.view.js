// Dinamic footer
function footerAlign() {
    $('footer').css('display', 'block');
    $('footer').css('height', 'auto');
    var footerHeight = $('footer').outerHeight();
    $('footer').css('height', footerHeight);
}

// Sidebar
function loadSidebar() {
    // Dropdown menu
    $(".sidebar-dropdown > a").click(function () {
        $(".sidebar-submenu").slideUp(200);
        if ($(this).parent().hasClass("active")) {
            $(".sidebar-dropdown").removeClass("active");
            $(this).parent().removeClass("active");
        } else {
            $(".sidebar-dropdown").removeClass("active");
            $(this).next(".sidebar-submenu").slideDown(200);
            $(this).parent().addClass("active");
        }

    });

    // toggle sidebar
    $("#toggle-sidebar").click(function () {
        $(".page-wrapper").toggleClass("toggled");
    });

    // Pin sidebar
    $("#close-sidebar").click(function () {
        if ($(".page-wrapper").hasClass("pinned")) {
            $("#hadle-menu").removeClass("fa-bars").addClass("fa-times");
            // unpin sidebar when hovered
            $(".page-wrapper").removeClass("pinned");
            $("#sidebar").unbind("hover");
        } else {
            $("#hadle-menu").removeClass("fa-times").addClass("fa-bars");
            $(".page-wrapper").addClass("pinned");
            $("#sidebar").hover(
                function () {
                    $(".page-wrapper").addClass("sidebar-hovered");
                    //$("#hadle-menu").removeClass("fa-times").addClass("fa-bars");
                },
                function () {
                    $(".page-wrapper").removeClass("sidebar-hovered");
                    //$("#hadle-menu").removeClass("fa-bars").addClass("fa-times");
                }
            );
        }
    });

    // toggle sidebar overlay
    $("#overlay").click(function () {
        $(".page-wrapper").toggleClass("toggled");
    });

    // switch between themes
    var themes = "default-theme legacy-theme chiller-theme ice-theme cool-theme light-theme";
    $('[data-theme]').click(function () {
        $('[data-theme]').removeClass("selected");
        $(this).addClass("selected");
        $('.page-wrapper').removeClass(themes);
        $('.page-wrapper').addClass($(this).attr('data-theme'));
    });

    // switch between background images
    var bgs = "bg1 bg2 bg3 bg4";
    $('[data-bg]').click(function () {
        $('[data-bg]').removeClass("selected");
        $(this).addClass("selected");
        $('.page-wrapper').removeClass(bgs);
        $('.page-wrapper').addClass($(this).attr('data-bg'));
    });

    // toggle background image
    $("#toggle-bg").change(function (e) {
        e.preventDefault();
        $('.page-wrapper').toggleClass("sidebar-bg");
    });

    // toggle border radius
    $("#toggle-border-radius").change(function (e) {
        e.preventDefault();
        $('.page-wrapper').toggleClass("border-radius-on");
    });

    // custom scroll bar is only used on desktop
    if (!/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        $(".sidebar-content").mCustomScrollbar({
            axis: "y",
            autoHideScrollbar: true,
            scrollInertia: 300
        });
        $(".sidebar-content").addClass("desktop");
    }
}

$(document).ready(function () {
    loadSidebar();
    footerAlign();
});

$(window).resize(function () {
    footerAlign();
});

/*Global utils methods*/
function drawRowNumbers(selector, table) {
    if (typeof (table) == 'undefined') return;

    var info = table.page.info();
    var index = info.start + 1;
    $.each($(selector + " tbody tr td:first-child"), function (idx, obj) {
        if ($(obj).hasClass('dataTables_empty')) return;
        $(obj).addClass('text-center').html(index++);
    });
}
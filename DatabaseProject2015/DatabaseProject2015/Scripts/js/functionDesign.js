
        jssor_1_slider_init = function() {
            
            var jssor_1_options = {
                $AutoPlay: true,
                $SlideDuration: 800,
                $SlideEasing: $Jease$.$OutQuint,
                $ArrowNavigatorOptions: {
                    $Class: $JssorArrowNavigator$
                },
                $BulletNavigatorOptions: {
                    $Class: $JssorBulletNavigator$
                }
            };
            
            var jssor_1_slider = new $JssorSlider$("jssor_1", jssor_1_options);
            
            //responsive code begin
            //you can remove responsive code if you don't want the slider scales while window resizes
            function ScaleSlider() {
                var refSize = jssor_1_slider.$Elmt.parentNode.clientWidth;
                if (refSize) {
                    refSize = Math.min(refSize, 1920);
                    jssor_1_slider.$ScaleWidth(refSize);
                }
                else {
                    window.setTimeout(ScaleSlider, 30);
                }
            }
            ScaleSlider();
            $Jssor$.$AddEvent(window, "load", ScaleSlider);
            $Jssor$.$AddEvent(window, "resize", $Jssor$.$WindowResizeFilter(window, ScaleSlider));
            $Jssor$.$AddEvent(window, "orientationchange", ScaleSlider);
            //responsive code end
        };


//<!----------------------------- Work when modal is close  ----------------------------->


$(document).ready(function(){
    $("#ModalSurway").on('hide.bs.modal', function(event){
        document.getElementById("loadingAJAX_listCourse").setAttribute("style", "z-index:1100; display:block; margin-top:50px;");
        var container = document.getElementById("AcademicRecordSection");
        container.innerHTML = '';
        $.ajax({
            type: "GET",
            url: "/Web/LoadAcademicRecords?Surway=true",
            data: param = "",
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: successFunc,
            error: errorFunc
        });

        function successFunc(data, status) {
            setTimeout(function () {
                $('#loadingAJAX_listCourse1').fadeOut();
            }, 10)
            container.innerHTML = data;
        }

        function errorFunc() {
            //alert('error');
        }
    });
});

//<!----------------------------- Work when modal is close  ----------------------------->



//<!----------------------------- Scroll to top ----------------------------->

jQuery(document).ready(function () {
    var offset = 150;
    var duration = 500;
    jQuery(window).scroll(function () {
        if (jQuery(this).scrollTop() > offset) {
            jQuery('.scroll-to-top').fadeIn(duration);
        } else {
            jQuery('.scroll-to-top').fadeOut(duration);
        }
    });

    jQuery('.scroll-to-top').click(function (event) {
        event.preventDefault();
        jQuery('html, body').animate({ scrollTop: 0 }, duration);
        return false;
    })
});

//<!-----------END----------- Scroll to top ----------------------------->



//<!----------------------------- Scroll to mark  ----------------------------->

function ScrollToMark(NameTarget) {
    // var target = $(this.hash);]
    var topPadding = 90;
    var target = $(this.hash);
    target = target.length ? target : $('[id=' + NameTarget + ']');
    if (target.length) {
        $('html,body').animate({
            scrollTop: target.offset().top - topPadding
        }, 600);
        return false;
    }
}

//<!-----------------------END------ Scroll to mark  ----------------------------->

//<!--------------------  Scroll/Follow Sidebar ----------------------------->

$(function () {
    var offset = $(".sidebar").offset();
    var topPadding = -60;
    $(window).scroll(function () {
        if ($(window).scrollTop() > offset.top) {
            $(".sidebar").stop().animate({
                marginTop: $(window).scrollTop() - offset.top + topPadding
            });
        } else {
            $(".sidebar").stop().animate({
                marginTop: 0
            });
        };
    });
});

//<!-------------END-------  Scroll/Follow Sidebar ----------------------------->


//<!--------------------  Menu active ----------------------------->
$(document).ready(function () {
        
    $('#menu > ul.nav li a').each(function () {
        var url = window.location;
            if (this.href == url) {
                $("#menu > ul.nav li").each(function () {
                    if ($(this).hasClass("active")) {
                       
                        $(this).removeClass("active");
                    }
                });
                $(this).parent().addClass('active');
            }
        });
});
//<!------------END---  Menu active ----------------------------->


//paste this code under head tag or in a seperate js file.
// Wait for window load
$(window).load(function () {
    // Animate loader off screen
    $(".se-pre-con").fadeOut("slow");;
});




//<!-------------------- Circle Progress Bar ----------------------------->

//jQuery(document).ready(function ($) {
//    $('.pie_progress').asPieProgress({
//        namespace: 'pie_progress'
//    });

//    $('.pie_progress').asPieProgress('start');


//    //var $variable = $('.ui-selected').innerHTML;


//    $('#text_current').keyup(function () {

//        $("#text_current").html(function (i, origText) {
//            return "Old html: " + origText + " New html: Hello <b>world!</b> (index: " + i + ")";
//        });

//        var content = $('#text_current').val;
//        alert(content);
//        //var value = $("#text_current").innerHTML;
//        //alert(value);
//        $('.pie_progress').asPieProgress('go', 80);
//    });
    //$('#Calculation_Grade_Estimation  > option:selected').each(function () {
    //    alert($(this).text() + ' ' + $(this).val());

    //});

    //var selectOption = document.getElementsByTagName('select');
    //var remember = "";
    //for (var i = 0; i < selectOption.length; i++) {
    //    remember = remember + selectOption[this.selectedIndex].textContent;
    //    }
    //alert(remember);

//});

//<!----------------END----  Circle Progress Bar ----------------------------->

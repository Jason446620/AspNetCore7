"use strict";

$("#status_div").append("<span class='hub-status-color' id='Status' style='background-color:'></span>");

var retried = false;
var retry;

function loadStatus(status) {
    
    if (status == "available") {
        if (retried == true) {
            clearInterval(retry);
            retried = false;
        }
       
        $("#Status").css("background-color", "#178600")

    } else if (status == "connecting") {
        var bling = false;
        var times = 0;
        if (!retried) {
            retried = true;

            retry = setInterval(function () {
                if (bling == true) {
                    $("#Status").css("background-color", "")
                    bling = false;
                } else {
                    $("#Status").css("background-color", "#FFD700")
                    bling = true;
                }
                times += 1;
                if (times == 120) {
                    $("#Status").css("background-color", "#A9A9A9")
                    clearInterval(retry);
                }
            }, 500)
           
        }
        
    } else {

        $("#Status").css("background-color", "#A9A9A9")
    }
}

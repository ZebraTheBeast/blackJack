$(document).ready(function () {

    $("#loginButton").click(function (event) {
        event.preventDefault();
        SetPlayerNameToCookie();
    });

});

function SetPlayerNameToCookie() {
    $.cookie("player-data", JSON.stringify($("#playerName").val()));
    window.location.href = "/Game/Game";
}
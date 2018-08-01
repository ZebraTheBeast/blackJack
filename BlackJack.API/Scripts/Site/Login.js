$(document).ready(function () {
    $("#loginButton").click(function (event) {
        event.preventDefault();
        StartGame();
    });
});

function StartGame() {
    var playerName = $("#playerName").val();
    $.ajax({
        url: '/api/values/StartGame',
        type: 'POST',
        data: JSON.stringify(playerName),
        contentType: "application/json;charset=utf-8",
        success: function () {
            $("#loadButton").trigger("click");
        }
    });
}
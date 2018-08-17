$(document).ready(function () {
    $("#loginButton").click(function (event) {
        event.preventDefault();
        StartGame();
    });

    $("#loadButton").click(function (event) {
        event.preventDefault();
        LoadGame();
    });

    $("#nameError").hide();
});

function StartGame() {
	var playerName = $("#playerName").val();
    $.ajax({
        url: '/api/values/StartGame',
        type: 'POST',
        data: JSON.stringify(playerName),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $.cookie("human-data", JSON.stringify(data));
            $("#loginForm").submit();
        },
        error: function () {
            $("#nameError").show();
        }
    });
}

function LoadGame() {
    var playerName = $("#playerName").val();
    $.ajax({
        url: '/api/values/LoadGame',
        type: 'POST',
        data: JSON.stringify(playerName),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $.cookie("human-data", JSON.stringify(data));
            $("#loginForm").submit();
        },
        error: function () {
            $("#nameError").show();
        }
    });
}
$(document).ready(function () {
    $("#loginButton").click(function (event) {
        event.preventDefault();
        StartGame();
    });

    $("#loadButton").click(function (event) {
        event.preventDefault();
        LoadGame();
    });

});

function StartGame() {
	var loginViewModel = {
		playerName: $("#playerName").val(),
		botsAmount: $("#botsAmount").val()
	};
    $.ajax({
        url: '/api/loginApi/StartGame',
        type: 'POST',
		data: JSON.stringify(loginViewModel),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $.cookie("game-data", JSON.stringify(data.gameId));
			$("#loginForm").submit();
        },
		error: function (exception) {
			showError(exception.responseJSON.Message);
        }
    });
}

function LoadGame() {
    var playerName = $("#playerName").val();
    $.ajax({
        url: '/api/loginApi/LoadGame',
        type: 'POST',
        data: JSON.stringify(playerName),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            $.cookie("game-data", JSON.stringify(data.gameId));
            $("#loginForm").submit();
        },
        error: function (exception) {
			showError(exception.responseJSON.Message);
        }
    });
}

function showError(message) {
	$('#errorModal').modal();
	$("#errorMessage").html(message);
}

function chooseBot() {
	betValue = document.getElementById("botsAmount").value;
	document.getElementById("imageBotsAmount").value = betValue;
}
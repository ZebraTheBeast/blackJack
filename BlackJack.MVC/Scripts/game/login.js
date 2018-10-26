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
	var startGameData = {
		playerName: $("#playerName").val(),
        botsAmount: $("#botsAmount").val(),
        isStartGame: true,
        isLoadGame: false
    };

    $.cookie("start-game", JSON.stringify(startGameData));
    $("#loginForm").submit();
}

function LoadGame() {
    var loadGameData = {
        playerName: $("#playerName").val(),
        isStartGame: false,
        isLoadGame: true
    };

    $.cookie("start-game", JSON.stringify(loadGameData));
    $("#loginForm").submit();
}

function showError(message) {
	$('#errorModal').modal();
	$("#errorMessage").html(message);
}

function chooseBot() {
	betValue = document.getElementById("botsAmount").value;
	document.getElementById("imageBotsAmount").value = betValue;
}
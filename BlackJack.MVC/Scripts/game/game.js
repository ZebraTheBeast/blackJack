$(document).ready(function () {

    var jsonData = $.cookie("start-game");
    var gameData = JSON.parse(jsonData);

    if (gameData.isStartGame) {
        StartGame(gameData.playerName, gameData.botsAmount);
    }

    if (gameData.isLoadGame) {
        LoadGame(gameData.playerName);
    }

    $("#placeBetButton").click(function (event) {
        event.preventDefault();
        Bet();
    });

    $("#drawButton").click(function (event) {
        event.preventDefault();
        Draw();
	});

    $("#standButton").click(function (event) {
        event.preventDefault();
        Stand();
    });

    $("#refreshButton").click(function (event) {
        event.preventDefault();
        location.reload();
    });

});

function StartGame(playerName, botsAmount) {
    var startGameView = {
        playerName: playerName,
        botsAmount: botsAmount
    }
    $.ajax({
        url: '/api/gameApi/StartMatch',
        type: 'POST',
        data: JSON.stringify(startGameView),
        contentType: "application/json;charset=utf-8",
        success: function (gameView) {
            disableDraw();
            WriteResponse(gameView);
            $.cookie("game-data", JSON.stringify(gameView.gameId));
        },
        error: function (exception) {
            showError(exception.responseJSON.Message);
        }
    });
}

function LoadGame(playerName) {
    
    $.ajax({
        url: '/api/gameApi/LoadMatch?playerName=',
        type: 'GET',
        data: playerName,
        contentType: "application/json;charset=utf-8",
        success: function (gameView) {
            WriteResponse(gameView);
            $.cookie("game-data", JSON.stringify(gameView.gameId));
        },
        error: function (exception) {
            showError(exception.responseJSON.Message);
        }
    });
}

function Stand() {
    var gameId = $.cookie("game-data");
    $.ajax({
        url: '/api/gameApi/Stand',
        type: 'POST',
        data: JSON.stringify(gameId),
        contentType: "application/json;charset=utf-8",
        success: function (gameViewModel) {
            disableDraw();
            WriteResponse(gameViewModel);
        },
        error: function (exception) {
            showError(exception.responseJSON.Message);
        }
    });
}

function GetGameViewModel() {
    var gameId = $.cookie("game-data");
    $.ajax({
		url: `/api/gameApi/GetGame`,
		type: 'POST',
		data: JSON.stringify(gameId),
        contentType: "application/json;charset=utf-8",
		success: function (gameViewModel) {
            WriteResponse(gameViewModel);
        },
        error: function (exception) {
            showError(exception.responseJSON.Message);
        }
    });
}

function Draw() {
    var gameId = $.cookie("game-data");
    $.ajax({
		url: '/api/gameApi/Draw',
        type: 'POST',
        data: JSON.stringify(gameId),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            WriteResponse(data);
        },
        error: function (exception) {
            showError(exception.responseJSON.Message);
        }
    });
}

function Bet() {

    var betViewModel = {
        BetValue: $("#betValue").val(),
        GameId: $.cookie("game-data")
    };

    $.ajax({
		url: '/api/gameApi/Bet',
        type: 'POST',
        data: JSON.stringify(betViewModel),
        contentType: "application/json;charset=utf-8",
        success: function (gameViewModel) {
            disableBet();
            WriteResponse(gameViewModel);
        },
        error: function (exception) {
            showError(exception.responseJSON.Message);
        }
    });
}

function WriteResponse(gameViewModel) {
    var dealerCards = "";
    var botResult = "";
    var statsResult = "";
    var humanCards = "";
	var humanResult = "";

    $("#dealerName").html(gameViewModel.dealer.name);

    $.each(gameViewModel.dealer.hand.cardsInHand, function (index, card) {
        dealerCards += "<li class='list-group-item'>" + card.title + " of " + card.suit + "</li>";
    });

    $("#dealerCards").html(dealerCards);
    $("#dealerValue").html(gameViewModel.dealer.hand.cardsInHandValue);
    $.each(gameViewModel.bots, function (index, bot) {
        statsResult += "<li class='list-group-item d-flex justify-content-between'>" + bot.name + "<span>" + bot.points + "</span></li>";

        botResult += "<div class = 'col-md-2'>" +
            "<h3>" + bot.name + "</h3>" +
			"<ul class = 'list-group'>";
		$.each(bot.hand.cardsInHand, function (index, card) {
            botResult += "<li class='list-group-item'>" + card.title + " of " + card.suit + "</li>";
        });
        botResult += "</ul>" +
			"<h4>Value: " + bot.hand.cardsInHandValue + " </h4>" +
            "<h4>Bet: " + bot.betValue + "</h4>" +
            "</div>";
    });
    statsResult += "<li class = 'list-group-item d-flex justify-content-between'>  " + gameViewModel.human.name + "<span>" + gameViewModel.human.points + "</span></li>";

    if (gameViewModel.deck !== null) {
        $("#cardsCount").html(gameViewModel.deck.length);
	}

	$.each(gameViewModel.human.hand.cardsInHand, function (index, card) {
		humanCards += "<li class='list-group-item'>" + card.title + " of " + card.suit + "</li>";
	});

	humanResult = `<div class="col-md-2">
		<h3>
			`+ gameViewModel.human.name + `
		</h3>
		<ul class="list-group">`+ humanCards +`</ul>
		<h4>Value: <span>`+ gameViewModel.human.hand.cardsInHandValue +`</span></h4>
		<h4>Bet: <span>`+ gameViewModel.human.betValue +`</span></h4>
	</div>`;

	$("#gameStat").html(gameViewModel.options);
	$("#playerStatsBlock").html(statsResult);

	$("#playerBlock").html(botResult + humanResult);

    $("#betValue").attr("max", gameViewModel.human.points);

	if (gameViewModel.human.hand.cardsInHandValue >= 21) {
        disableDraw();
    }

    if (gameViewModel.human.betValue === 0) {
        disableDraw();
    }

	if (gameViewModel.dealer.hand.cardsInHandValue === 21) {
        disableDraw();
    }

	if ((gameViewModel.human.hand.cardsInHandValue.length !== 0) && (gameViewModel.human.betValue !== 0)) {
        disableBet();
    }

}

function showError(message) {
    $('#errorModal').modal();
    $("#errorMessage").html(message);
}

function placeBet() {
    betValue = document.getElementById("betValue").value;
    document.getElementById("imageBetValue").value = betValue;
}

function disableDraw() {
    $("#drawButton").attr("disabled", true);
    $("#standButton").attr("disabled", true);
    $("#betValue").attr("disabled", false);
    $("#betValue").val(10);
    $("#imageBetValue").val(10);
    $("#imageBetValue").attr("disabled", false);
    $("#placeBetButton").attr("disabled", false);
}

function disableBet() {
    $("#drawButton").attr("disabled", false);
    $("#standButton").attr("disabled", false);
    $("#betValue").attr("disabled", true);
    $("#imageBetValue").attr("disabled", true);
    $("#placeBetButton").attr("disabled", true);
}

$(document).ready(function () {
    GetGameViewModel();
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

function Stand() {
    var humanId = JSON.parse($.cookie("human-data"));
    $.ajax({
        url: '/api/values/Stand',
        type: 'POST',
        data: JSON.stringify(humanId),
        contentType: "application/json;charset=utf-8",
        success: function (gameViewModel) {
            disableDraw();
            WriteResponse(gameViewModel);
        },
        error: function (exception) {
            showError(exception.responseJSON.Message);
        }
    });
};

function GetGameViewModel() {
    var humanId = JSON.parse($.cookie("human-data"));
    $.ajax({
        url: `/api/values/GetGameViewModel`,
		type: 'POST',
		data: JSON.stringify(humanId),
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
    var humanId = JSON.parse($.cookie("human-data"));
    $.ajax({
        url: '/api/values/Draw',
        type: 'POST',
        data: JSON.stringify(humanId),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            WriteResponse(data);
        },
        error: function (exception) {
            showError(exception.responseJSON.Message);
        }
    });
};

function Bet() {

    var betViewModel = {
        BetValue: $("#betValue").val(),
        HumanId: JSON.parse($.cookie("human-data"))
    };

    $.ajax({
        url: '/api/values/Bet',
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
};

function WriteResponse(gameViewModel) {
    var dealerCards = "";
    var botResult = "";
    var statsResult = "";
    var humanCards = "";
    $.cookie("deck-data", JSON.stringify(gameViewModel.deck));

    $("#dealerName").html(gameViewModel.dealer.name);

    $.each(gameViewModel.dealer.hand.cardList, function (index, card) {
        dealerCards += "<li class='list-group-item'>" + card.title + " of " + card.color + "</li>";
    });

    $("#dealerCards").html(dealerCards);
    $("#dealerValue").html(gameViewModel.dealer.hand.cardListValue);
    $.each(gameViewModel.bots, function (index, bot) {
        statsResult += "<li class='list-group-item d-flex justify-content-between'>" + bot.name + "<span>" + bot.points + "</span></li>";

        botResult += "<div class = 'col-md-4'>" +
            "<h3>" + bot.name + "</h3>" +
            "<ul class = 'list-group'>";
        $.each(bot.hand.cardList, function (index, card) {
            botResult += "<li class='list-group-item'>" + card.title + " of " + card.color + "</li>";
        });
        botResult += "</ul>" +
            "<h4>Value: " + bot.hand.cardListValue + " </h4>" +
            "<h4>Bet: " + bot.hand.betValue + "</h4>" +
            "</div>";
    });
    statsResult += "<li class = 'list-group-item d-flex justify-content-between'>  " + gameViewModel.human.name + "<span>" + gameViewModel.human.points + "</span></li>";

    if (gameViewModel.deck !== null) {
        $("#cardsCount").html(gameViewModel.deck.length);
    }

    $.each(gameViewModel.human.hand.cardList, function (index, card) {
        humanCards += "<li class='list-group-item'>" + card.title + " of " + card.color + "</li>";
    });

    $("#humanName").html(gameViewModel.human.name);
    $("#humanCards").html(humanCards);
    $("#humanValue").html(gameViewModel.human.hand.cardListValue);
    $("#humanBet").html(gameViewModel.human.hand.betValue);

    $("#gameStat").html(gameViewModel.options);
    $("#playerStatsBlock").html(statsResult);
    $("#botBlock").html(botResult);

    $("#betValue").attr("max", gameViewModel.human.points);

    if (gameViewModel.human.hand.cardListValue >= 21) {
        disableDraw();
    }

    if (gameViewModel.human.hand.betValue === 0) {
        disableDraw();
    }

    if (gameViewModel.dealer.hand.cardListValue === 21) {
        disableDraw();
    }

    if ((gameViewModel.human.hand.cardList.length !== 0) && (gameViewModel.human.hand.betValue !== 0)) {
        disableBet();
    }

}

function showError(message) {
    $('#errorModal').modal();
    $("#errorMessage").html(message);
}
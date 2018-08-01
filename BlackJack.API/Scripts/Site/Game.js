$(document).ready(function () {
    1
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
});

function Stand() {
    var deck = JSON.parse($.cookie("deck-data"));

    $.ajax({
        url: '/api/values/Stand',
        type: 'POST',
        data: JSON.stringify(deck),
        contentType: "application/json;charset=utf-8",
        success: function (gameViewModel) {
            disableDraw();
            WriteResponse(gameViewModel);
        }
    });
};

function GetGameViewModel() {
    $.ajax({
        url: '/api/values/GetGameViewModel',
        type: 'GET',
        contentType: "application/json;charset=utf-8",
        success: function (gameViewModel) {
            WriteResponse(gameViewModel);
        }
    });
}

function Draw() {

    var drawViewModel = {
        HumanId: $("#humanId").val(),
        Deck: JSON.parse($.cookie("deck-data"))
    };

    $.ajax({
        url: '/api/values/Draw',
        type: 'POST',
        data: JSON.stringify(drawViewModel),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            WriteResponse(data);
        }
    });
};

function Bet() {
    disableBet();
    var betViewModel = {
        HumanId: $("#humanId").val(),
        BetValue: $("#betValue").val()
    };
    $.ajax({
        url: '/api/values/Bet',
        type: 'POST',
        data: JSON.stringify(betViewModel),
        contentType: "application/json;charset=utf-8",
        success: function (gameViewModel) {
            WriteResponse(gameViewModel);
        }
    });
};

function WriteResponse(gameViewModel) {
    var dealerCards = "";
    var botResult = "";
    var statsResult = "";
    var humanCards = "";
    $.cookie("deck-data", JSON.stringify(gameViewModel.Deck));

    $("#dealerName").html(gameViewModel.Dealer.Name);

    $.each(gameViewModel.Dealer.Hand.CardList, function (index, card) {
        dealerCards += "<li class='list-group-item'>" + card.Title + " of " + card.Color + "</li>";
    });

    $("#dealerCards").html(dealerCards);
    $("#dealerValue").html(gameViewModel.Dealer.Hand.CardListValue);
    $.each(gameViewModel.Bots, function (index, bot) {
        statsResult += "<li class='list-group-item'>" + bot.Name + " " + bot.Points + "</li>";

        botResult += "<div class = 'col-md-4'>" +
            "<h3>" + bot.Name + "</h3>" +
            "<ul class = 'list-group'>";
        $.each(bot.Hand.CardList, function (index, card) {
            botResult += "<li class='list-group-item'>" + card.Title + " of " + card.Color + "</li>";
        });
        botResult += "</ul>" +
            "<h4>Value: " + bot.Hand.CardListValue + " </h4>" +
            "<h4>Bet: " + bot.Hand.Points + "</h4>" +
            "</div>";
    });
    statsResult += "<li class = 'list-group-item'> " + gameViewModel.Human.Name + " " + gameViewModel.Human.Points + "</li>";

    if (gameViewModel.Deck != null) {
        $("#cardsCount").html(gameViewModel.Deck.length);
    }

    $.each(gameViewModel.Human.Hand.CardList, function (index, card) {
        humanCards += "<li class='list-group-item'>" + card.Title + " of " + card.Color + "</li>";
    });

    $("#humanName").html(gameViewModel.Human.Name);
    $("#humanCards").html(humanCards);
    $("#humanValue").html(gameViewModel.Human.Hand.CardListValue);
    $("#humanBet").html(gameViewModel.Human.Hand.Points);
    $("#humanId").val(gameViewModel.Human.Id);

    $("#gameStat").html(gameViewModel.Options);
    $("#playerStatsBlock").html(statsResult);
    $("#botBlock").html(botResult);

    $("#betValue").attr("max", gameViewModel.Human.Points);

    if (gameViewModel.Human.Hand.CardListValue >= 21){
        disableDraw();
    }

    if (gameViewModel.Human.Hand.Points == 0){
        disableDraw();
    }

    if (gameViewModel.Dealer.Hand.CardListValue == 21) {
        disableDraw();
    }

    if ((gameViewModel.Human.Hand.CardList.length != 0) && (gameViewModel.Human.Hand.Points != 0)) {
        disableBet();
    }

}
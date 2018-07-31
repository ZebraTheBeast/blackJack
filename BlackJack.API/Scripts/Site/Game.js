﻿$(document).ready(function () {
    StartGame();
    disableDraw();

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


function StartGame() {
    var playerName = JSON.parse($.cookie("player-data"));
    $.ajax({
        url: '/api/values/StartGame',
        type: 'POST',
        data: JSON.stringify(playerName),
        contentType: "application/json;charset=utf-8",
        success: function (gameViewModel) {
            WriteResponse(gameViewModel);
        }
    });
}

function WriteResponse(gameViewModel) {
    var dealerResult;
    var botResult = "";
    var statsResult;
    var humanResul;
    var gameOption;
    $.cookie("deck-data", JSON.stringify(gameViewModel.Deck));
    
    dealerResult = "<h2>" + gameViewModel.Dealer.Name + "</h2>";
    dealerResult += "<ul class = 'list-group'>";
    $.each(gameViewModel.Dealer.Hand.CardList, function (index, card) {
        dealerResult += "<li class='list-group-item'>" + card.Title + " of " + card.Color + "</li>";
    });
    dealerResult += "</ul>" +
        "<h4> Value:" + gameViewModel.Dealer.Hand.CardListValue + "</h4>";

    statsResult = "<ul class = 'list-group'>";
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

    statsResult += "<li class = 'list-group-item'> " + gameViewModel.Human.Name + " " + gameViewModel.Human.Points + "</li>" +
        "</ul>";

    if (gameViewModel.Deck != null) {
        statsResult += "<h3>Cards in deck: " + gameViewModel.Deck.length + "</h3>";
    }

    humanResult = "<h3>" + gameViewModel.Human.Name + "</h3>" +
        "<ul class='list-group'>";
    $.each(gameViewModel.Human.Hand.CardList, function (index, card) {
        humanResult += "<li class='list-group-item'>" + card.Title + " of " + card.Color + "</li>";
    });
    humanResult += "<h4>Value: " + gameViewModel.Human.Hand.CardListValue + " </h4>" +
        "<h4>Bet:" + gameViewModel.Human.Hand.Points + "</h4>";
    humanResult += "<input type='hidden' value = '" + gameViewModel.Human.Id + "' id = 'humanId'>";

    gameOption = " <h1 class='alert alert-info'> " + gameViewModel.Options + "</h1>";

    $("#gameStat").html(gameOption);
    $("#playerStatsBlock").html(statsResult);
    $("#botBlock").html(botResult);
    $("#humanBlock").html(humanResult);
    $("#dealerBlock").html(dealerResult);

    $("#betValue").attr("max", gameViewModel.Human.Points);

    if ((gameViewModel.Human.Hand.CardListValue >= 21)
        || (gameViewModel.Dealer.Hand.CardListValue == 21)) {
        disableDraw();
    }
}
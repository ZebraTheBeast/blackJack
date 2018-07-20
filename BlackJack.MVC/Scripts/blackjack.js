function placeBet() {
    betValue = document.getElementById("betValue").value;
    document.getElementById("imageBetValue").value = betValue;
}

function disableButtons() {
    draw = document.getElementById("drawButton");
    botTurn = document.getElementById("botTurnButton");
    dealerTurn = document.getElementById("dealerTurnButton");
    endTurn = document.getElementById("endTurnButton");
    betValue = document.getElementById("betValue");
    imageBetValue = document.getElementById("imageBetValue");
    placeBetButton = document.getElementById("placeBetButton");

    playerPoits = document.getElementById("playerPoints").value;
    playerValue = document.getElementById("playerValue");
    idButton = document.getElementById("idButton").value;

    if (playerValue.value >= 21) {
        draw.disabled = true;
    }



    if (idButton == 0) {
        draw.disabled = true;
        botTurn.disabled = true;
        dealerTurn.disabled = true;
        endTurn.disabled = true;
    }

    if (idButton == 1) {
        betValue.disabled = true;
        imageBetValue.disabled = true;
        dealerTurn.disabled = true;
        endTurn.disabled = true;
        placeBetButton.disabled = true;
    }

    if (idButton == 2) {
        botTurn.disabled = true;
        draw.disabled = true;
        endTurn.disabled = true;
        betValue.disabled = true;
        imageBetValue.disabled = true;
        placeBetButton.disabled = true;
    }

    if (idButton == 3) {
        botTurn.disabled = true;
        draw.disabled = true;
        dealerTurn.disabled = true;
        betValue.disabled = true;
        imageBetValue.disabled = true;
        placeBetButton.disabled = true;
    }
   
    if (playerPoits < 10) {
        betValue.disabled = true;
        imageBetValue.disabled = true;
        placeBetButton.disabled = true;
    }
    
    var objDiv = document.getElementById("GameStat");
    objDiv.scrollTop = objDiv.scrollHeight;

}

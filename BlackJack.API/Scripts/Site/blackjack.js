﻿function placeBet() {
    betValue = document.getElementById("betValue").value;
    document.getElementById("imageBetValue").value = betValue;
}

function disableButtons() {
    draw = document.getElementById("drawButton");
    standButton = document.getElementById("standButton");

    betValue = document.getElementById("betValue");
    imageBetValue = document.getElementById("imageBetValue");
    placeBetButton = document.getElementById("placeBetButton");

    playerPoits = document.getElementById("playerPoints");
   
    idButton = document.getElementById("idButton");

    if (idButton == 0) {
        draw.disabled = true;
        standButton.disabled = true;
    }

    if (idButton == 1) {
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

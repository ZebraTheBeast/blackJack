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

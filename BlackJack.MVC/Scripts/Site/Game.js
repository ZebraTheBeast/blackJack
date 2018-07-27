$(document).ready(function () {
    GetTestSting();
});

function GetTestSting() {
    $.ajax({
        url: '/api/values',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            WriteResponse(data);
        },
        error: function (x, y, z) {
            console.log(x + '\n' + y + '\n' + z);
        }
    });
}

function WriteResponse(gameViewModel) {
    var strResult = "<h1>" + gameViewModel + "</h1>";
    $("#dealerBlock").html(strResult);
}
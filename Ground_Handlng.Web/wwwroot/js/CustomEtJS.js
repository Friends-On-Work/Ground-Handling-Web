
$(document).on('click', '#search', function () {

    var valid = true;
    var errorMessages = "<ul>";
    var checkin = $("#CheckInDate").val();
    var checkout = $("#CheckOutDate").val();

    if (checkin === "") {
        $("#CheckInDate").addClass("error");
        errorMessages = errorMessages + "<li>Please enter your Check In Date</li>";
        valid = false;
    }
    else {
        $("#CheckInDate").removeClass("error");
    }
    if (checkout === "") {
        $("#CheckOutDate").addClass("error");
        errorMessages = errorMessages + "<li>Please enter your Check Out Date</li>";
        valid = false;
    }
    else {
        $("#CheckOutDate").removeClass("error");
    }
    errorMessages = errorMessages + "</ul>";

    if (valid) {
        $("#divLoading").show();
        return true;
       
    }
    else {
        $('.errorDisplay').removeClass('d-none');
        $('.errorDisplay').html(errorMessages);
        return false;
    }
   
});

$(document).on('click', '.btn-minusAdult', function () {
    var Adult = parseInt($("#NumberOfAdult").val());
    if (Adult > 1) {
        $("#NumberOfAdult").val(Adult-1);
    }
});
$(document).on('click', '.btn-plusAdult', function () {
    var Adult = parseInt($("#NumberOfAdult").val());
    if (Adult < 20) {
        $("#NumberOfAdult").val(Adult+1);
    }
});
$(document).on('click', '.btn-minusChilds', function () {
    var Childs = parseInt($("#NumberOfChild").val());
    if (Childs > 0) {
        $("#NumberOfChild").val(Childs - 1);
    }
});
$(document).on('click', '.btn-plusChilds', function () {
    var Childs = parseInt($("#NumberOfChild").val());
    if (Childs <20) {
        $("#NumberOfChild").val(Childs + 1);
    }
});
$(document).on('click', '.btn-minusRooms', function () {
    var NumberOfRooms = parseInt($("#NumberOfRooms").val());
    if (NumberOfRooms > 1) {
        $("#NumberOfRooms").val(NumberOfRooms - 1);
    }
});
$(document).on('click', '.btn-plusRooms', function () {
    var NumberOfRooms = parseInt($("#NumberOfRooms").val());
    if (NumberOfRooms <9) {
        $("#NumberOfRooms").val(NumberOfRooms + 1);
    }
});

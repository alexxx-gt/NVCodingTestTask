$(function () {
    //request in process flag - to avoid duplicate asynchronous requests to the controller methods
    window.requestInProcess = false;

    $('#Name').focusout(send);
    $('#Email').focusout(send);
    $('#Avatar').focusout(send);
    $('#SkypeLogin').focusout(send);
    $('#Signature').focusout(send);

    //Hiding submit button programmatically
    $('#hiddenSubmitButton').hide();

});


function send() {
    //Getting values from respective fields
    var userName = $('#Name').val();
    var eMail = $('#Email').val();
    var avatar = $('#Avatar').val();
    var skypeLogin = $('#SkypeLogin').val();
    var signature = $('#Signature').val();
    var id = $('#Id').val();

    //trying to validate form
    var form = document.forms[0];
    $(form).validate();

    //performing ajax query
    if ($(form).valid() && window.requestInProcess == false) {
        window.requestInProcess = true;
        var jqxhr = $.ajax(
            {
                url: "/Home/AjaxSaveData",
                type: "POST",
                data: "userName=" +
                userName +
                "&email=" +
                eMail +
                "&avatar=" +
                avatar +
                "&skypeLogin=" +
                skypeLogin +
                "&signature=" +
                signature +
                "&id=" + id,
                success: function (jsonData) {
                    console.log(jsonData);
                    if (jsonData.justAddedId != -1) {
                        $('#Id').val(jsonData.justAddedId);
                    }
                    if (jsonData.success == "true") {
                        $('#successIndicator').html("Data have been saved successfully").css("color", "green");
                    } else {
                        $('#successIndicator').html("Error occured while saving data").css("color", "green");
                    }
                    window.requestInProcess = false;
                },
                error: function () {
                    console.log("Failed!");
                    $('#successIndicator').html("Error occured while saving data").css("color", "green");
                    window.requestInProcess = false;
                }
            });
    }


};







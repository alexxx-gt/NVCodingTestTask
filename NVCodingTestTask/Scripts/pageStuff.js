$(function () {
    window.requestInProcess = false;

    $('#Name').focusout(send);
    $('#Email').focusout(send);
    $('#Avatar').focusout(send);
    $('#SkypeLogin').focusout(send);
    $('#Signature').focusout(send);

    $('#hiddenSubmitButton').hide();


});


function send() {
    var userName = $('#Name').val();
    var eMail = $('#Email').val();
    var avatar = $('#Avatar').val();
    var skypeLogin = $('#SkypeLogin').val();
    var signature = $('#Signature').val();
    var id = $('#Id').val();

    //console.log(id);

    //var formValid = this.validate().form();

    //$('#hiddenSubmitButton').click();

    //var errorsCount = 0;

    //var textDanger = $('.text-danger');

    //console.log(textDanger);

    //if (textDanger.length > 0) {
    //    for (i = 0; i < textDanger.length; i++) {
    //        console.log(textDanger[i]);
    //        if (textDanger[i].childElementCount > 0) {
    //            console.log(textDanger[i].childElementCount);
    //            errorsCount++;
    //        } else {
    //            console.log(textDanger[i].childElementCount);
    //        }
    //    }
    //}

    var form = document.forms[0];
    $(form).validate();

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







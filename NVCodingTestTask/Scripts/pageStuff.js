$(function () {
    //request in process flag - to avoid duplicate asynchronous requests to the controller methods
    window.requestInProcess = false;
    window.fileRequestInProcess = false;

    $('#Name').focusout(send);
    $('#Email').focusout(send);
    $('#uploadFile').focusout(setImage);
    $('#SkypeLogin').focusout(send);
    $('#Signature').focusout(send);

    //Hiding submit button programmatically
    $('#hiddenSubmitButton').hide();

});


function send() {
    //Getting values from respective fields
    var userName = $('#Name').val();
    var eMail = $('#Email').val();
    //var avatar = $('#Avatar').val();
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

function setImage() {
    var input = $('#uploadFile');

    var id = $('#Id').val();

    if (window.FormData !== undefined) {
        var data = new FormData();

        data.append("file", input.prop('files')[0]);
        data.append("id", id);

        if (input.prop('files').length > 0 && window.fileRequestInProcess == false) {
            window.fileRequestInProcess = true;
            $.ajax({
                type: "POST",
                url: '/Home/AjaxSaveImage',
                contentType: false,
                processData: false,
                data: data,
                success: function(result) {
                    $('#successIndicator').html("Image have been saved successfully").css("color", "green");
                    window.fileRequestInProcess = false;
                    
                    //in case of editing - change src, else create img tag
                    if ($('#avatarImage').length) {
                        $('#avatarImage').attr('src',
                            '/Files/avatar_id_' +
                            id +
                            input.prop('files')[0].name.substring(input.prop('files')[0].name.lastIndexOf(".")));
                    } else {
                        var addstring = "<br /><img src=\"/Files/avatar_id_" + id +
                            input.prop('files')[0].name.substring(input.prop('files')[0].name.lastIndexOf(".")) +
                            "\" height=\"100\" width=\"100\">";
                        console.log(addstring);
                        $('#uploadFile').after(addstring);
                        $('#avatarAbsent').remove();
                    }
                },
                error: function(xhr, status, p3) {
                    $('#successIndicator').html("Error occured while saving image.").css("color", "red");
                    window.fileRequestInProcess = false;
                }
            });
        }
    } else {
        $('#successIndicator').html("Error occured while saving image. You cannot save image, because your browser is too old.").css("color", "red");
    }


}







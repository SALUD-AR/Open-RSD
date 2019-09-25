
function UpdateSecuence(tableName) {
    var dataToPost = {
        TableName: tableName,
        Sequence: $('#' + tableName).val(),
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    //////////////////////////////////////
    //console.log(dataToPost);
    //alert($('#Enabled').is(':checked'));
    //alert("Registro en Consola");
    //return;
    //////////////////////////////////////

    var errorLabel = $('#errorLabel');
    
    var loc = window.rootUrl + "Admin/Database/UpdateSecuense";
    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {
            if (data.success) {
                
            }
            
        },
        error: function (request, status, error) {
            console.log("request:" + request.responseText);
            alert(request.responseText);
        }
    });
}
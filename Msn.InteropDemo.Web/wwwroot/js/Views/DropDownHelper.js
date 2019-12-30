
function dropDownFill(dropDownObjId, autocompleteData, selectedOptionId, defaultText) {

    if (selectedOptionId === undefined) {
        selectedOptionId = '';
    }
    if (defaultText === undefined) {
        defaultText = '-- Seleccione --';
    }

    var options = "<option value=''>" + defaultText + "</option>";
    for (var i = 0; i < autocompleteData.length; i++) {

        options += '<option value="' + autocompleteData[i].id + '"' + (autocompleteData[i].id == selectedOptionId ? " selected " : "") + '>' + autocompleteData[i].name + '</option>';
    }

    $("select#" + dropDownObjId).html(options)

}

function dropDownFillNoDefaultItem(dropDownObjId, autocompleteData, selectedOptionId) {

    if (selectedOptionId === undefined) {
        selectedOptionId = '';
    }
    
    var options = "";
    for (var i = 0; i < autocompleteData.length; i++) {

        options += '<option value="' + autocompleteData[i].id + '"' + (autocompleteData[i].id == selectedOptionId ? " selected " : "") + '>' + autocompleteData[i].name + '</option>';
    }

    $("select#" + dropDownObjId).html(options)

}



function dropDownFillMultiple(dropDownObjId, autocompleteData, selectedOptionsArray) {

    if (selectedOptionsArray === undefined) {
        selectedOptionsArray = [];
    }
    
    var options = '';
    for (var i = 0; i < autocompleteData.length; i++) {
        
        options += '<option value="' + autocompleteData[i].Id + '"' + (jQuery.inArray(autocompleteData[i].Id, selectedOptionsArray) > -1 ? " selected " : "") + '>' + autocompleteData[i].Name + '</option>';
    }
    
    $("select#" + dropDownObjId).html(options)
    
}

function clearDropDown(dropDownObjId, defaultText, disable) {

    if (defaultText === undefined) {
        defaultText = '-- Seleccione --';
    }
    if (disable === undefined) {
        disable = false;
    }

    var options = "<option value=''>" + defaultText + "</option>";
    $("#" + dropDownObjId).html(options)

    if (disable) {
        $('#' + dropDownObjId).prop('disabled', 'disabled');
    }

    //document.getElementById(id).options.length = 0;
}
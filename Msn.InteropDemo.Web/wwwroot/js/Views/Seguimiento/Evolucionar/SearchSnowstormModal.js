﻿
//On document Ready
$(function () {

    $('#searchSnowstormModal').on('shown.bs.modal', function (e) {
        var textSearch = $('#textSearch');
        textSearch.focus();
    })

    $('#modalMapeoCie10').on('shown.bs.modal', function (e) {
        resetCheckBoxes();
    })

    SearchType = {
        HALLAZGOS: {
            Value: 1,
            ECL: "<404684003",
            Description: "|hallazgo clínico (hallazgo)|"
        },
        VACUNAS: {
            Value: 2,
            ECL: "^2281000221106",
            Description: "|conjunto de referencias simples de inmunizaciones notificables (metadato fundacional)|"
        },
        MEDICAMENTOS: {
            Value: 3,
            ECL: "<781405001",
            Description: "|envase de producto medicinal (producto)|"
        }
    };

    searchTypeSelected = "";
});


function showSearchSnowstormHallagos() {
    showSearchSnowstormModal(SearchType.HALLAZGOS);
}


function showSearchSnowstormMedicamentos() {
    showSearchSnowstormModal(SearchType.MEDICAMENTOS);
}


function showSearchSnowstormVacunas() {
    showSearchSnowstormModal(SearchType.VACUNAS);
}


function showSearchSnowstormModal(st) {
    var seachSnowstormResultContainer = $('#seachSnowstormResultContainer');
    var theModal = $('#searchSnowstormModal');
    var textSearch = $('#textSearch');

    textSearch.val('');
    seachSnowstormResultContainer.css('display', 'none');

    searchTypeSelected = st;

    if (searchTypeSelected == SearchType.HALLAZGOS) {
        theModal.find('.modal-title').text('Hallazgos');
    } else if (searchTypeSelected == SearchType.VACUNAS) {
        theModal.find('.modal-title').text('Vacunas');
    } else if (searchTypeSelected == SearchType.MEDICAMENTOS) {
        theModal.find('.modal-title').text('Medicamentos');
    }

    theModal.modal('show');
}


function seachSnowstorm() {
    var textSearch = $('#textSearch');
    var term = textSearch.val();
    if (!term) {
        return;
    }

    var loc = window.rootUrl + "Seguimiento/Evolucionar/SearchByExpressionTerm";
    var seachSnowstormResultContainer = $('#seachSnowstormResultContainer');
    var btnSeachSnowstorm = $('#btnSeachSnowstorm');
    var preHtml = btnSeachSnowstorm.html();

    btnSeachSnowstorm.html('<span class="fas fa-spinner fa-spin"></span><span> Buscar</span>');
    btnSeachSnowstorm.addClass('disabled');

    var dataToPost = {
        term: term,
        expression: searchTypeSelected.ECL,
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {
            seachSnowstormResultContainer.html(data.table);
            seachSnowstormResultContainer.fadeIn('slow');

            btnSeachSnowstorm.html(preHtml);
            btnSeachSnowstorm.removeClass('disabled');
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });
}


function snowstomResultItemSelected(obj) {
    var table;
    var trHTML = '';

    if (searchTypeSelected == SearchType.HALLAZGOS) {
        table = $('#tableHallazgos');
    }
    else if (searchTypeSelected == SearchType.MEDICAMENTOS) {
        table = $('#tableMedicamentos');
    }
    else if (searchTypeSelected == SearchType.VACUNAS) {
        table = $('#tableVacunas');
    }


    trHTML += '<tr id="' + obj.dataset.conceptid + '" class="tr-evolucion">';

    if (searchTypeSelected == SearchType.HALLAZGOS) {
        trHTML += '<td>' +
            '<div id="sctTerm' + obj.dataset.conceptid +   '" class="sctTerm">' + obj.dataset.term + '</div>' +
            '<div class="text-muted small">SctId: ' + obj.dataset.conceptid + ' --> ' +
            '<span id="mapContainer' + obj.dataset.conceptid + '"><a href="#!" onClick="mapToCie10(' + obj.dataset.conceptid + ')">Mapear a CIE10</a></span>' +
            '</div> ' +
            '</td>';
    }
    else {
        trHTML += '<td>' + obj.dataset.term + '</td>';
    }

    trHTML += '<td class="text-right">';
    trHTML += '<div class="deleteItemContent">';
    trHTML += '<i class="fas fa-times" onClick="deleteItem(' + obj.dataset.conceptid + ')">&nbsp;</i>';
    trHTML += '</div>';
    trHTML += '</td>';
    trHTML += '</tr>';

    table.append(trHTML);
}


function mapToCie10(id) {
    var loc = window.rootUrl + "Seguimiento/Evolucionar/GetMapeoCie10";
    var dataToPost = {
        conceptId: id,
        sexo: $('#pacienteSexo').val(),
        edad: $('#pacienteEdad').val(),
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    $.ajax({
        url: loc,
        type: 'POST',
        data: dataToPost,
        dataType: 'json',
        cache: false,
        success: function (data) {

            if (data.count == 1) {

                var mappedItems = {
                    containerId: id,
                    items: []
                };
                mappedItems.items.push(data.item);

                mapToCie10Items(id, mappedItems.items);

            } else if (data.count > 1) {

                $('#sctConceptIdtoMap').val(id);
                var theModal = $('#modalMapeoCie10');

                $('#sctIdToMap').text(id);
                $('#sctTermToMap').text($('#sctTerm' + id).text());

                $('#modalMapeoCie10GridContainer').css('display', 'none');
                $('#modalMapeoCie10GridContainer').html(data.table);
                theModal.modal('show');
                $('#modalMapeoCie10GridContainer').show('slow');

            } else {
                var container = $('#mapContainer' + id);
                container.text('CIE10: No Encontrado');
                container.addClass('text-danger');
            }
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });
}

function resetCheckBoxes() {
    $('.swith_mapping_cie10').bootstrapToggle()
}

function mapToCie10MultiplesItems() {
    var sctConceptId = $('#sctConceptIdtoMap').val();

    var mappedItems = {
        containerId: sctConceptId,
        items: []
    };

    $('#tableMultipleMapeoCie10 > tbody > tr').each(function () {

        var codCie10 = this.id;
        //si está seleccionado el checkBox en "Mapear"
        if ($(this).find('#checkbox-' + codCie10).is(':checked')) {
            var item = {
                subcategoriaId: codCie10,
                subcategoriaNombre: $(this).find('div.SubcategoriaNombre').text()
            }
            mappedItems.items.push(item);    
        }
    });

    mapToCie10Items(mappedItems.containerId, mappedItems.items);
}

function mapToCie10Items(containerId, items) {
    var container = $('#mapContainer' + containerId);
    container.text('CIE10: ');

    items.forEach(function (item) {
        var nexElement = '<span id="' + item.subcategoriaId + '" class="cie10MappedCode text-success">' + item.subcategoriaId + ': </span>';
        nexElement += '<span id="cie10MappedText-' + item.subcategoriaId + '" class="cie10MappedText text-success">' + item.subcategoriaNombre + '</span>';
        nexElement += '<span> | </span>';
        $(nexElement).insertAfter(container);
    });
}

function deleteItem(rowId) {
    var tr = $('#' + rowId);
    tr.fadeOut(300, function () {
        tr.remove();
    });


};




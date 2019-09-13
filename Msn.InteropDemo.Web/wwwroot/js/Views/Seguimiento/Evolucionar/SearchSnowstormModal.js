
//On document Ready
$(function () {

    $('#searchSnowstormModal').on('shown.bs.modal', function (e) {
        var textSearch = $('#textSearch');
        textSearch.focus();
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
            '<div class="sctTerm">' + obj.dataset.term + '</div>' +
            '<div class="text-muted small">SctId: ' + obj.dataset.conceptid + ' --> ' +
            '<span id="mapContainer' + obj.dataset.conceptid + '"><a href="#!" onClick="mapToCie10(' + obj.dataset.conceptid + ')">Maperar a CIE10</a></span>' +
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

    var loc = window.rootUrl + "Seguimiento/Evolucionar/GetMapeoCie10?conceptId=" + id;

    $.ajax({
        url: loc,
        type: 'GET',
        dataType: 'json',
        cache: false,
        success: function (data) {

            var container = $('#mapContainer' + id);

            if (data.count == 1) {
                mapToCie10Item(id, data.items[0]);
            } else if (data.count > 1) {
                showCie10MapperSelectorModal(id, data);
            } else {
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


function showCie10MapperSelectorModal(containerId, data) {
    var theModal = $('#modalMapeoCie10');
    theModal.modal('show');

    var trHTML = '';
    trHTML += '<table class="table table-hover table-responsive-sm">';
    trHTML += '<tr class="font-weight-light"><th>Recomendación para el mapeo</th><th>&nbsp;</th><th>Código CIE10</th><th>&nbsp;</th></tr>'

    $.each(data.items, function (i, item) {
        trHTML += '<tr id="' + item.subcategoriaId + '" class="font-weight-light">'
        trHTML += '<td><div>' + item.mapAdvice + '</div></td>';
        trHTML += '<td class="text-center"><div>' + item.subcategoriaNombre + '</div></td>';
        trHTML += '<td class="text-center"><div>' + item.subcategoriaId + '</div></td>';

        trHTML += '<td class="text-center">';
        trHTML += '     <button class="btn btn-outline-primary" data-dismiss="modal" onclick="javascript:mapToCie10Item(' + containerId + ',{subcategoriaId:\'' + item.subcategoriaId + '\', subcategoriaNombre:\'' + item.subcategoriaNombre + '\'})"> <i class="fas fa-arrow-right"></i></button>';
        trHTML += '</td>';

        trHTML += '</tr>';
    });

    trHTML += '</table>';

    //console.log(data);
    //console.log(trHTML);

    $('#modalMapeoCie10GridContainer').html(trHTML);

}

function mapToCie10Item(containerId, item) {
    var container = $('#mapContainer' + containerId);
    var nexElement = '<span class="cie10MappedText text-success">' + item.subcategoriaNombre + '</span>';
    var str = 'CIE10: ' + item.subcategoriaId + ' - ';

    container.hide('slow', function () {
        container.attr('id', item.subcategoriaId);
        container.text(str);
        container.addClass('cie10MappedId text-success');
        container.show('slow');
        $(nexElement).insertAfter(container).hide().show('slow');
    });
}


function deleteItem(rowId) {
    var tr = $('#' + rowId);
    tr.fadeOut(300, function () {
        tr.remove();
    });


};




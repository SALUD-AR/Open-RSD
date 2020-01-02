
$(function () {
    $('#vacuna-tab-link').on('shown.bs.tab', function (e) {
        loadGridVacunas();
    })

    $('#aplicarVacunaFecha').datetimepicker({
        format: 'DD/MM/YYYY'
    });
});


function loadGridVacunas() {
    var pacienteId = $('#PacienteId').val();
    var container = $('#vacunasGridContainer');
    var loc = window.rootUrl + "Seguimiento/Evolucionar/GetGridVacunas";

    var dataToPost = {
        PacienteId: pacienteId,
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {
            container.html(data.table);
            container.fadeIn('slow');
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });
}


function showEsquemaVacunaAplicacionModal(evolucionAplicacionVacunaId, vacunaSctId) {
    //var container = $('#vacunasGridContainer');
    var loc = window.rootUrl + "Seguimiento/Evolucionar/GetEsquemasVacunasAsync";

    $('#aplicarVacunaCurrentEvolucionVacunaId').val(evolucionAplicacionVacunaId);

    var dataToPost = {
        evolucionAplicacionVacunaId: evolucionAplicacionVacunaId,
        vacunaSctId: vacunaSctId,
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {

            $('#vacunaAplicacionSctId').text(data.vacuna.sctConceptId);
            $('#vacunaAplicacionSctTerm').text(data.vacuna.sctDescriptionTerm);
            $('#aplicarVacunaFechaInput').val(getFullCurrentDateAR());

            dropDownFill("selectAplicarVacunaEsquemaId", data.esquemaItems, null, "-- Seleccione Esquema --")

            var theModal = $('#aplicarVacunaModal');
            theModal.modal('show');

        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });
}


function aplicarVacuna() {
    if (!validateFormAplicatVacuna()) {
        return;
    }

    registrarAplicacionVacuna();

   
}


function validateFormAplicatVacuna() {
    var form = $('#formAplicarVacunaModal');
    form.validate()
    var valid = form.valid();
    return valid;
}


function registrarAplicacionVacuna() {
    var loc = window.rootUrl + "Seguimiento/Evolucionar/RegistrarAplicacionVacunaAsync";

    btnConfirmAplicarVacuna = $('#btnConfirmAplicarVacuna');
    btnCancelAplicarVacuna = $('#btnCancelAplicarVacuna');

    var btnPreHtml = btnConfirmAplicarVacuna.html();

    btnConfirmAplicarVacuna.html('<span class="fas fa-spinner fa-spin"></span><span> aplicando</span>');
    btnCancelAplicarVacuna.addClass('disabled');

    var dataToPost = {
        evolucionAplicacionVacunaId: $('#aplicarVacunaCurrentEvolucionVacunaId').val(),
        esquemaNomivacId: $('#selectAplicarVacunaEsquemaId').val(),
        fechaAplicacion: $('#aplicarVacunaFechaInput').val(),
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {

            btnCancelAplicarVacuna.removeClass('disabled');
            btnConfirmAplicarVacuna.html(btnPreHtml);

            loadGridVacunas();

            var theModal = $('#aplicarVacunaModal');
            theModal.modal('hide');
        },
        error: function (request, status, error) {
            btnCancelAplicarVacuna.removeClass('disabled');
            btnConfirmAplicarVacuna.html(btnPreHtml);

            showErrorDialog("Error", request.responseText, function () { });
            console.log(request.responseText);
        }
    });
}

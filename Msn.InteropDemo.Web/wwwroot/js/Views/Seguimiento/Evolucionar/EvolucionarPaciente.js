//On document Ready
$(function () {
    var id = $('#evolucionId').val();
    if (id) {
        loadEvolucionMenuList();
        loadEvolucion(id);
    } else {
        setupNuevaEvolucion();
    }

});


function loadEvolucionMenuList() {
    var pacienteId = $('#PacienteId').val();
    var container = $('#evolucionListMenuContainer');
    var loc = window.rootUrl + "Seguimiento/Evolucionar/GetEvolucionListMenu?pacienteId=" + pacienteId;

    $.ajax({
        url: loc,
        type: 'GET',
        dataType: 'json',
        cache: false,
        success: function (data) {
            container.html(data.menuItems);
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });

}

function loadEvolucion(id) {
    var btnSerachHallazgos = $('#btnSerachHallazgos');
    var btnSearchVacunas = $('#btnSearchVacunas');
    var btnSearchMedicamentos = $('#btnSearchMedicamentos');
    var btnSaveEvolucion = $('#btnSaveEvolucion');
    var currentConsultaFecha = $('#currentConsultaFecha');
    var nuevaConsultaTitulo = $('#nuevaConsultaTitulo');
    var btnMenuFecha = $('#btnMenuFecha' + id);
    var currentFechaConsultaText = $('#currentFechaConsultaText');
    var currentConsultaProfesionalFullname = $('#currentConsultaProfesionalFullname');
    var textAreaObservacion = $('#textAreaObservacion');

    $('.btn-menu-fechas').each(function () {
        $(this).removeClass('active');
    });

    btnMenuFecha.addClass('active');
    btnSerachHallazgos.css('display', 'none');
    btnSearchVacunas.css('display', 'none');
    btnSearchMedicamentos.css('display', 'none');
    btnSaveEvolucion.css('display', 'none');
    nuevaConsultaTitulo.hide('slow');
    textAreaObservacion.prop('readonly', true);

    var loc = window.rootUrl + "Seguimiento/Evolucionar/GetEvolucion/" + id;

    $.ajax({
        url: loc,
        type: 'GET',
        dataType: 'json',
        cache: false,
        success: function (data) {

            currentFechaConsultaText.text(data.evolucion.fechaConsultaUI)
            currentConsultaProfesionalFullname.text(data.evolucion.profesionalApellidoNombre);
            currentConsultaFecha.show('slow');

            $('#textAreaObservacion').val(data.evolucion.observacion);

            var trHTML = '';

            $("#tableHallazgos > tbody").css('display', 'none');
            $("#tableHallazgos > tbody > tr").remove();

            $.each(data.evolucion.diagnosticos, function (i, item) {

                trHTML += '<tr id="' + item.sctConceptId + '" class="tr-evolucion">';

                if (item.cie10Mapeos.length == 0) {
                    trHTML += '<td>' +
                        '<div class="sctTerm">' + item.sctDescriptionTerm + '</div>' +
                        '<div class="text-muted small">SctId: ' + item.sctConceptId + ' --> ' +
                        '<span>(No mapeado)</span>' +
                        '</div> ' +
                        '</td>';
                }
                else {

                    trHTML += '<td>' +
                        '<div id"' + item.sctConceptId + '" class="sctTerm">' + item.sctDescriptionTerm + '</div>' +
                        '<div class="text-muted small">SctId: ' + item.sctConceptId + ' --> ' +
                    '<span> CIE10: </span >';

                    $.each(item.cie10Mapeos, (function (i, cie) {
                        trHTML += '<span class="text-success">' + cie.cie10SubcategoriaId + ': </span>' +
                            '<span class="text-success">' + cie.cie10SubcategoriaNombre + '</span>' +
                            '<span> | </span>';
                    }));

                    trHTML += '</div>' +
                              '</td>';


                    //trHTML += '<td>' +
                    //    '<div class="sctTerm">' + item.sctDescriptionTerm + '</div>' +
                    //    '<div class="text-muted small">SctId: ' + item.sctConceptId +
                    //    ' --> <span class="text-success">CIE10: ' + item.cie10SubcategoriaId + ' - </span>' +
                    //    '<span class="text-success">' + item.cie10SubcategoriaNombre + '</span>' +
                    //    '</div>' +
                    //    '</td>';
                }

                //trHTML += '<td>' + item.sctDescriptionTerm + '</td>';
                //trHTML += '<td></td></tr>';
            });


            $('#tableHallazgos').append(trHTML);
            $("#tableHallazgos > tbody").show('slow');


            trHTML = ''
            $("#tableVacunas > tbody").css('display', 'none');
            $("#tableVacunas > tbody > tr").remove();
            $.each(data.evolucion.vacunas, function (i, item) {

                trHTML += '<tr id="' + item.sctConceptId + '"><td>' + item.sctDescriptionTerm + '</td></tr>';
            });
            $('#tableVacunas').append(trHTML);
            $("#tableVacunas > tbody").show('slow');


            trHTML = ''
            $("#tableMedicamentos > tbody").css('display', 'none');
            $("#tableMedicamentos > tbody > tr").remove();
            $.each(data.evolucion.medicamentos, function (i, item) {

                trHTML += '<tr id="' + item.sctConceptId + '"><td>' + item.sctDescriptionTerm + '</td></tr>';
            });
            $('#tableMedicamentos').append(trHTML);
            $("#tableMedicamentos > tbody").show('slow');
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });
}


function setupNuevaEvolucion() {

    var btnSerachHallazgos = $('#btnSerachHallazgos');
    var btnSearchVacunas = $('#btnSearchVacunas');
    var btnSearchMedicamentos = $('#btnSearchMedicamentos');
    var btnSaveEvolucion = $('#btnSaveEvolucion');
    var currentConsultaFecha = $('#currentConsultaFecha');
    var nuevaConsultaTitulo = $('#nuevaConsultaTitulo');
    var textAreaObservacion = $('#textAreaObservacion');

    currentConsultaFecha.hide('slow');
    nuevaConsultaTitulo.show('slow');
    btnSerachHallazgos.show('slow');
    btnSearchVacunas.show('slow');
    btnSearchMedicamentos.show('slow');
    btnSaveEvolucion.show('slow');
    textAreaObservacion.prop('readonly', false);



    textAreaObservacion.val('');
    $('#evolucionId').val('');
    $("#tableHallazgos > tbody > tr").remove();
    $("#tableMedicamentos > tbody > tr").remove();
    $("#tableVacunas > tbody > tr").remove();

    $('.btn-menu-fechas').each(function () {
        $(this).removeClass('active');
    });
}


function saveEvolucion() {

    var textAreaObservacion = $('#textAreaObservacion');
    var pacienteId = $('#PacienteId');
    var loc = window.rootUrl + "Seguimiento/Evolucionar/SaveEvolucion";

    var dataToPost = {
        PacienteId: pacienteId.val(),
        Observacion: textAreaObservacion.val(),
        Diagnosticos: [],
        Vacunas: [],
        Medicamentos: [],
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    //Table Hallazgos
    $('#tableHallazgos > tbody > tr').each(function () {

        var sctId = this.id;
        var sctTerm = $('#sctTerm' + sctId).text();

        var diagnosticoItem = {
            SctConceptId: sctId,
            SctDescriptionTerm: sctTerm,
            Cie10Mapeos: []
        };

        var spanMappedCode = $(this).find('span.cie10MappedCode');

        if (spanMappedCode.length > 0) {
            spanMappedCode.each(function () {

                var item = {
                    Cie10SubcategoriaId: this.id,
                    Cie10SubcategoriaNombre: $('#cie10MappedText-' + this.id).text()
                };

                diagnosticoItem.Cie10Mapeos.push(item);
            });
        }

        dataToPost.Diagnosticos.push(diagnosticoItem);
    });

    //Table Vacunas
    $('#tableVacunas > tbody > tr').each(function () {
        var item = {
            SctConceptId: this.id,
            SctDescriptionTerm: this.cells[0].innerHTML
        };

        dataToPost.Vacunas.push(item);
    });

    //Table Medicamentos
    $('#tableMedicamentos > tbody > tr').each(function () {
        var item = {
            SctConceptId: this.id,
            SctDescriptionTerm: this.cells[0].innerHTML
        };

        dataToPost.Medicamentos.push(item);
    });

    if (dataToPost.Observacion.length == 0 || dataToPost.Diagnosticos.length == 0) {
        messageDialog('Control de datos', 'Es necesario al menos un Diagnóstico y una Evolución', (ok) => { });
        return;
    }

    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {
            if (data.success) {
                showSaveEvolucionOk(data.id);
            }
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });

}

function saveEvolucion_OLD() {
    var textAreaObservacion = $('#textAreaObservacion');
    var pacienteId = $('#PacienteId');
    var loc = window.rootUrl + "Seguimiento/Evolucionar/SaveEvolucion";

    var dataToPost = {
        PacienteId: pacienteId.val(),
        Observacion: textAreaObservacion.val(),
        Diagnosticos: [],
        Vacunas: [],
        Medicamentos: [],
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    //Table Hallazgos
    $('#tableHallazgos > tbody > tr').each(function () {

        var strTerm = $(this).find('div.sctTerm').text();
        var cie10MappedId = $(this).find('span.cie10MappedId');
        var cie10MappedText = $(this).find('span.cie10MappedText');
        var cie10Id = null;
        var cie10Text = null;

        if (cie10MappedId.length) {
            cie10Id = cie10MappedId.attr('id');
            cie10Text = cie10MappedText.text();
        }

        var item = {
            SctConceptId: this.id,
            SctDescriptionTerm: strTerm,
            Cie10SubcategoriaId: cie10Id,
            Cie10SubcategoriaNombre: cie10Text
        };

        dataToPost.Diagnosticos.push(item);
    });

    //Table Vacunas
    $('#tableVacunas > tbody > tr').each(function () {
        var item = {
            SctConceptId: this.id,
            SctDescriptionTerm: this.cells[0].innerHTML
        };

        dataToPost.Vacunas.push(item);
    });

    //Table Medicamentos
    $('#tableMedicamentos > tbody > tr').each(function () {
        var item = {
            SctConceptId: this.id,
            SctDescriptionTerm: this.cells[0].innerHTML
        };

        dataToPost.Medicamentos.push(item);
    });

    if (dataToPost.Observacion.length == 0 || dataToPost.Diagnosticos.length == 0) {
        messageDialog('Control de datos', 'Es necesario al menos un Diagnóstico y una Evolución', (ok) => { });
        return;
    }

    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {

            console.log(data);

            if (data.success) {
                showSaveEvolucionOk(data.id);
            }
        },
        error: function (request, status, error) {
            console.log(request.responseText);
            alert(request.responseText);
        }
    });

}

function showSaveEvolucionOk(id) {
    messageDialog('Evolución', 'Evolución generada exitosamente.',
        (accept) => {
            if (accept) {
                $('#evolucionId').val(id);
                loadEvolucionMenuList();
                loadEvolucion(id);
                //redirectToPacientes();
            }
        })
}


function redirectToPacientes() {
    var redirect = window.rootUrl + "Seguimiento/Pacientes/";
    window.location = redirect;
}


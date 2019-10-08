
//On document Ready
$(function () {
    $('#FechaNacimiento').mask('99/99/9999');
})

//////////////////////////////////////////////////////////////////////////////////////
/////////// POST PARA BUSCAR PACIENTES ///////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////
function doPost() {
    var dataToPost = {
        PrimerApellido: $('#PrimerApellido').val(),
        PrimerNombre: $('#PrimerNombre').val(),
        Sexo: $('#Sexo').val(),
        TipoDocumentoId: $("#TipoDocumentoId").val(),
        NroDocumento: $("#NroDocumento").val(),
        FechaNacimiento: $("#FechaNacimiento").val(),
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    //////////////////////////////////////
    //console.log(dataToPost);
    //alert($('#Enabled').is(':checked'));
    //alert("Registro en Consola");
    //return;
    //////////////////////////////////////

    var errorLabel = $('#errorLabel');
    var gridContentLocalResult = $('#gridContentLocalResult');
    var btnNuevoPaciente = $('#btnNuevoPaciente');

    var loc = window.rootUrl + "Seguimiento/Pacientes/BuscarPacienteCoincidentes";
    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        success: function (data) {
            gridContentLocalResult.html(data.table);
            gridContentLocalResult.fadeIn('slow');
            btnNuevoPaciente.fadeIn('slow');
        },
        error: function (request, status, error) {
            console.log("request:" + request.responseText);
            alert(request.responseText);
        }
    });
}

//////////////////////////////////////////////////////////////////////////////////////
/// *** TRABAJO CON UN PACIENTE YA EXISTENTE *** /////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////////////////////
/// MUESTRA EL MODAL Y EJECUTA LAS OPEACIONES DE FEDERACION DE PACIENTE ////////////////
//////////////////////////////////////////////////////////////////////////////////////
function MostarPaciente(id, preExistenteEnDB = false) {
    var progressPacienteLocal = $('#progressPacienteLocal');
    var progressPacienteLocalText = $('#progressPacienteLocalText');
    var progressPacienteLocalCheck = $('#progressPacienteLocalCheck');
    var progressErrorTitle = $('#progressErrorTitle');

    var progressVerificandoExistenciaEnElBUS = $('#progressVerificandoExistenciaEnElBUS');

    var progressPacienteYaFederado = $('#progressPacienteYaFederado');
    var progressPacienteNoFederado = $('#progressPacienteNoFederado');
    var btnEvolucionar = $('#btnEvolucionar');
    var progressAltaPacienteBUS = $('#progressAltaPacienteBUS');
    var progressAltaPacienteBUSFinalizada = $('#progressAltaPacienteBUSFinalizada');
    var progressAltaPacienteBUSFinalizadaError = $('#progressAltaPacienteBUSFinalizadaError');
    var pacienteId = $('#pacienteId');

    var loc = window.rootUrl + "Seguimiento/Pacientes/GetById/?id=" + id;

    pacienteId.val(id);

    $.ajax({
        url: loc,
        type: 'GET',
        dataType: 'json',
        cache: false,
        success: function (data) {
            $('#modalPacienteId').val(data.paciente.id);
            $('#modalPrimerApellido').val(data.paciente.primerApellido);
            $('#modalPrimerNombre').val(data.paciente.primerNombre);
            $('#modalGeneroNombre').val(data.paciente.sexoNombre);
            $('#modalTipoDocumentoNombre').val(data.paciente.tipoDocumentoNombre);
            $('#modalNroDocumento').val(data.paciente.nroDocumento);
            $('#modalFechaNacimiento').val(data.paciente.fechaNacimiento);
        },
        error: function (request, status, error) {
            if (errorLabel) {
                errorLabel.text(request.responseText);
            } else {
                alert(request.responseText);
            }
        }
    });

    if (preExistenteEnDB) {
        progressPacienteLocalText.text(' Paciente ya registrado en la BBDD local ');
        progressPacienteLocalCheck.css('display', 'none');
    } else {
        progressPacienteLocalText.text(' Creado exitosamente en la BBDD local ');
        progressPacienteLocalCheck.show();
    }

    progressPacienteLocal.show();
    progressAltaPacienteBUS.css('display', 'none');
    progressVerificandoExistenciaEnElBUS.css('display', 'none');
    progressAltaPacienteBUSFinalizada.css('display', 'none');
    progressAltaPacienteBUSFinalizadaError.css('display', 'none');
    progressPacienteYaFederado.css('display', 'none');
    progressPacienteNoFederado.css('display', 'none');
    btnEvolucionar.css('display', 'none');

    var federaResult = false;

    $('#modalPaciente').modal('show');

    progressVerificandoExistenciaEnElBUS.delay(500).fadeIn('slow', () => {

        //VERIFICAR EXISTENCIA EN EL BUS
        var pacienteFederado = VerificarExistenciaPacienteLocalEnBUS(id);

        progressVerificandoExistenciaEnElBUS.delay(1000).fadeOut('slow', () => {

            //SI EL PACIENTE NO ESTA FEDERADO
            if (!pacienteFederado) {
                console.log('Paciente:' + id + ' No Federado, se procede a Federar');
                progressPacienteNoFederado.fadeIn('slow');
                progressAltaPacienteBUS.delay(500).fadeIn('slow', () => {
                    federaResult = FederarPaciente(id);
                });

                progressAltaPacienteBUS.delay(2000).fadeOut('slow', () => {
                    //SI LA FEDERACION FUE EXITOSA
                    if (federaResult.success) {
                        progressAltaPacienteBUSFinalizada.fadeIn('slow', () => {
                            btnEvolucionar.fadeIn('slow');
                        });
                    } else {
                        progressAltaPacienteBUSFinalizadaError.fadeIn('slow', () => {
                            console.log(federaResult.message);
                            progressErrorTitle.tooltip('hide');
                            progressErrorTitle.attr('title', federaResult.message);
                            $('[data-toggle="tooltip"]').tooltip(); //refresh del Tooltip
                            progressErrorTitle.tooltip('show');

                            btnEvolucionar.fadeIn('slow');
                        });
                    }
                });
            }
            ////SI EL PACIENTE YA SE ENCUENTRA FEDERADO
            else {
                progressAltaPacienteBUS.delay(500).fadeOut('slow', () => {
                    progressPacienteYaFederado.fadeIn('slow', () => {
                        btnEvolucionar.fadeIn('slow');
                    });
                });
            }
        });
    });
}

/////////////////////////////////////////////////////////////////////////////////////////////
/// VERIFICA LA EXISTENCIA DE UN PACIENTE EN EL BUS QUE YA STA REGISTRADO EN  NUESTRA DB ////
/////////////////////////////////////////////////////////////////////////////////////////////
function VerificarExistenciaPacienteLocalEnBUS(id) {

    var errorLabel = $('#errorLabel');
    var pacieneExiste = false;

    var dataToPost = {
        id: id
    };

    var loc = window.rootUrl + "Seguimiento/Pacientes/VerificarExistenciaPacienteLocalEnBUS";
    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        async: false, //OJO Lo pongo sincronico para poder mosrar los avances en la simulacion de tiempo !! (No usar en produccion)
        cache: false,
        success: function (data) {
            pacieneExiste = data.existe;
        },
        error: function (request, status, error) {
            if (errorLabel) {
                errorLabel.text(request.responseText);
            } else {
                alert(request.responseText);
            }
        }
    });

    return pacieneExiste;
}


//////////////////////////////////////////////////////////////////////////////////////
/// FEDER UN PACIENTE EN EL BUS ////////////////
//////////////////////////////////////////////////////////////////////////////////////
function FederarPaciente(id) {
    var dataToPost = {
        id: id,
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    var ret = "";

    //////////////////////////////////////
    //console.log(dataToPost);
    //alert($('#Enabled').is(':checked'));
    //alert("Registro en Consola");
    //return;
    //////////////////////////////////////

    var errorLabel = $('#errorLabel');

    var loc = window.rootUrl + "Seguimiento/Pacientes/FederarPaciente";
    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        async: false, //OJO Lo pongo sincronico para poder mosrar los avances en la simulacion de tiempo !! (No usar en produccion)
        cache: false,
        success: function (data) {
            if (!data.success) {
                console.log('Error Federando el Paciente: ' + data.message);
            }

            ret = { success: data.success, message: data.message };
        },
        error: function (request, status, error) {
            if (errorLabel) {
                errorLabel.text(request.responseText);
            } else {
                alert(request.responseText);
            }

            ret = { success: false, message: request.responseText };;
        }
    });

    return ret;
}
//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////



/////////////////////////////////////////////////////////////////////////////////////
/// *** TRABAJO CON UN PACIENTE NUEVO *** ///////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////////
/// MUSTRA EL FOMULARIO DE CONFIRMACION Y CARGA DE EMAIL ////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////
function showModalCreatePaciente() {
    $('#modalCreatePrimerApellido').val($('#PrimerApellido').val());
    $('#modalCreatePrimerNombre').val($('#PrimerNombre').val());
    $('#modalCreateSexoId').val($('#Sexo').val());
    $('#modalCreateSexoNombre').val($('#Sexo option:selected').text());
    $('#modalCreateTipoDocumentoId').val($('#TipoDocumentoId').val());
    $('#modalCreateTipoDocumentoNombre').val($("#TipoDocumentoId option:selected").text());
    $('#modalCreateNroDocumento').val($('#NroDocumento').val());
    $('#modalCreateFechaNacimiento').val($('#FechaNacimiento').val());
    $('#modalCreateEmail').val('');

    $('#modalCreatePaciente').modal('show');
}


/////////////////////////////////////////////////////////////////////////////////////
/// EL USUARIO CONFIRMA EN EL FORMULACION DE CONFIRMACION ///////////////////////////
/////////////////////////////////////////////////////////////////////////////////////
function onModalCreatePacienteAccept() {
    var form = $('#formModalCreate');

    form.validate()
    if (!form.valid()) {
        return;
    }

    $('#modalCreatePaciente').modal('hide');

    confirmNuevoPaciente();
}


/////////////////////////////////////////////////////////////////////////////////////
/// SE MUESTRA EL MODAL DE CONFIRMACION PARA LA CREACION DEL NUEVO PACIENTE /////////
/////////////////////////////////////////////////////////////////////////////////////
function confirmNuevoPaciente() {
    confirmDialog("Nuevo Paciente", "Confirma Generar un nuevo paciente con los datos ingresados ?",
        (confirm) => {
            if (confirm) {
                MostarPacienteNuevo();
            }
        },
        false);
}


///////////////////////////////////////////////////////////////////////////////////////////////
/// REGISTRA EL NUEVO PACIENTE Y LO MUESTRA                                                 ///
///////////////////////////////////////////////////////////////////////////////////////////////
function MostarPacienteNuevo() {
    //1. Registrar el paciente en la DB Local
    //2. Muestra el paciente 
    var pacienteId = RegistrarPacienteLocal();
    MostarPaciente(pacienteId, false);
}


////////////////////////////////////////////////////////////////////////////////
/// REGISTRA UN NUEVO PACIENTE EN LA DB LOCAL //////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
function RegistrarPacienteLocal() {
    var ret = 0;
    var pacienteId = $('#pacienteId');

    var dataToPost = {
        PrimerApellido: $('#PrimerApellido').val(),
        PrimerNombre: $('#PrimerNombre').val(),
        Sexo: $('#Sexo').val(),
        TipoDocumentoId: $("#TipoDocumentoId").val(),
        NroDocumento: $("#NroDocumento").val(),
        FechaNacimiento: $("#FechaNacimiento").val(),
        Email: $('#modalCreateEmail').val(), //del formulario de _ModalCreatePaciente
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

    //////////////////////////////////////
    //console.log(dataToPost);
    //alert($('#Enabled').is(':checked'));
    //alert("Registro en Consola");
    //return;
    //////////////////////////////////////
    var errorLabel = $('#errorLabel');

    var loc = window.rootUrl + "Seguimiento/Pacientes/CreateByFrontEnd";
    $.ajax({
        url: loc,
        type: 'POST',
        dataType: 'json',
        data: dataToPost,
        cache: false,
        async: false, //OJO Lo pongo sincronico para poder mosrar los avances en la simulacion de tiempo !! (No usar en produccion)
        success: function (data) {
            if (data.success) {
                pacienteId.val(data.pacienteId);
                ret = data.pacienteId;
            }
            else {
                ret = 0;
            }
        },
        error: function (request, status, error) {
            if (errorLabel) {
                errorLabel.text(request.responseText);
            } else {
                alert(request.responseText);
            }

            ret = 0;
        }
    });

    return ret;
}

function redirectFromPacientePrueba() {
    var pacienteId = $('#pacientesPruebaSelect').val();
    if (pacienteId) {
        $('#pacienteId').val(pacienteId);
        redirect2Evolucion();
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////
/// REDIRECCION A LA VIEW DE EVOLUCION /////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////
function redirect2Evolucion() {
    var loc = window.rootUrl + "Seguimiento/Evolucionar/EvolucionarPaciente/" + $('#pacienteId').val();
    window.location.href = loc;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////
/// MUESTRA LOS COEFICIENTES DE LA BUSQUEDA PAR UN PACIENTES /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////
function MostarCoeficienteBusqueda(id) {
    var loc = window.rootUrl + "Seguimiento/Pacientes/GetCoeficienteBuqueda";
    var theModal = $('#modalCoeficienteBusqueda');
    var container = $('#gridCoeficienteBusquedaContainer');

    let dataToPost = {
        PacienteId: id,
        ApellidoIngresado: $('#PrimerApellido').val(),
        NombreIngresado: $('#PrimerNombre').val(),
        SexoIngresado: $('#Sexo').val(),
        TipoDocumentoIngresado: $("#TipoDocumentoId option:selected").text(),
        NroDocumentoIngresado: $("#NroDocumento").val(),
        FechaNacimientoIngresado: $("#FechaNacimiento").val(),
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
            theModal.modal('show');
        },
        error: function (request, status, error) {
            if (errorLabel) {
                errorLabel.text(request.responseText);
            } else {
                alert(request.responseText);
            }
        }
    });

}
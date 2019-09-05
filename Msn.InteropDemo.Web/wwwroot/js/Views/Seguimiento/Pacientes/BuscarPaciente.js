
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
/// MUESTRA EL MODAL Y EJECUTA LAS OPEACIONES DE BUSQUEDA DE PACIENTE ////////////////
//////////////////////////////////////////////////////////////////////////////////////
function MostarPacienteExistente(id) {
    var progressPacienteLocalRegistrado = $('#progressPacienteLocalRegistrado');
    var progressPacienteLocalRegistradoNuevo = $('#progressPacienteLocalRegistradoNuevo');
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

    progressPacienteLocalRegistrado.show();
    progressPacienteLocalRegistradoNuevo.css('display', 'none');
    progressAltaPacienteBUS.css('display', 'none');
    progressVerificandoExistenciaEnElBUS.css('display', 'none');
    progressAltaPacienteBUSFinalizada.css('display', 'none');
    progressAltaPacienteBUSFinalizadaError.css('display', 'none');
    progressPacienteYaFederado.css('display', 'none');
    progressPacienteNoFederado.css('display', 'none');
    btnEvolucionar.css('display', 'none');

    var federaOk = false;

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
                    federaOk = FederarPaciente(id);
                });

                progressAltaPacienteBUS.delay(2000).fadeOut('slow', () => {
                    if (federaOk) {
                        progressAltaPacienteBUSFinalizada.fadeIn('slow', () => {
                            btnEvolucionar.fadeIn('slow');
                        });
                    } else {
                        progressAltaPacienteBUSFinalizadaError.fadeIn('slow', () => {
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

    var ret = false;

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

            ret = data.success;
        },
        error: function (request, status, error) {
            if (errorLabel) {
                errorLabel.text(request.responseText);
            } else {
                alert(request.responseText);
            }

            ret = false;
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
/// SE MUESTRA EL MODAL QUE REALIZA LAS VERIFICACIONES Y FEDERA EL PACIENTE SI ES NECESARIO ///
///////////////////////////////////////////////////////////////////////////////////////////////
function MostarPacienteNuevo() {
    //1. Registrar el paciente en la DB Local
    //2. Registrar el paciente en la DB Local

    var progressPacienteLocalRegistradoNuevo = $('#progressPacienteLocalRegistradoNuevo');
    var progressPacienteLocalRegistrado = $('#progressPacienteLocalRegistrado');
    var progressVerificandoExistenciaEnElBUS = $('#progressVerificandoExistenciaEnElBUS');
    var progressAltaPacienteBUSFinalizadaError = $('#progressAltaPacienteBUSFinalizadaError');
    var federacionErrorDetalle = $('#federacionErrorDetalle');
    var progressPacienteYaFederado = $('#progressPacienteYaFederado');
    var btnEvolucionar = $('#btnEvolucionar');
    var progressAltaPacienteBUS = $('#progressAltaPacienteBUS');
    var progressAltaPacienteBUSFinalizada = $('#progressAltaPacienteBUSFinalizada');
    var pacienteId = $('#pacienteId');

   
    $('#modalPrimerApellido').val($('#PrimerApellido').val());
    $('#modalPrimerNombre').val($('#PrimerNombre').val());
    $('#modalGeneroNombre').val($('#Sexo').children("option:selected").text());
    $('#modalTipoDocumentoNombre').val($('#TipoDocumentoId').children("option:selected").text());
    $('#modalNroDocumento').val($('#NroDocumento').val());
    $('#modalFechaNacimiento').val($('#FechaNacimiento').val());

    progressPacienteLocalRegistrado.css('display', 'none');
    progressPacienteLocalRegistradoNuevo.css('display', 'none');
    progressVerificandoExistenciaEnElBUS.css('display', 'none');
    progressAltaPacienteBUS.css('display', 'none');
    progressAltaPacienteBUSFinalizada.css('display', 'none');
    progressPacienteYaFederado.css('display', 'none');

    btnEvolucionar.css('display', 'none');

    $('#modalPaciente').modal('show');


    //DAR DE ALTA EL PACIENTE EN NUESTRA DB
    RegistrarPacienteLocal();

    
    progressPacienteLocalRegistradoNuevo.fadeIn('slow', () => {

        //VERIFICAR EXISTENCIA EN EL BUS
        var pacienteFederado = VerificarExistenciaPacienteLocalEnBUS(pacienteId.val());

        progressVerificandoExistenciaEnElBUS.fadeIn('slow', () => {
            progressVerificandoExistenciaEnElBUS.delay(1000).fadeOut('slow', () => {

                alert("pacienteFederado:" + pacienteFederado);

                //SI NO EXISTE EN EL BUS
                if (pacienteFederado) {
                    progressAltaPacienteBUS.fadeIn('slow', () => {
                        var federacionResult = FederarPaciente(pacienteId.val());
                        console.log(federacionResult);
                    });

                    progressAltaPacienteBUS.fadeOut('slow', () => {

                        if (federacionResult.success) {
                            progressAltaPacienteBUSFinalizada.fadeIn('slow', () => {
                                btnEvolucionar.fadeIn('slow');
                            });
                        } else {
                            progressAltaPacienteBUSFinalizadaError.fadeIn('slow', () => {
                                federacionErrorDetalle.text(federacionResult.message);
                                federacionErrorDetalle.fadeIn('slow')
                                btnEvolucionar.fadeIn('slow');
                            });
                        }
                    });
                }
                //SI EXISTE EN EL BU
                else {
                    progressAltaPacienteBUS.fadeOut('slow', () => {
                        progressPacienteYaFederado.fadeIn('slow', () => {
                            btnEvolucionar.fadeIn('slow');
                        });
                    });
                }
            });
        });
    });
    
}


////////////////////////////////////////////////////////////////////////////////
/// REGISTRA UN NUEVO PACIENTE EN LA DB LOCAL //////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
function RegistrarPacienteLocal() {
    var ret;
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
                ret = true;
            }
            else {
                ret = false;
            }
        },
        error: function (request, status, error) {
            if (errorLabel) {
                errorLabel.text(request.responseText);
            } else {
                alert(request.responseText);
            }

            ret = false;
        }
    });

    return ret;    
}



////////////////////////////////////////////////////////////////////////////////////////////////
/// REDIRECCION A LA VIEW DE EVOLUCION /////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////
function redirect2Evolucion() {
    var loc = window.rootUrl + "Seguimiento/Evolucionar/EvolucionarPaciente/" + $('#pacienteId').val();
    window.location.href = loc;
}
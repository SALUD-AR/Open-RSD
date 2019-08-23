
//On document Ready
$(function () {
    updateToolTips();
    $('#FechaNacimiento').mask('99/99/9999');    
})

//Inicializa los Tooltips
function updateToolTips() {
    //$('[data-toggle="tooltip"]').tooltip();
}


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
            updateToolTips()
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
/// MUESTRA EL MODAL Y EJECUTA LAS OPEACIONES DE BUSQUEDA DE PACIENTE ////////////////
//////////////////////////////////////////////////////////////////////////////////////
function MostarPacienteExistente(id) {
    var progressRegistrandoPacienteLocal = $('#progressRegistrandoPacienteLocal');
    var progressPacienteLocalRegistrado = $('#progressPacienteLocalRegistrado');
    var progressPacienteLocalRegistradoNuevo = $('#progressPacienteLocalRegistradoNuevo');
    var progressVerificandoExistenciaEnElBUS = $('#progressVerificandoExistenciaEnElBUS');
    var progressExistenciaEnElBUSVerificada = $('#progressExistenciaEnElBUSVerificada');
    var progressPacienteYaFederado = $('#progressPacienteYaFederado');
    var btnEvolucionar = $('#btnEvolucionar');
    var progressAltaPacienteBUS = $('#progressAltaPacienteBUS');
    var progressAltaPacienteBUSFinalizada = $('#progressAltaPacienteBUSFinalizada');
    var pacienteId = $('#pacienteId');
    
    var loc = window.rootUrl + "Seguimiento/Pacientes/GetById/?id=" + id;

    pacienteId.val(id);

    $.ajax({
        url: loc,
        type: 'GET',
        dataType: 'json',
        //data: dataToPost,
        cache: false,
        //async: false,
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
    progressRegistrandoPacienteLocal.css('display', 'none');
    progressAltaPacienteBUS.css('display', 'none');
    progressVerificandoExistenciaEnElBUS.css('display', 'none');
    progressAltaPacienteBUSFinalizada.css('display', 'none');
    progressPacienteYaFederado.css('display', 'none');
    progressExistenciaEnElBUSVerificada.css('display', 'none');

    btnEvolucionar.css('display', 'none');

    $('#modalPaciente').modal('show');
    

    progressVerificandoExistenciaEnElBUS.fadeIn('slow', () => {

        //VERIFICAR EXISTENCIA EN EL BUS
        var pacienteFederado = VerificarExistenciaPacienteLocalEnBUS(id);

        progressVerificandoExistenciaEnElBUS.delay(500).fadeOut('slow', () => {
            progressExistenciaEnElBUSVerificada.fadeIn('slow');

            //SI NO EXISTE EN EL BU
            if (!pacienteFederado) {
                console.log('Paciente:' + id + ' No Federado, de procese a Federar');
                progressAltaPacienteBUS.fadeIn('slow', () => {
                    FederarPaciente(id);
                });

                progressAltaPacienteBUS.fadeOut('slow', () => {
                    progressAltaPacienteBUSFinalizada.fadeIn('slow', () => {
                        btnEvolucionar.fadeIn('slow');
                    });
                });
            }
            //SI EXISTE EN EL BU
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

function FederarPaciente(id) {
    var dataToPost = {
        id: id,
        __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
    };

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
        cache: false,
        success: function (data) {
            if (!data.success) {
                console.log('Error Federando el Paciente: ' + data.message);
            }
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
//////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////



/////////////////////////////////////////////////////////////////////////////////////
/// *** TRABAJO CON UN PACIENTE NUEVO *** ///////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////
/// PROCESAR NUEVO PACIENTE /////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////

function showModalCreatePaciente() {

    $('#modalCreatePrimerApellido').val($('#PrimerApellido').val());
    $('#modalCreatePrimerNombre').val($('#PrimerNombre').val());
    $('#modalCreateSexoNombre').val($('#Sexo').val());
    $('#modalCreateTipoDocumentoNombre').val($('#TipoDocumentoId').val());
    $('#modalCreateNroDocumento').val($('#NroDocumento').val());
    $('#modalCreateFechaNacimiento').val($('#FechaNacimiento').val());
    $('#modalCreatePaciente').modal('show');
}

function modalCreatePacienteAccept() {
    var form = $('#formModalCreate');
    form.validate()

    if (form.valid()) {
        alert("valido");
    } else {
        alert("IN valido");
    }

}


function confirmNuevoPaciente() {
    confirmDialog("Nuevo Paciente", "Confirma Generar un nuevo paciente con los datos ingresados ?",
        (confirm) => {
            if (confirm) {
                MostarPacienteNuevo();
            }
        },
        false);
}


function MostarPacienteNuevo() {
    //1. Registrar el paciente en la DB Local
    //2. Registrar el paciente en la DB Local

    var progressRegistrandoPacienteLocal = $('#progressRegistrandoPacienteLocal');
    var progressPacienteLocalRegistradoNuevo = $('#progressPacienteLocalRegistradoNuevo');
    var progressPacienteLocalRegistrado = $('#progressPacienteLocalRegistrado');
    var progressVerificandoExistenciaEnElBUS = $('#progressVerificandoExistenciaEnElBUS');
    var progressExistenciaEnElBUSVerificada = $('#progressExistenciaEnElBUSVerificada');
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
    progressRegistrandoPacienteLocal.css('display', 'none');
    progressVerificandoExistenciaEnElBUS.css('display', 'none');
    progressExistenciaEnElBUSVerificada.css('display', 'none');
    progressAltaPacienteBUS.css('display', 'none');
    progressAltaPacienteBUSFinalizada.css('display', 'none');
    progressPacienteYaFederado.css('display', 'none');

    btnEvolucionar.css('display', 'none');

    $('#modalPaciente').modal('show');

    progressRegistrandoPacienteLocal.fadeIn('slow');

    //DAR DE ALTA EL PACIENTE EN NUESTRA DB
    RegistrarPacienteLocal();

    progressRegistrandoPacienteLocal.delay(1000).fadeOut('slow', () => {
        progressPacienteLocalRegistradoNuevo.fadeIn('slow', () => {

            //VERIFICAR EXISTENCIA EN EL BUS
            var pacienteFederado = VerificarExistenciaPacienteLocalEnBUS(pacienteId.val());

            progressVerificandoExistenciaEnElBUS.fadeIn('slow', () => {
                progressVerificandoExistenciaEnElBUS.delay(1000).fadeOut('slow', () => {
                    progressExistenciaEnElBUSVerificada.fadeIn('slow');

                    //SI NO EXISTE EN EL BU
                    if (!pacienteFederado) {
                        progressAltaPacienteBUS.fadeIn('slow', () => {
                            FederarPaciente(pacienteId.val());
                        });

                        progressAltaPacienteBUS.fadeOut('slow', () => {
                            progressAltaPacienteBUSFinalizada.fadeIn('slow', () => {
                                btnEvolucionar.fadeIn('slow');
                            });
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
    });
}


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
        async: false, //Para la simulacion, en Produccion poner: True.
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
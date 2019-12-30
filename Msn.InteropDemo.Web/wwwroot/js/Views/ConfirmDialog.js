
function confirmDialog(title, message, handler, isForDelete = true) {

    var titleBackColor = isForDelete ? "#dc3545" : "whitesmoke";
    var titleTextColor = isForDelete ? "text-light" : "";
    var btnConfirmText = isForDelete ? "Eliminar" : "Confirmar";
    var btnConfirmBackColor = isForDelete ? "btn-outline-danger" : "btn-outline-primary";

    $(`<div class="modal fade" id="myModal" role="dialog" aria-labelledby="confirmDialogModalTitle"> 
     <div class="modal-dialog modal-dialog-centered"> 
       <!-- Modal content--> 
        <div class="modal-content"> 
            <div class="modal-header" style="background-color:${titleBackColor}">
                <h5 class="modal-title ${titleTextColor}" id="confirmDialogModalTitle" >${title}</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
           <div class="modal-body"> 
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <h6 class="text-center">${message}</h6> 
                    </div> 
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12 mb-0">
                         <div class="text-center"> 
                            <button type="button" class="btn ${btnConfirmBackColor} btn-yes" data-dismiss="modal">${btnConfirmText}</button>
                            <button type="button" class="btn btn-outline-secondary btn-no" data-dismiss="modal">Cancelar</button>
                         </div> 
                    </div> 
                </div>
           </div> 
       </div> 
    </div> 
  </div>`).appendTo('body');

    //Trigger the modal
    $("#myModal").modal({
        backdrop: 'static',
        keyboard: false
    });

    //Pass true to a callback function
    $(".btn-yes").click(function () {
        handler(true);
        $("#myModal").modal("hide");
    });

    //Pass false to callback function
    $(".btn-no").click(function () {
        handler(false);
        $("#myModal").modal("hide");
    });

    //Remove the modal once it is closed.
    $("#myModal").on('hidden.bs.modal', function () {
        $("#myModal").remove();
    });
}


function messageDialog(title, message, handler) {

    var titleBackColor = "whitesmoke";
    var titleTextColor = "";
    var btnConfirmText = "Aceptar";
    var btnConfirmBackColor = "btn-outline-primary";

    $(`<div class="modal fade" id="myModalSingleMsg" role="dialog" aria-labelledby="confirmDialogModalTitle"> 
     <div class="modal-dialog modal-dialog-centered"> 
       <!-- Modal content--> 
        <div class="modal-content"> 
            <div class="modal-header" style="background-color:${titleBackColor}">
                <h5 class="modal-title ${titleTextColor}" id="confirmDialogModalTitle" >${title}</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
           <div class="modal-body"> 
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <h6 class="text-center">${message}</h6> 
                    </div> 
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12 mb-0">
                         <div class="text-center"> 
                            <button type="button" class="btn ${btnConfirmBackColor} btn-yes" data-dismiss="modal">${btnConfirmText}</button>
                         </div> 
                    </div> 
                </div>
           </div> 
       </div> 
    </div> 
  </div>`).appendTo('body');

    //Trigger the modal
    $("#myModalSingleMsg").modal({
        backdrop: 'static',
        keyboard: false
    });

    //Pass true to a callback function
    $(".btn-yes").click(function () {
        handler(true);
        $("#myModalSingleMsg").modal("hide");
    });

    //Remove the modal once it is closed.
    $("#myModalSingleMsg").on('hidden.bs.modal', function () {
        $("#myModalSingleMsg").remove();
    });
}


function showErrorDialog(title, message, handler) {

    var titleBackColor = "#dc3545";
    var titleTextColor = "text-light";
    var btnConfirmText = "Aceptar";
    var btnConfirmBackColor = "btn-outline-danger";

    $(`<div class="modal fade" id="myErrorModal" role="dialog" aria-labelledby="errorDialogModalTitle"> 
     <div class="modal-dialog modal-dialog-centered"> 
       <!-- Modal content--> 
        <div class="modal-content"> 
            <div class="modal-header" style="background-color:${titleBackColor}">
                <h5 class="modal-title ${titleTextColor}" id="errorDialogModalTitle" >${title}</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
           <div class="modal-body"> 
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <h6 class="text-center">${message}</h6> 
                    </div> 
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12 mb-0">
                         <div class="text-center"> 
                            <button type="button" class="btn ${btnConfirmBackColor} btn-yes" data-dismiss="modal">${btnConfirmText}</button>                            
                         </div> 
                    </div> 
                </div>
           </div> 
       </div> 
    </div> 
  </div>`).appendTo('body');

    //Trigger the modal
    $("#myErrorModal").modal({
        backdrop: 'static',
        keyboard: false
    });

    //Pass true to a callback function
    $(".btn-yes").click(function () {
        handler(true);
        $("#myErrorModal").modal("hide");
    });

    //Pass false to callback function
    $(".btn-no").click(function () {
        handler(false);
        $("#myErrorModal").modal("hide");
    });

    //Remove the modal once it is closed.
    $("#myErrorModal").on('hidden.bs.modal', function () {
        $("#myErrorModal").remove();
    });
}
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices;
using Msn.InteropDemo.ViewModel.Pacientes;
using Msn.InteropDemo.Web.Helpers;
using Msn.InteropDemo.Web.Extensions;
using Msn.InteropDemo.ViewModel.Request;
using Microsoft.AspNetCore.Authorization;

namespace Msn.InteropDemo.Web.Areas.Seguimiento.Controllers
{
    [Area("Seguimiento")]
    [Authorize]
    public class PacientesController : Web.Controllers.Base.ControllerBase
    {
        private readonly IPacienteAppService _pacienteAppService;
        private readonly ILogger<PacientesController> _logger;
        private readonly ISelectListHelper _selectListHelper;

        public PacientesController(AppServices.IPacienteAppService pacienteAppService,
                                   ILogger<PacientesController> logger,
                                   Helpers.ISelectListHelper selectListHelper)
        {
            _pacienteAppService = pacienteAppService;
            _logger = logger;
            _selectListHelper = selectListHelper;
        }

        private void SetupModel(BuscarPacienteRequestModel model)
        {
            model.TipoDocumentoList = _selectListHelper.GetTipoDocumentoList();
            model.SexoList = _selectListHelper.GetSexoList();
        }

        private void SetupModel(ViewModel.Pacientes.PacienteViewModel model)
        {
            model.TipoDocumentoList = _selectListHelper.GetTipoDocumentoList();
            model.SexoList = _selectListHelper.GetSexoList();
        }

        
        
        // GET: Pacientes
        public ActionResult Index()
        {
            return RedirectToAction(nameof(BuscarPaciente));
        }

    
        // GET: Pacientes/Create
        [Helpers.Attributes.Breadcrumb("Nuevo Paciente")]
        public ActionResult Create()
        {
            var model = new PacienteViewModel();
            SetupModel(model);
            return View(model);
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Helpers.Attributes.Breadcrumb()]
        public ActionResult Create(PacienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                SetupModel(model);
                return View(model);
            }

            try
            {
                var op = _pacienteAppService.Save(model);
                if (op.NOK)
                {
                    this.SetModelStateErrors(op);
                    SetupModel(model);
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error generando Paciente");
                ModelState.AddModelError("", ex.Message);
                SetupModel(model);
                return View(model);                
            }
        }

        [Helpers.Attributes.Breadcrumb("Pacientes")]        
        public ActionResult BuscarPaciente()
        {
            var model = new BuscarPacienteRequestModel();
            SetupModel(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult BuscarPacienteCoincidentes(BuscarPacienteRequestModel model)
        {
            try
            {
                var items = _pacienteAppService.BuscarPacienteCoincidentesLocal(model.PrimerApellido,
                                                       model.PrimerNombre,
                                                       Enum.Parse<Common.Constants.TipoDocumento>(model.TipoDocumentoId.Value.ToString(), true),
                                                       model.NroDocumento.Value,
                                                       model.Sexo,
                                                       model.FechaNacimiento);

                var pacienteListModel = new PacienteListViewModel { Items = items };
                var table = this.RenderViewToStringAsync("Partials/_GridBusquedaPaciente", pacienteListModel).Result;
                return new JsonResult(new { table }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError("Error generando vista dinamica: _GridBusquedaPaciente");
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
        }

        public JsonResult GetById(int id)
        {
            try
            {
                var paciente = _pacienteAppService.GetById(id);
                return new JsonResult(new { paciente }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo Paciente");
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpPost]
        public JsonResult VerificarExistenciaPacienteLocalEnBUS(int id)
        {
            try
            {
                var existe = _pacienteAppService.ExistePacienteEnBUS(id);
                return new JsonResult(new { existe }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Verificando Paciente");
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult FederarPaciente(int id)
        {
            try
            {
                var op = _pacienteAppService.FederarPaciente(id);
                if(op.OK)
                {
                    return new JsonResult(new { success = true, message = "" }) { StatusCode = 200 };
                }
                else
                {
                    return new JsonResult(new { success = false, message = op.ToString() }) { StatusCode = 200 };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Federando Paciente");
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult CreateByFrontEnd(ViewModel.Pacientes.PacienteViewModel entity)
        {
            if(!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = FriendlyErrors() }) { StatusCode = 200 };
            }

            try
            {
                var op = _pacienteAppService.Save(entity);
                if (op.OK)
                {
                    return new JsonResult(new { success = true, message = "", PacienteId = op.Id }) { StatusCode = 200 };
                }
                else
                {
                    return new JsonResult(new { success = false, message = op.ToString() }) { StatusCode = 200 };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Federando Paciente");
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
        }


    }
}
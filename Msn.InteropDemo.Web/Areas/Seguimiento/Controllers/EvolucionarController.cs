using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices;
using Msn.InteropDemo.Web.Extensions;
using Msn.InteropDemo.Web.Helpers;
using System;
using System.Linq;

namespace Msn.InteropDemo.Web.Areas.Seguimiento.Controllers
{
    [Area("Seguimiento")]
    [Authorize]
    public class EvolucionarController : Web.Controllers.Base.ControllerBase
    {
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IEvolucionAppService _evolucionAppService;
        private readonly ISelectListHelper _selectListHelper;
        private readonly ILogger<EvolucionarController> _logger;

        public EvolucionarController(IPacienteAppService pacienteAppService, 
                                     IEvolucionAppService evolucionAppService,
                                     ISelectListHelper selectListHelper,
                                     ILogger<EvolucionarController> logger
            )
        {
            _pacienteAppService = pacienteAppService;
            _evolucionAppService = evolucionAppService;
            _selectListHelper = selectListHelper;
            _logger = logger;
        }
        // GET: Evolucionar
        public ActionResult Index()
        {
            //Redirecciono con el Paciente 1 
            //return RedirectToAction(nameof(EvolucionarPaciente), new {id});
            return View();
        }

        [Helpers.Attributes.Breadcrumb("Historia Clínica")]
        public ActionResult EvolucionarPaciente(int id)
        {
            var model = _evolucionAppService.GetEvolucionesHisto(id);
            if(model.Items.Any())
            {
                model.Items = model.Items.OrderByDescending(x => x.FechaEvolucion).ToList();
                model.PredefEvolucionId = model.Items[0].Id;
            }

            return View(model);
        }



        public JsonResult SearchByExpressionTerm(string expression, string term)
        {
            try
            {
                string table;
                var items = _evolucionAppService.SearchSnowstormByExpressionTerm(expression,
                                                                                 term)
                                                                                .ToList();
                if(items.Any())
                {
                    table = this.RenderViewToStringAsync("Partials/_GridSnowstormSearchResult", items).Result;
                }
                else
                {
                    var model = new Models.SearchNotfound { TermNotFound = term };
                    table = this.RenderViewToStringAsync("Partials/_SerachSnowstormNotFound", model).Result;
                }

                return new JsonResult(new { table }) { StatusCode = 200 };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
        }

        public JsonResult SearchHallazgos(string term)
        {
            try
            {
                var items = _evolucionAppService.SearchSnowstormHallazgos(term);
                var table = this.RenderViewToStringAsync("Partials/_GridBusquedaHallazgos", items).Result;
                return new JsonResult(new { table }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
        }

        public JsonResult GetEvolucion(int id)
        {
            try
            {
                var model = _evolucionAppService.GetById(id);
                return new JsonResult(new { Evolucion = model }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
        }

        [HttpPost]
        public JsonResult SaveEvolucion(ViewModel.Evoluciones.EvolucionViewModel model)
        {
            try
            {
                var op = _evolucionAppService.Save(model);
                if (op.OK)
                {
                    return new JsonResult(new { success = true, id = op.Id, message = "" }) { StatusCode = 200 };
                }
                else
                {
                    return new JsonResult(new { success = false, message = op.ToString() }) { StatusCode = 200 };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando Evolucion");
                return new JsonResult(new { success = false,  message = ex.Message }) { StatusCode = 500 };
            }
        }
    }
}
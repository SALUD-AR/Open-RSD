using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices;


namespace Msn.InteropDemo.Web.Areas.Seguimiento.Controllers
{
    [Area("Seguimiento")]
    [Authorize]
    public class LogActividadesController : Web.Controllers.Base.ControllerBase
    {
        private readonly ILogActivityAppService _service;
        private readonly ILogger<LogActividadesController> _logger;

        public LogActividadesController(ILogActivityAppService logActivityAppService,
                                        ILogger<LogActividadesController> logger)
        {
            _service = logActivityAppService;
            _logger = logger;
        }

        // GET: LogActividades
        public ActionResult Index()
        {
            //var sessionId = HttpContext.Session.Id;
            var model = _service.Get<ViewModel.Activity.ActivityLogListItemViewModel>(filter: x => x.CreatedUserId == this.GetUserId(), 
                                                                                                    includeProperties: "ActivityTypeDescriptor");

            model = model.OrderByDescending(x => x.CreatedDateTime).ToList();

            return View(model);
        }


        public JsonResult GetActivity(int id)
        {
            try
            {
                var model = _service.GetById<ViewModel.Activity.ActivityLogViewModel>(id);
                return new JsonResult(new { activityLog = model }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return new JsonResult(new { message = ex.Message }) { StatusCode = 500 };
            }
        }


    }
}
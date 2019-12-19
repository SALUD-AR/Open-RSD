using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices;
using Msn.InteropDemo.Web.Extensions;

namespace Msn.InteropDemo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
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
            ViewBag.DateFrom = DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Today.ToString("dd/MM/yyyy");
            return View();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SeachActivities(string dateFrom, string dateTo)
        {
            var dtFrom = Common.Utils.Helpers.DateTimeHelper.FromDateTimeAR(dateFrom);
            var dtTo   = Common.Utils.Helpers.DateTimeHelper.FromDateTimeAR(dateTo);
            dtTo = dtTo.Value.AddHours(23).AddMinutes(59).AddSeconds(29);

            try
            {
                var model = _service.Get<ViewModel.Activity.ActivityLogListItemViewModel>(filter:x => x.CreatedDateTime >= dtFrom && x.CreatedDateTime <= dtTo, 
                                                                                  includeProperties: "ActivityTypeDescriptor,CreatedUser", 
                                                                                  orderBy: o => o.OrderByDescending(c => c.CreatedDateTime));

                var table = this.RenderViewToStringAsync("Partials/_GridLogActivity", model).Result;
                return new JsonResult(new { success = true, table }) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo datos");
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }

        }


    }
}
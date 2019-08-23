using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.Fhir;

namespace Msn.InteropDemo.Web.Areas.Seguimiento.Controllers
{
    [Area("Seguimiento")]
    [Authorize]
    public class HomeController : Web.Controllers.Base.ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPatientManager _patientManager;

        public HomeController(ILogger<HomeController> logger, IPatientManager patientManager)
        {
            _logger = logger;
            _patientManager = patientManager;
        }

     
        public IActionResult Index()
        {
            //PROBADO: OK
            //try
            //{
            //    var bunble = _patientManager.GetPatientsByIdentifier(Common.Constants.DomainName.LocalDomain, "2222222");
            //    var str = bunble.ToString();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.ToString());
            //}
            //PROBADO: OK
            //try
            //{
            //    var patient = _patientManager.GetPatientByBusId("3509519");
            //    var str = patient.ToString();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.ToString());
            //}

            //try
            //{
            //    var bundle = _patientManager.GetPatientsByMatch(17287412, 
            //                                                    "amorese", 
            //                                                    "miguel", 
            //                                                    null, 
            //                                                    Common.Constants.Sexo.Masculino, 
            //                                                    new DateTime(1964, 6, 30));
            //    var str = bundle.ToString();
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.ToString());
            //}

            return RedirectToAction("Index", "Pacientes");
        }
    }
}
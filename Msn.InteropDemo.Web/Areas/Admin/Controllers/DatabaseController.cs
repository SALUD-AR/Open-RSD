using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.Data.Context;
using System;
using System.Linq;

namespace Msn.InteropDemo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public class DatabaseController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<DatabaseController> _logger;

        public DatabaseController(ILogger<DatabaseController> logger,
                                  Msn.InteropDemo.Data.Context.DataContext dataContext)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sequences()
        {
            var items = _dataContext.SqliteSequences.ToList();
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateSequence(string tableName, int sequence)
        {
            if(sequence <= 0)
            {
                return new JsonResult(new { success = false, message = $"Valor de secuencia debe ser mayor a cero. Se ingresó: {sequence}" }) { StatusCode = 200 };
            }

            try
            {
                var entity = _dataContext.SqliteSequences.FirstOrDefault(x => x.TableName == tableName);

                if (entity != null)
                {
                    var prevValue = int.Parse(entity.Sequence);
                    if (prevValue > sequence)
                    {
                        return new JsonResult(new { success = false, message = $"No es posible configurar una secuencia menor a la actual. Secuencia actual:{entity.Sequence}, Secuencia ingresada:{sequence} " }) { StatusCode = 200 };
                    }

                    entity.Sequence = sequence.ToString();
                    _dataContext.SaveChanges();
                    _logger.LogInformation($"Secuencia actualizada correctamente. Tabla:{tableName}\tValor Anterior:{prevValue}\tValor actual:{sequence}\tUsuario{User.Identity.Name}");

                    return new JsonResult(new { success = true, message = "Secuancia actualizada correctamente." }) { StatusCode = 200 };
                }
                else
                {
                    return new JsonResult(new { success = false, message = $"Entity con TableName:{tableName}. No encontrado." }) { StatusCode = 200 };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error intentando modificar Sequence por el usuario:{this.User.Identity.Name}");
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }
        }
    }
}
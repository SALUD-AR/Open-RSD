using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msn.InteropDemo.Data.Context;
using System.Linq;

namespace Msn.InteropDemo.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public class DatabaseController : Controller
    {
        private readonly DataContext _dataContext;

        public DatabaseController(Msn.InteropDemo.Data.Context.DataContext dataContext)
        {
            _dataContext = dataContext;
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
    }
}
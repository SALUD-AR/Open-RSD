
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Msn.InteropDemo.Web.Controllers.Base
{
    public abstract class ControllerBase : Controller
    {
        public void SetCurrentConsorcioId(int id) => ViewBag.HeaderConsorcioId = id;

        public void SetModelStateErrors<TKey>(Common.OperationResults.OperationResult<TKey> operationResult)
        {
            foreach (var item in operationResult.GetErrorlist())
            {
                ModelState.AddModelError(item.ErrorCode, item.ErrorDescription);
            }
        }

        public string FriendlyErrors(string title = "Existen datos invalidos o incompletos: ")
        {
            var errors = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

            errors = title + errors;

            return errors;
        }

        public string GetUserId()
        {
            #if DEBUG
            if (!User.Identity.IsAuthenticated)
            {
                return "1";
            }
            #endif

            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}

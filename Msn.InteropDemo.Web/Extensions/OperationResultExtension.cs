using Microsoft.AspNetCore.Mvc.ModelBinding;
using Msn.InteropDemo.Common.OperationResults;

namespace Msn.InteropDemo.Web.Extensions
{
    public static class OperationResultExtension
    {
        public static ModelStateDictionary LoadErrors<Tkey>(this ModelStateDictionary modelState, OperationResult<Tkey> op)
        {
            foreach (var item in op.GetErrorlist())
            {
                modelState.AddModelError(item.ErrorCode, item.ErrorDescription);
            }

            return modelState;
        }
    }
}

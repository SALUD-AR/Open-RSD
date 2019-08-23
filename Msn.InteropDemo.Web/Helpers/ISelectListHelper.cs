using Microsoft.AspNetCore.Mvc.Rendering;

namespace Msn.InteropDemo.Web.Helpers
{
    public interface ISelectListHelper
    {
        SelectList GetSexoList(string selectedValue = "");
        SelectList GetTipoDocumentoList(string selectedValue = "");
    }
}

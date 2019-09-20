using Msn.InteropDemo.ViewModel.Snomed;
using System.Collections.Generic;

namespace Msn.InteropDemo.AppServices
{
    public interface ICie10AppService : Core.IGenericServiceReadOnly<Entities.Codificacion.Cie10>
    {
        IEnumerable<Cie10MapResultViewModel> GetCie10MappedItems(string conceptId);
        IEnumerable<Cie10MapResultViewModel> GetCie10MappedItems(string conceptId,
                                                                 string sexo,
                                                                 int edad);
    }
}

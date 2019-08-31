namespace Msn.InteropDemo.AppServices
{
    public interface ICie10AppService : Core.IGenericServiceReadOnly<Entities.Codificacion.Cie10>
    {
        System.Collections.Generic.IEnumerable<ViewModel.Snomed.Cie10MapResultViewModel> GetCie10MappedItems(string conceptId);
    }
}

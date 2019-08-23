
namespace Msn.InteropDemo.ViewModel.Core
{
    public abstract class EntityDescriptorModel : EntityDescriptorModel<int>
    { }

    public abstract class EntityDescriptorModel<TKey> : EntityBaseModel<TKey>
    {
        public string Nombre { get; set; }

        public int? Orden { get; set; }
    }
}

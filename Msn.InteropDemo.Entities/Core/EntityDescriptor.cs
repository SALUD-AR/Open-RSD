using System.ComponentModel.DataAnnotations.Schema;

namespace Msn.InteropDemo.Entities.Core
{
    public abstract class EntityDescriptor : EntityDescriptor<int>
    { }

    public abstract class EntityDescriptor<TKey> : EntityBase<TKey>
    {
        [Column(TypeName = "varchar(50)")]
        public string Nombre { get; set; }

        public int? Orden { get; set; }
    }
}

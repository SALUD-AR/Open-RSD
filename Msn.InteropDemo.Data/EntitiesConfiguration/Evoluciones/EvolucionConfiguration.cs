using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Pacientes
{
    public class EvolucionConfiguration : IEntityTypeConfiguration<Entities.Evoluciones.Evolucion>
    {
        public void Configure(EntityTypeBuilder<Entities.Evoluciones.Evolucion> builder)
        {
            builder.Property(p => p.Observacion)
                .HasMaxLength(500)
                .IsUnicode()
                .IsRequired(false);

            builder.HasIndex(x => x.CreatedDateTime);
        }
    }
}

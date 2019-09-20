using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Pacientes
{
    public class EvolucionDiagnosticoCie10Configuration : IEntityTypeConfiguration<Entities.Evoluciones.EvolucionDiagnosticoCie10>
    {
        public void Configure(EntityTypeBuilder<Entities.Evoluciones.EvolucionDiagnosticoCie10> builder)
        {
            builder.Property(x => x.Cie10SubcategoriaId)
               .HasMaxLength(4)
               .HasColumnType("char(4)")
               .IsRequired(false)
               .IsUnicode(false);

            builder.Property(x => x.Cie10SubcategoriaNombre)
              .HasMaxLength(300)
              .IsRequired(false)
              .IsUnicode();
        }
    }
}

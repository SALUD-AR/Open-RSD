using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Pacientes
{
    public class EvolucionDiagnosticoConfiguration : IEntityTypeConfiguration<Entities.Evoluciones.EvolucionDiagnostico>
    {
        public void Configure(EntityTypeBuilder<Entities.Evoluciones.EvolucionDiagnostico> builder)
        {
            builder.Property(p => p.SctConceptId)
                .HasColumnType("numeric(18,0)")
                .IsRequired();

            builder.Property(p => p.SctDescriptionTerm)
                .HasMaxLength(500)
                .IsUnicode()
                .IsRequired();

            builder.Property(x => x.Cie10SubcategoriaId)
               .HasMaxLength(4)
               .HasColumnType("char(4)")
               .IsRequired(false)
               .IsUnicode(false);

            builder.Property(x => x.Cie10SubcategoriaNombre)
              .HasMaxLength(300)
              .IsRequired(false)
              .IsUnicode();

            builder.HasIndex(x => x.SctConceptId);

            builder.HasIndex(x => x.Cie10SubcategoriaId);
        }
    }
}

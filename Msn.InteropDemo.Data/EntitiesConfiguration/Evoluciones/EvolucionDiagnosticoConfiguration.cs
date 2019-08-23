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

            builder.HasIndex(x => x.SctConceptId);
        }
    }
}

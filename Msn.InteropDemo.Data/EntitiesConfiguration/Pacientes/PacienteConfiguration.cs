using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msn.InteropDemo.Entities.Pacientes;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Pacientes
{
    public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder.Property(p => p.PrimerNombre)
                .HasMaxLength(50)
                .IsRequired();


            builder.Property(p => p.PrimerNombreSoundex)
                .HasMaxLength(4)
                .IsRequired()
                .IsUnicode(false);

            builder.HasIndex(x => x.PrimerNombreSoundex);


            builder.Property(p => p.OtrosNombres)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(p => p.PrimerApellido)
                .HasMaxLength(50)
                .IsRequired();
            

            builder.Property(p => p.PrimerApellidoSoundex)
                .HasMaxLength(4)
                .IsRequired()
                .IsUnicode(false);

            builder.HasIndex(x => x.PrimerApellidoSoundex);


            builder.Property(p => p.OtrosApellidos)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(p => p.Sexo)
                .HasColumnType("char(1)")
                .IsRequired();

            builder.Property(p => p.FechaNacimiento)
                            .HasColumnType("Date")
                            .IsRequired();

            builder.Property(p => p.FederadoDateTime)
                            .HasColumnType("SmallDateTime")
                            .IsRequired(false);

            builder.Property(p => p.Email)
                .HasMaxLength(100)
                .IsRequired(false)
                .IsUnicode(false);
        }
    }
}

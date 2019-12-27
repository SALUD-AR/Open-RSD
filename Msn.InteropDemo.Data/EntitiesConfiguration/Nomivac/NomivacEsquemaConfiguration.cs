using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msn.InteropDemo.Entities.Nomivac;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Nomivac
{
    public class NomivacEsquemaConfiguration : IEntityTypeConfiguration<NomivacEsquema>
    {
        public void Configure(EntityTypeBuilder<NomivacEsquema> builder)
        {
            builder.Property(k => k.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.Nombre)
                .HasMaxLength(250)
                .IsUnicode()
                .IsRequired();
        }
    }
}

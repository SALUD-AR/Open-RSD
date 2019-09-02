using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msn.InteropDemo.Entities.Codificacion;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Codificacion
{
    public class Cie10Configuration : IEntityTypeConfiguration<Entities.Codificacion.Cie10>
    {
        public void Configure(EntityTypeBuilder<Cie10> builder)
        {
            builder.HasKey(x => x.SubcategoriaId);

            builder.Property(x => x.SubcategoriaId)
                .ValueGeneratedNever()
                .HasMaxLength(4)
                .HasColumnType("char(4)")
                .IsRequired(true)
                .IsUnicode(false);

            builder.Property(x => x.SubcategoriaNombre)
                .HasMaxLength(300)
                .IsRequired()
                .IsUnicode();

            builder.Property(x => x.CategoriaNombre)
                .HasMaxLength(300)
                .IsRequired()
                .IsUnicode();
        }
    }
}

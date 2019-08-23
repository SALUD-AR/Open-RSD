using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msn.InteropDemo.Entities.Pacientes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Pacientes
{
    public class TipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDocumento>
    {
        public void Configure(EntityTypeBuilder<TipoDocumento> builder)
        {
            builder.Property(k => k.Id)
              .ValueGeneratedNever();

            builder.Property(p => p.Nombre)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}

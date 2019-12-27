using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msn.InteropDemo.Entities.Nomivac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Nomivac
{
    public class NomivacVacunaConfiguration : IEntityTypeConfiguration<Entities.Nomivac.NomivacVacuna>
    {
        public void Configure(EntityTypeBuilder<NomivacVacuna> builder)
        {
            builder.Property(k => k.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.Nombre)
                .HasMaxLength(250)
                .IsUnicode()
                .IsRequired();

            builder.Property(p => p.SctId)
                .HasColumnType("numeric(18,0)")
                .IsRequired();

            builder.Property(p => p.SctTerm)
                .HasMaxLength(250)
                .IsUnicode()
                .IsRequired();

            builder.HasIndex(p => p.SctId).IsUnique(true);

        }
    }
}

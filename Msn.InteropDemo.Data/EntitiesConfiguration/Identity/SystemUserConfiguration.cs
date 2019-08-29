using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msn.InteropDemo.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Identity
{
    public class SystemUserConfiguration : IEntityTypeConfiguration<Entities.Identity.SystemUser>
    {
        public void Configure(EntityTypeBuilder<SystemUser> builder)
        {
            builder.Property(p => p.Nombre)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            builder.Property(p => p.Apellido)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            builder.Property(p => p.CUIT)
                .IsRequired(false);
        }
    }
}

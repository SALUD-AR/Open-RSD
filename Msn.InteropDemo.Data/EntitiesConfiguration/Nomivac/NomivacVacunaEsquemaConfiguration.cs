using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msn.InteropDemo.Entities.Nomivac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Nomivac
{
    public class NomivacVacunaEsquemaConfiguration : IEntityTypeConfiguration<NomivacVacunaEsquema>
    {
        public void Configure(EntityTypeBuilder<NomivacVacunaEsquema> builder)
        {
            builder.HasKey("NomivacEsquemaId", "NomivacVacunaId");
        }
    }
}

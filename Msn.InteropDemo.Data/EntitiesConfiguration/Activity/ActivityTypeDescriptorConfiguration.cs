using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Pacientes
{
    public class ActivityTypeDescriptorConfiguration : IEntityTypeConfiguration<Entities.Activity.ActivityTypeDescriptor>
    {
        public void Configure(EntityTypeBuilder<Entities.Activity.ActivityTypeDescriptor> builder)
        {
            builder.Property(p => p.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired(true);
        }
    }
}

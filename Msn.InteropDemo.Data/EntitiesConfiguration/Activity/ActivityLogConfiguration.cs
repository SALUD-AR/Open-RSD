using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Pacientes
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<Entities.Activity.ActivityLog>
    {
        public void Configure(EntityTypeBuilder<Entities.Activity.ActivityLog> builder)
        {
            builder.Property(p => p.ActivityRequest)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .IsRequired(false);

            builder.Property(p => p.ActivityResponse)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .IsRequired(false);
            
            builder.Property(p => p.SessionUserId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(p => p.CreatedDateTime)
                .HasColumnType("SmallDateTime")
                .IsRequired();

            builder.HasIndex(x => x.CreatedUserId);

            builder.HasIndex(x => x.SessionUserId);

            builder.HasIndex(x => x.CreatedDateTime);

        }
    }
}

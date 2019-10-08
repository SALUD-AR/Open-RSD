using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Msn.InteropDemo.Entities.Sqlite;

namespace Msn.InteropDemo.Data.EntitiesConfiguration.Sqlite
{
    public class SqliteSequenceConfiguration : IEntityTypeConfiguration<Msn.InteropDemo.Entities.Sqlite.SqliteSequence>
    {
        public void Configure(EntityTypeBuilder<SqliteSequence> builder)
        {
            builder.ToTable("sqlite_sequence");

            builder.HasKey(p => p.TableName);

            builder.Property(p => p.TableName)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Sequence)
                .HasColumnName("seq")
                .HasMaxLength(100)
                .IsRequired();

        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Msn.InteropDemo.Data.Extensions;
using Msn.InteropDemo.Entities.Identity;
using System.Linq;

namespace Msn.InteropDemo.Data.Context
{
    public class DataContext : IdentityDbContext<SystemUser>
    {
        //public DataContext(DbContextOptions<DataContext> options)
        //    : base(options)
        //{
        //}

        public DataContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();
            optionsBuilder.UseSqlite(config.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //****************** CONFIGURACION GENERAL *****************************************
            // equivalent of modelBuilder.Conventions.AddFromAssembly(Assembly.GetExecutingAssembly());
            // look at this answer: https://stackoverflow.com/a/43075152/3419825

            // for the other conventions, we do a metadata model loop
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                entityType.Relational().TableName = entityType.DisplayName();

                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                // and modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }
            //********************************************************************************************

            modelBuilder.ApplyAllConfigurations();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Entities.Pacientes.Paciente> Pacientes { get; set; }

        public DbSet<Entities.Pacientes.TipoDocumento> TiposDocumento { get; set; }

        public DbSet<Entities.Activity.ActivityLog> ActivityLogs { get; set; }

        public DbSet<Entities.Activity.ActivityTypeDescriptor> ActivityTypeDescriptors { get; set; }

        public DbSet<Entities.Evoluciones.Evolucion> Evoluciones { get; set; }

        public DbSet<Entities.Evoluciones.EvolucionDiagnostico> EvolucionDiagnosticos { get; set; }

        public DbSet<Entities.Evoluciones.EvolucionMedicamento> EvolucionMedicamentos { get; set; }

        public DbSet<Entities.Evoluciones.EvolucionVacunaAplicacion> EvolucionVacunaAplicaciones { get; set; }

        public DbSet<Entities.Codificacion.Cie10> Cie10 { get; set; }
    }
}

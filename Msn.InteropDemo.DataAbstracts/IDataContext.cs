using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Msn.InteropDemo.DataAbstracts
{
    public interface IDataContext : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        IDbSet<Entities.Pacientes.Paciente> Pacientes { get; set; }

        IDbSet<Entities.Pacientes.TipoDocumento> TiposDocumento { get; set; }

        IDbSet<Entities.Activity.ActivityLog> ActivityLogs { get; set; }

        IDbSet<Entities.Activity.ActivityTypeDescriptor> ActivityTypeDescriptors { get; set; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
    }
}

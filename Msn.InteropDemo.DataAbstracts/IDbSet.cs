using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Msn.InteropDemo.DataAbstracts
{
    public interface IDbSet<TEntity> : IQueryable<TEntity>, IEnumerable<TEntity>, IEnumerable, IQueryable, IAsyncEnumerableAccessor<TEntity>, IInfrastructure<IServiceProvider>, IListSource where TEntity : class
    {
        LocalView<TEntity> Local { get; }

        //
        // Summary:
        //     Begins tracking the given entity, and any other reachable entities that are not
        //     already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        //     state of only a single entity.
        //
        // Parameters 
        //   entity:
        //     The entity to add.
        //
        // Returns:
        //     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1 for the entity.
        //     The entry provides access to change tracking information and operations for the
        //     entity.
        TEntity Add(TEntity entity);
        //
        // Summary:
        //     Begins tracking the given entity, and any other reachable entities that are not
        //     already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     This method is async only to allow special value generators, such as the one
        //     used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        //     to access the database asynchronously. For all other cases the non async method
        //     should be used.
        //     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        //     state of only a single entity.
        //
        // Parameters:
        //   entity:
        //     The entity to add.
        //
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // Returns:
        //     A task that represents the asynchronous Add operation. The task result contains
        //     the Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1 for the entity.
        //     The entry provides access to change tracking information and operations for the
        //     entity.
        void AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        //
        // Summary:
        //     Begins tracking the given entities, and any other reachable entities that are
        //     not already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //
        // Parameters:
        //   entities:
        //     The entities to add.
        void AddRange(IEnumerable<TEntity> entities);
        //
        // Summary:
        //     Begins tracking the given entities, and any other reachable entities that are
        //     not already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //
        // Parameters:
        //   entities:
        //     The entities to add.
        void AddRange(params TEntity[] entities);
        //
        // Summary:
        //     Begins tracking the given entities, and any other reachable entities that are
        //     not already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     This method is async only to allow special value generators, such as the one
        //     used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        //     to access the database asynchronously. For all other cases the non async method
        //     should be used.
        //
        // Parameters:
        //   entities:
        //     The entities to add.
        //
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // Returns:
        //     A task that represents the asynchronous operation.
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        //
        // Summary:
        //     Begins tracking the given entities, and any other reachable entities that are
        //     not already being tracked, in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state such that they will be inserted into the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     This method is async only to allow special value generators, such as the one
        //     used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
        //     to access the database asynchronously. For all other cases the non async method
        //     should be used.
        //
        // Parameters:
        //   entities:
        //     The entities to add.
        //
        // Returns:
        //     A task that represents the asynchronous operation.
        Task AddRangeAsync(params TEntity[] entities);
        //
        // Summary:
        //     Begins tracking the given entity in the Microsoft.EntityFrameworkCore.EntityState.Unchanged
        //     state such that no operation will be performed when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     A recursive search of the navigation properties will be performed to find reachable
        //     entities that are not already being tracked by the context. These entities will
        //     also begin to be tracked by the context. If a reachable entity has its primary
        //     key value set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Unchanged
        //     state. If the primary key value is not set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state. An entity is considered to have its primary key value set if the primary
        //     key property is set to anything other than the CLR default for the property type.
        //     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        //     state of only a single entity.
        //
        // Parameters:
        //   entity:
        //     The entity to attach.
        //
        // Returns:
        //     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry for the entity.
        //     The entry provides access to change tracking information and operations for the
        //     entity.
        void Attach(TEntity entity);
        //
        // Summary:
        //     Begins tracking the given entities in the Microsoft.EntityFrameworkCore.EntityState.Unchanged
        //     state such that no operation will be performed when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     A recursive search of the navigation properties will be performed to find reachable
        //     entities that are not already being tracked by the context. These entities will
        //     also begin to be tracked by the context. If a reachable entity has its primary
        //     key value set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Unchanged
        //     state. If the primary key value is not set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state. An entity is considered to have its primary key value set if the primary
        //     key property is set to anything other than the CLR default for the property type.
        //
        // Parameters:
        //   entities:
        //     The entities to attach.
        void AttachRange(IEnumerable<TEntity> entities);
        //
        // Summary:
        //     Begins tracking the given entities in the Microsoft.EntityFrameworkCore.EntityState.Unchanged
        //     state such that no operation will be performed when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     A recursive search of the navigation properties will be performed to find reachable
        //     entities that are not already being tracked by the context. These entities will
        //     also begin to be tracked by the context. If a reachable entity has its primary
        //     key value set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Unchanged
        //     state. If the primary key value is not set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state. An entity is considered to have its primary key value set if the primary
        //     key property is set to anything other than the CLR default for the property type.
        //
        // Parameters:
        //   entities:
        //     The entities to attach.
        void AttachRange(params TEntity[] entities);
        //
        // Summary:
        //     Determines whether the specified object is equal to the current object.
        //
        // Parameters:
        //   obj:
        //     The object to compare with the current object.
        //
        // Returns:
        //     true if the specified object is equal to the current object; otherwise, false.
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
        //
        // Summary:
        //     Finds an entity with the given primary key values. If an entity with the given
        //     primary key values is being tracked by the context, then it is returned immediately
        //     without making a request to the database. Otherwise, a query is made to the database
        //     for an entity with the given primary key values and this entity, if found, is
        //     attached to the context and returned. If no entity is found, then null is returned.
        //
        // Parameters:
        //   keyValues:
        //     The values of the primary key for the entity to be found.
        //
        // Returns:
        //     The entity found, or null.
        TEntity Find(params object[] keyValues);
        //
        // Summary:
        //     Finds an entity with the given primary key values. If an entity with the given
        //     primary key values is being tracked by the context, then it is returned immediately
        //     without making a request to the database. Otherwise, a query is made to the database
        //     for an entity with the given primary key values and this entity, if found, is
        //     attached to the context and returned. If no entity is found, then null is returned.
        //
        // Parameters:
        //   keyValues:
        //     The values of the primary key for the entity to be found.
        //
        //   cancellationToken:
        //     A System.Threading.CancellationToken to observe while waiting for the task to
        //     complete.
        //
        // Returns:
        //     The entity found, or null.
        Task<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken);
        //
        // Summary:
        //     Finds an entity with the given primary key values. If an entity with the given
        //     primary key values is being tracked by the context, then it is returned immediately
        //     without making a request to the database. Otherwise, a query is made to the database
        //     for an entity with the given primary key values and this entity, if found, is
        //     attached to the context and returned. If no entity is found, then null is returned.
        //
        // Parameters:
        //   keyValues:
        //     The values of the primary key for the entity to be found.
        //
        // Returns:
        //     The entity found, or null.
        Task<TEntity> FindAsync(params object[] keyValues);
        //
        // Summary:
        //     Serves as the default hash function.
        //
        // Returns:
        //     A hash code for the current object.
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();
        //
        // Summary:
        //     Begins tracking the given entity in the Microsoft.EntityFrameworkCore.EntityState.Deleted
        //     state such that it will be removed from the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //
        // Parameters:
        //   entity:
        //     The entity to remove.
        //
        // Returns:
        //     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry`1 for the entity.
        //     The entry provides access to change tracking information and operations for the
        //     entity.
        //
        // Remarks:
        //     If the entity is already tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state then the context will stop tracking the entity (rather than marking it
        //     as Microsoft.EntityFrameworkCore.EntityState.Deleted) since the entity was previously
        //     added to the context and does not exist in the database.
        //     Any other reachable entities that are not already being tracked will be tracked
        //     in the same way that they would be if Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0)
        //     was called before calling this method. This allows any cascading actions to be
        //     applied when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
        //     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        //     state of only a single entity.
        void Remove(TEntity entity);
        //
        // Summary:
        //     Begins tracking the given entities in the Microsoft.EntityFrameworkCore.EntityState.Deleted
        //     state such that they will be removed from the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //
        // Parameters:
        //   entities:
        //     The entities to remove.
        //
        // Remarks:
        //     If any of the entities are already tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state then the context will stop tracking those entities (rather than marking
        //     them as Microsoft.EntityFrameworkCore.EntityState.Deleted) since those entities
        //     were previously added to the context and do not exist in the database.
        //     Any other reachable entities that are not already being tracked will be tracked
        //     in the same way that they would be if Microsoft.EntityFrameworkCore.DbSet`1.AttachRange(System.Collections.Generic.IEnumerable{`0})
        //     was called before calling this method. This allows any cascading actions to be
        //     applied when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
        void RemoveRange(IEnumerable<TEntity> entities);
        //
        // Summary:
        //     Begins tracking the given entities in the Microsoft.EntityFrameworkCore.EntityState.Deleted
        //     state such that they will be removed from the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //
        // Parameters:
        //   entities:
        //     The entities to remove.
        //
        // Remarks:
        //     If any of the entities are already tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state then the context will stop tracking those entities (rather than marking
        //     them as Microsoft.EntityFrameworkCore.EntityState.Deleted) since those entities
        //     were previously added to the context and do not exist in the database.
        //     Any other reachable entities that are not already being tracked will be tracked
        //     in the same way that they would be if Microsoft.EntityFrameworkCore.DbSet`1.AttachRange(`0[])
        //     was called before calling this method. This allows any cascading actions to be
        //     applied when Microsoft.EntityFrameworkCore.DbContext.SaveChanges is called.
        void RemoveRange(params TEntity[] entities);
        //
        // Summary:
        //     Returns a string that represents the current object.
        //
        // Returns:
        //     A string that represents the current object.
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();
        //
        // Summary:
        //     Begins tracking the given entity in the Microsoft.EntityFrameworkCore.EntityState.Modified
        //     state such that it will be updated in the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     All properties of the entity will be marked as modified. To mark only some properties
        //     as modified, use Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0) to begin tracking
        //     the entity in the Microsoft.EntityFrameworkCore.EntityState.Unchanged state and
        //     then use the returned Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry
        //     to mark the desired properties as modified.
        //     A recursive search of the navigation properties will be performed to find reachable
        //     entities that are not already being tracked by the context. These entities will
        //     also begin to be tracked by the context. If a reachable entity has its primary
        //     key value set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Modified
        //     state. If the primary key value is not set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state. An entity is considered to have its primary key value set if the primary
        //     key property is set to anything other than the CLR default for the property type.
        //     Use Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry.State to set the
        //     state of only a single entity.
        //
        // Parameters:
        //   entity:
        //     The entity to update.
        //
        // Returns:
        //     The Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry for the entity.
        //     The entry provides access to change tracking information and operations for the
        //     entity.
        void Update(TEntity entity);
        //
        // Summary:
        //     Begins tracking the given entities in the Microsoft.EntityFrameworkCore.EntityState.Modified
        //     state such that they will be updated in the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     All properties of each entity will be marked as modified. To mark only some properties
        //     as modified, use Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0) to begin tracking
        //     each entity in the Microsoft.EntityFrameworkCore.EntityState.Unchanged state
        //     and then use the returned Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry
        //     to mark the desired properties as modified.
        //     A recursive search of the navigation properties will be performed to find reachable
        //     entities that are not already being tracked by the context. These entities will
        //     also begin to be tracked by the context. If a reachable entity has its primary
        //     key value set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Modified
        //     state. If the primary key value is not set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state. An entity is considered to have its primary key value set if the primary
        //     key property is set to anything other than the CLR default for the property type.
        //
        // Parameters:
        //   entities:
        //     The entities to update.
        void UpdateRange(IEnumerable<TEntity> entities);
        //
        // Summary:
        //     Begins tracking the given entities in the Microsoft.EntityFrameworkCore.EntityState.Modified
        //     state such that they will be updated in the database when Microsoft.EntityFrameworkCore.DbContext.SaveChanges
        //     is called.
        //     All properties of each entity will be marked as modified. To mark only some properties
        //     as modified, use Microsoft.EntityFrameworkCore.DbSet`1.Attach(`0) to begin tracking
        //     each entity in the Microsoft.EntityFrameworkCore.EntityState.Unchanged state
        //     and then use the returned Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry
        //     to mark the desired properties as modified.
        //     A recursive search of the navigation properties will be performed to find reachable
        //     entities that are not already being tracked by the context. These entities will
        //     also begin to be tracked by the context. If a reachable entity has its primary
        //     key value set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Modified
        //     state. If the primary key value is not set then it will be tracked in the Microsoft.EntityFrameworkCore.EntityState.Added
        //     state. An entity is considered to have its primary key value set if the primary
        //     key property is set to anything other than the CLR default for the property type.
        //
        // Parameters:
        //   entities:
        //     The entities to update.
        void UpdateRange(params TEntity[] entities);
    }
}

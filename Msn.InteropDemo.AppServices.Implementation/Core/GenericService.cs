using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Common.OperationResults;
using System;

namespace Msn.InteropDemo.AppServices.Implementation.Core
{
    public abstract class GenericService<TEntity> : GenericService<TEntity, int> where TEntity : Entities.Core.EntityAuditable<int>, new()
    {
        public GenericService(ICurrentContext currentContext, ILogger logger) : base(currentContext, logger) { }
    }

    public abstract class GenericService<TEntity, TKey> : GenericServiceReadOnly<TEntity, TKey>, IGenericService<TEntity, TKey> where TEntity : Entities.Core.EntityAuditable<TKey>, new()
    {
        public GenericService(ICurrentContext currentContext, ILogger logger) : base(currentContext, logger) { }

        public virtual OperationResult<TKey> Delete(TEntity entity)
        {
            var op = new OperationResult<TKey>();

            try
            {
                if (CurrentContext.DataContext.Entry(entity).State == EntityState.Detached)
                {
                    DbSetEntity.Attach(entity);
                }
                DbSetEntity.Remove(entity);
                CurrentContext.DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                op.AddError(ex.Message);
                Logger.LogError(ex.ToString());
            }

            return op;
        }

        public virtual OperationResult<TKey> Delete<TModel>(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);
            return Delete(entity);
        }

        public virtual OperationResult<TKey> Save(TEntity entity)
        {
            var op = new OperationResult<TKey>();

            try
            {
                entity.CreatedDateTime = DateTime.Now;
                entity.CreatedUserId = CurrentContext.GetCurrentUserId;
                DbSetEntity.Add(entity);
                CurrentContext.DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                op.AddError(ex.Message);
                Logger.LogError(ex.ToString());
            }

            return op;
        }

        public virtual OperationResult<TKey> Save<TModel>(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);
            var op = Save(entity);
            op.Id = entity.Id;
            return op;
        }

        public virtual OperationResult<TKey> Update(TEntity entity)
        {
            var op = new OperationResult<TKey>();

            try
            {
                entity.UpdatedDateTime = DateTime.Now;
                entity.UpdatedUserId = CurrentContext.GetCurrentUserId;
                DbSetEntity.Attach(entity);
                CurrentContext.DataContext.Entry(entity).State = EntityState.Modified;
                CurrentContext.DataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                op.AddError(ex.Message);
                Logger.LogError(ex.ToString());
            }

            return op;
        }

        public virtual OperationResult<TKey> Update<TModel>(TModel model)
        {
            var entity = Mapper.Map<TEntity>(model);
            return Update(entity);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Microsoft.EntityFrameworkCore;

namespace Msn.InteropDemo.AppServices.Implementation.Core
{
    public abstract class GenericServiceReadOnly<TEntity> : GenericServiceReadOnly<TEntity, int> where TEntity : class, new()
    {
        public GenericServiceReadOnly(ICurrentContext currentContext, ILogger logger) : base(currentContext, logger) { }
    }

    public abstract class GenericServiceReadOnly<TEntity, TKey> : ServiceBase, IGenericServiceReadOnly<TEntity, TKey> where TEntity : class, new()
    {
        public GenericServiceReadOnly(ICurrentContext currentContext, ILogger logger)
        {
            CurrentContext = currentContext;
            DbSetEntity = currentContext.DataContext.Set<TEntity>();
            Logger = logger;
        }

        protected ICurrentContext CurrentContext { get; private set; }

        protected ILogger Logger { get; }

        protected DbSet<TEntity> DbSetEntity { get; private set; }

        public virtual bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            return DbSetEntity.Any(filter);
        }

        public virtual Task<TEntity> FindAsync(params object[] keyValues)
        {
            return DbSetEntity.FindAsync(keyValues);
        }

        public virtual IList<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null
            )
        {
            IQueryable<TEntity> query = DbSetEntity;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }

            if (take != null)
            {
                query = query.Take(take.Value);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IList<TModel> Get<TModel>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null)
        {
            var entities = Get(filter, orderBy, includeProperties, skip, take);
            var models = Mapper.Map<IList<TModel>>(entities);
            return models;
        }

        public virtual int GetRecordCount(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
            {
                return DbSetEntity.Count(filter);
            }

            return DbSetEntity.Count();
        }

        public virtual TModel GetById<TModel>(Expression<Func<TEntity, bool>> criteria, string includeProperties = "")
        {
            var entity = Get(filter: criteria, includeProperties: includeProperties).FirstOrDefault();
            var model = Mapper.Map<TModel>(entity);
            return model;
        }

        public virtual TModel GetById<TModel>(TKey id)
        {
            var entity = DbSetEntity.Find(id);
            var model = Mapper.Map<TModel>(entity);
            return model;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbSetEntity = null;
                CurrentContext.DataContext.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}

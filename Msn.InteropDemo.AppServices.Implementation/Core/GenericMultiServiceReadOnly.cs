using Microsoft.Extensions.Logging;
using Msn.InteropDemo.AppServices.Core;
using Msn.InteropDemo.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Msn.InteropDemo.AppServices.Implementation.Core
{
    public abstract class GenericMultiServiceReadOnly : ServiceBase, IGenericMultiServiceReadOnly
    {
        public GenericMultiServiceReadOnly(DataContext context, ILogger logger)
        {
            Context = context;
            Logger = logger;
        }

        protected ILogger Logger { get; }

        protected DataContext Context { get; }

        public virtual bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class
        {
            return Context.Set<TEntity>().Any(filter);
        }

        public virtual Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            return Context.Set<TEntity>().FindAsync(keyValues);
        }

        public virtual IList<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null) where TEntity : class
        {

            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
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

        public virtual TModel GetById<TModel, TEntity>(Expression<Func<TEntity, bool>> criteria, string includeProperties = "") where TModel : class where TEntity : class
        {
            var entity = Get(filter: criteria, includeProperties: includeProperties).FirstOrDefault();
            var model = Mapper.Map<TModel>(entity);
            return model;
        }

        public virtual IList<TModel> GetMapped<TModel, TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", int? skip = null, int? take = null) where TModel : class where TEntity : class
        {
            var entity = Get(filter: filter, includeProperties: includeProperties).FirstOrDefault();
            var model = Mapper.Map<List<TModel>>(entity);
            return model;
        }

        public virtual int GetRecordCount<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            if (filter != null)
            {
                return Context.Set<TEntity>().Count(filter);
            }

            return Context.Set<TEntity>().Count();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

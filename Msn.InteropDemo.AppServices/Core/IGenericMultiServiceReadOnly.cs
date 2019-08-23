using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Msn.InteropDemo.AppServices.Core
{
    public interface IGenericMultiServiceReadOnly : IDisposable
    {
        bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;

        Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class;

        IList<TEntity> Get<TEntity>
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null
        ) where TEntity : class;

        IList<TModel> GetMapped<TModel, TEntity>
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null
        ) where TModel : class where TEntity : class;

        int GetRecordCount<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;

        TModel GetById<TModel, TEntity>(Expression<Func<TEntity, bool>> criteria, string includeProperties = "") where TModel : class where TEntity : class;
    }
}

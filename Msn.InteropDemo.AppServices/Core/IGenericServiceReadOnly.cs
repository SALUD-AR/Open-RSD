using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Msn.InteropDemo.AppServices.Core
{
    public interface IGenericServiceReadOnly<TEntity> : IGenericServiceReadOnly<TEntity, int> where TEntity : class, new() { }

    public interface IGenericServiceReadOnly<TEntity, TKey> : IDisposable where TEntity : class, new()
    {
        bool Exists(Expression<Func<TEntity, bool>> filter);

        Task<TEntity> FindAsync(params object[] keyValues);

        IList<TEntity> Get
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null
        );

        IList<TModel> Get<TModel>
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? skip = null,
            int? take = null
        );

        int GetRecordCount(Expression<Func<TEntity, bool>> filter = null);

        TModel GetById<TModel>(Expression<Func<TEntity, bool>> criteria, string includeProperties = "");

        TModel GetById<TModel>(TKey id);

    }
}

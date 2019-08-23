using Msn.InteropDemo.Common.OperationResults;
using System;

namespace Msn.InteropDemo.AppServices.Core
{
    public interface IGenericService<TEntity> : IGenericService<TEntity, int> where TEntity : class, new() { }

    public interface IGenericService<TEntity, TKey> : IDisposable, IGenericServiceReadOnly<TEntity, TKey> where TEntity : class, new()
    {

        OperationResult<TKey> Save(TEntity entity);

        OperationResult<TKey> Save<TModel>(TModel model);

        OperationResult<TKey> Update(TEntity entity);

        OperationResult<TKey> Update<TModel>(TModel model);

        OperationResult<TKey> Delete(TEntity entity);

        OperationResult<TKey> Delete<TModel>(TModel model);

    }
}

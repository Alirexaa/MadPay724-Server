using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Repo.Infrastructure
{
    public interface IRepository<TEntity>  where TEntity:class
    {
        #region SyncMethods
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(Object Id);
        void Delete(TEntity entity);
        void Delete(System.Linq.Expressions.Expression<Func<TEntity,bool>> where);

        TEntity GetById(object Id);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetMany(System.Linq.Expressions.Expression<Func<TEntity,bool>> where);
        TEntity Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> where);
        #endregion

        #region AsyncMethods
        System.Threading.Tasks.Task InsertAsync(TEntity entity);

        System.Threading.Tasks.Task<TEntity> GetByIdAsync(object Id);
        System.Threading.Tasks.Task<IEnumerable <TEntity>> GetAllAsync();
        System.Threading.Tasks.Task<IEnumerable<TEntity>> GetManyAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> where);
        System.Threading.Tasks.Task<TEntity> GetAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> where);

        #endregion

    }
}

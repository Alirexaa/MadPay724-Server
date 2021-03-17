using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        void Delete(Expression<Func<TEntity, bool>> where);

        TEntity GetById(object Id);
        IEnumerable<TEntity> GetAll();
        TEntity Get(Expression<Func<TEntity, bool>> where);
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeEntity);
        #endregion

        #region AsyncMethods
        System.Threading.Tasks.Task InsertAsync(TEntity entity);

        System.Threading.Tasks.Task<TEntity> GetByIdAsync(object Id);
        System.Threading.Tasks.Task<IEnumerable <TEntity>> GetAllAsync();
        System.Threading.Tasks.Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where);
        System.Threading.Tasks.Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeEntity);

        #endregion

    }
}

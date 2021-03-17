using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MadPay724.Repo.Infrastructure
{
    public abstract class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : class
    {
        #region ctor
        private readonly DbContext _db;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(DbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
        }
        #endregion

        #region Insert
        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }
        #endregion

        #region Update
        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentException("there is no entity");
            _dbSet.Update(entity);
        }
        #endregion

        #region Delete
        #region sync
        public void Delete(object Id)
        {
            var entity = GetById(Id);
            if (entity == null)
                throw new ArgumentException("there is no entity");
            _dbSet.Remove(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> where)
        {
            IEnumerable<TEntity> entities = _dbSet.Where(where).AsEnumerable();
            foreach (TEntity item in entities)
            {
                _db.Remove(item);
            }
        }
        #endregion
        #endregion

        #region Get

        #region sync

        public TEntity GetById(object Id)
        {
            return _dbSet.Find(Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> where)
        {
            return _dbSet.Where(where).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "")
        {

            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeentity in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeentity);
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


        #endregion

        #region async
        public async Task<TEntity> GetByIdAsync(object Id)
        {
            return await _dbSet.FindAsync(Id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where)
        {
            return await _dbSet.Where(where).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeEntity = "")
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeentity in includeEntity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeentity);
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }

        }

        #endregion

        #endregion

        #region dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposed = true;
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
        }
        #endregion
    }
}

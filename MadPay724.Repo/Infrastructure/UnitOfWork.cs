using MadPay724.Repo.Repositories.Interface;
using MadPay724.Repo.Repositories.Repo;
using System;
using System.Collections.Generic;
using System.Text;


namespace MadPay724.Repo.Infrastructure
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : Microsoft.EntityFrameworkCore.DbContext, new()
    {

        #region ctor
        protected readonly Microsoft.EntityFrameworkCore.DbContext _db;
        public UnitOfWork()
        {
            _db = new TContext();
        }
        #endregion

        private IUserRepository _userRepository;

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_db);
                }
                return _userRepository;
            }
        }

        

        #region save
        public void Save()
        {
            _db.SaveChanges();

        }

        public System.Threading.Tasks.Task<int> SaveAsync()
        {
            return _db.SaveChangesAsync();
        }

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

        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion
    }
}

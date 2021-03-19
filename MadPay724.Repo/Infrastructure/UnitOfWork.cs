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
        #region private repository
        private IUserRepository _userRepository;
        private IPhotoRepository _photoRepository;
        private ISettingRepository _settingRepository;
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

        public IPhotoRepository PhotoRepository
        {
            get
            {
                if (_photoRepository == null)
                {
                    _photoRepository = new PhotoRepository(_db);
                }
                return _photoRepository;
            }
        }

        public ISettingRepository SettingRepository
        {
            get
            {
                if (_settingRepository == null)
                {
                    _settingRepository = new SettingRepository(_db);
                }
                return _settingRepository;
            }
        }


        #endregion


        #region save
        public bool Save()
        {
            if (_db.SaveChanges() > 0)
                return true;
            else return false;

        }

        public async System.Threading.Tasks.Task<bool> SaveAsync()
        {
            if (await _db.SaveChangesAsync() > 0)
                return true;
            else return false;

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

﻿using MadPay724.Repo.Repositories.Interface;
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

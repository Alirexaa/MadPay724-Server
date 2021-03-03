﻿using System;
using System.Collections.Generic;
using System.Text;


namespace MadPay724.Data.Infrastructure
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

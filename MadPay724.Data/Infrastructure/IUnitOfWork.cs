using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Infrastructure
{
    public interface IUnitOfWork<TContext>: IDisposable where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        void Save();

        System.Threading.Tasks.Task<int> SaveAsync();
    }
}

﻿using MadPay724.Repo.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Repo.Infrastructure
{
    public interface IUnitOfWork<TContext>: IDisposable where TContext : Microsoft.EntityFrameworkCore.DbContext
    {
        IUserRepository UserRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        ISettingRepository SettingRepository { get; }
        IRoleRepository RoleRepository { get; }
        bool Save();

        System.Threading.Tasks.Task<bool> SaveAsync();
    }
}

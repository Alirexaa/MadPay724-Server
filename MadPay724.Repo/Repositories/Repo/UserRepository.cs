﻿using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Models;
using MadPay724.Repo.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MadPay724.Common.Helper;

namespace MadPay724.Repo.Repositories.Repo
{
    public class UserRepository : Infrastructure.Repository<User>, IUserRepository
    {

        private readonly DbContext _db;
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            _db ??= (MadpayDbContext)_db;
        }

        public async Task<bool> UserExist(string username)
        {
            return await GetAsync(p => p.UserName == username) != null;
        }
    }
}

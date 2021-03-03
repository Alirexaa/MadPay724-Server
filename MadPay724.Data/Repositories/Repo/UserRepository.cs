using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Models;
using MadPay724.Data.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Data.Repositories.Repo
{
    public class UserRepository : Infrastructure.Repository<User> ,IUserRepository
    {

        private readonly DbContext _db;
        public UserRepository(DbContext dbContext): base( dbContext)
        {
            _db = _db ?? (MadpayDbContext) _db;
        }

    }
}

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

        public void Delete(object Id)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<User, bool>> where)
        {
            throw new NotImplementedException();
        }

        public User Get(Expression<Func<User, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(Expression<Func<User, bool>> where)
        {
            throw new NotImplementedException();
        }

        public User GetById(object Id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(object Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetMany(Expression<Func<User, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetManyAsync(Expression<Func<User, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Insert(User entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}

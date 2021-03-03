using MadPay724.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MadPay724.Common.Helper;
using MadPay724.Repo.Infrastructure;
using MadPay724.Data.DatabaseContext;
using MadPay724.Services.Site.Admin.Auth.Interface;

namespace MadPay724.Services.Site.Admin.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        public AuthService(IUnitOfWork<MadpayDbContext> dbContext)
        {
            _db = dbContext;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _db.UserRepository.GetAsync(p => p.UserName == username);
            if (user == null)
            {
                return null;
            }
            if (!Utilities.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            Utilities.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _db.UserRepository.InsertAsync(user);
            await _db.SaveAsync();
            return user;
        }
    }
}

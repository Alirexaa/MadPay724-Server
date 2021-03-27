using MadPay724.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using MadPay724.Common.Helper;
using MadPay724.Repo.Infrastructure;
using MadPay724.Data.DatabaseContext;
using MadPay724.Services.Site.Admin.Auth.Interface;
using MadPay724.Common.Helper.Interface;

namespace MadPay724.Services.Site.Admin.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IUtilities _utilities;
        public AuthService(IUnitOfWork<MadpayDbContext> dbContext, IUtilities utilitie)
        {
            _db = dbContext;
            _utilities = utilitie;
        }
        public async Task<User> Login(string username, string password)
        {
            var users = await _db.UserRepository.GetManyAsync(p => p.UserName == username, null, "Photos");
            var user = users.SingleOrDefault();
            if (user == null)
                return null;

            //if (!_utilities.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            //    return null;

            return user;
        }

        public async Task<User> Register(User user, Photo photo, string password)
        {
            byte[] passwordHash, passwordSalt;
            _utilities.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            //user.PasswordHash = passwordHash;
            //user.PasswordSalt = passwordSalt;

            await _db.UserRepository.InsertAsync(user);
            await _db.PhotoRepository.InsertAsync(photo);
            if (await _db.SaveAsync())
            {
                return user;
            }
            return null;

        }

        public async Task<bool> AddUserPhoto(Photo photo)
        {
            await _db.PhotoRepository.InsertAsync(photo);
            if (await _db.SaveAsync())
            {
                return true;
            }
            return false;
        }
    
    }
}

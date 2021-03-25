using MadPay724.Common.Helper;
using MadPay724.Common.Helper.Interface;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.UserServices.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Services.Site.Admin.UserServices.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IUtilities _utilities;
        public UserService(IUnitOfWork<MadpayDbContext> dbContext, IUtilities utilities)
        {
            _db = dbContext;
            _utilities = utilities;
        }
        public async Task<User> GetUserForChangingPassword(string id, string password)
        {
            var user = await _db.UserRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            if (!_utilities.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public async Task<bool> UpdateUserPassword(User userFromReo, string newPassword)
        {

            byte[] passwordHash, passwordSalt;
            _utilities.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
            userFromReo.PasswordHash = passwordHash;
            userFromReo.PasswordSalt = passwordSalt;
            _db.UserRepository.Update(userFromReo);
            return await _db.SaveAsync();
        }
    }
}

using MadPay724.Common.Helper;
using MadPay724.Common.Helper.Interface;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Seed.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadPay724.Services.Seed.Service
{

    public class SeedService : ISeedService
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IUtilities _utilities;
        public SeedService(IUnitOfWork<MadpayDbContext> dbContext, IUtilities utilities)
        {
            _db = dbContext;
            _utilities = utilities;
        }

        public void SeedUsers()
        {
            var userData = System.IO.File.ReadAllText("wwwroot/Files/Json/Seed/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                _utilities.CreatePasswordHash("123456789", out passwordHash, out passwordSalt);
                user.UserName = user.UserName.ToLower().Trim();
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _db.UserRepository.Insert(user);
            }
            _db.Save();
        }

        public async Task SeedUsersAsync()
        {
            var userData = System.IO.File.ReadAllText("Files/Json/Seed/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                _utilities.CreatePasswordHash("123456789", out passwordHash, out passwordSalt);
                user.UserName = user.UserName.ToLower().Trim();
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _db.UserRepository.InsertAsync(user);
            }
            await _db.SaveAsync();
        }
    }
}


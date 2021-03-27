using MadPay724.Common.Helper;
using MadPay724.Common.Helper.Interface;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Seed.Interface;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadPay724.Services.Seed.Service
{

    public class SeedService : ISeedService
    {
        //private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IUtilities _utilities;
        private readonly UserManager<User> _userManager;
        //public SeedService(IUnitOfWork<MadpayDbContext> dbContext, IUtilities utilities)
        //{
        //    _db = dbContext;
        //    _utilities = utilities;
        //}

        public SeedService(UserManager<User> userManager, IUtilities utilities)
        {
            _userManager = userManager;
            _utilities = utilities;
        }
        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("wwwroot/Files/Json/Seed/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    user.Id = Guid.NewGuid().ToString();
                    user.UserName = user.UserName.ToLower().Trim();
                    var result =_userManager.CreateAsync(user, "123456789").Result;
                } 
            }
        }

        public async Task SeedUsersAsync()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("wwwroot/Files/Json/Seed/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach (var user in users)
                {
                    user.Id = new Guid().ToString();
                    user.UserName = user.UserName.ToLower().Trim();
                    await _userManager.CreateAsync(user, "123456789");

                } 
            }
        }
    }
}


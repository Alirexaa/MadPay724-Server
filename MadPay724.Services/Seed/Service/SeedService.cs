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
        private readonly RoleManager<Role> _roleManager;
        //public SeedService(IUnitOfWork<MadpayDbContext> dbContext, IUtilities utilities)
        //{
        //    _db = dbContext;
        //    _utilities = utilities;
        //}

        public SeedService(UserManager<User> userManager, RoleManager<Role> roleManager, IUtilities utilities)
        {
            _userManager = userManager;
            _utilities = utilities;
            _roleManager = roleManager;
        }
        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("wwwroot/Files/Json/Seed/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role {Name="Admin"},
                    new Role {Name="User"},
                    new Role {Name="Blogger"},
                    new Role {Name="Accountant"},
                    new Role {Name="Backer"},
                };

                foreach (var role in roles)
                {
                    var roleResult = _roleManager.CreateAsync(role).Result;
                }

                foreach (var user in users)
                {
                    user.UserName = user.UserName.ToLower().Trim();
                    _userManager.CreateAsync(user, "123456789").Wait();
                    _userManager.AddToRoleAsync(user, "User").Wait();
                }

                //Create AdminUser

                var adminUser = new User
                {
                    UserName = "admin@maday724.com",
                    Name = "Admin",
                    DateOfBirth = DateTime.Now,
                    LastActive = DateTime.Now,
                    Address = "Street 1",

                };
                var result = _userManager.CreateAsync(adminUser,"123456789").Result;
                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("admin@maday724.com").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Blogger", "Accountant", "Backer" }).Wait();
                }
                //Create Blogger

                var bloggerUser = new User
                {
                    UserName = "blogger@maday724.com",
                    Name = "Blogger",
                    DateOfBirth = DateTime.Now,
                    LastActive = DateTime.Now,
                    Address = "Street 1",
                };
                var resultblogger = _userManager.CreateAsync(bloggerUser, "123456789").Result;
                if (resultblogger.Succeeded)
                {
                    var blogger = _userManager.FindByNameAsync("blogger@maday724.com").Result;
                    _userManager.AddToRolesAsync(blogger, new[] { "Blogger" }).Wait();
                }
                //Create Backer

                var BackerUser = new User
                {
                    UserName = "backer@maday724.com",
                    Name = "Backer",
                    DateOfBirth = DateTime.Now,
                    LastActive = DateTime.Now,
                    Address = "Street 1",
                };
                var resultBacker = _userManager.CreateAsync(BackerUser, "123456789").Result;
                if (resultBacker.Succeeded)
                {
                    var backer = _userManager.FindByNameAsync("backer@maday724.com").Result;
                    _userManager.AddToRolesAsync(backer, new[] { "Backer" }).Wait();
                }

                //Create Accountant

                var AccountantUser = new User
                {
                    UserName = "accountant@maday724.com",
                    Name = "Accountant",
                    DateOfBirth = DateTime.Now,
                    LastActive = DateTime.Now,
                    Address = "Street 1",
                };
                var resultAccountant = _userManager.CreateAsync(AccountantUser, "123456789").Result;
                if (resultAccountant.Succeeded)
                {
                    var accountant = _userManager.FindByNameAsync("accountant@maday724.com").Result;
                    _userManager.AddToRolesAsync(accountant, new[] { "Accountant" }).Wait();
                }
            }
        }
    }
}


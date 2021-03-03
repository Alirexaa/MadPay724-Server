using MadPay724.Data.DatabaseContext;
using MadPay724.Repo.Infrastructure;
using MadPay724.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MadPay724.Services.Auth.Interface;
using MadPay724.Services.Auth.Service;

namespace MadPay724.Presentation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IAuthService _authService;
        public ValueController(IUnitOfWork<MadpayDbContext> dbContext, IAuthService authService)
        {
            _db = dbContext;
            _authService = authService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var user = new User()
            {
                Address = "",
                City = "",
                DataOfBirth = "",
                Gender = "",
                IsActive = true,
                Name = "",

                PasswordHash = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, },
                PasswordSalt = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, },

                PhoneNumber = "",
                Status = true,
                UserName = ""
            };
            var createdUser = await _authService.Register(user, "123456");

            return Ok(createdUser);
        }

    }
}

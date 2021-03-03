using MadPay724.Common.ErrorAndMessge;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.Auth.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MadPay724.Presentation.Controllers.Site.Admin
{
    [Route("site/admin/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IAuthService _authService;
        public AuthController(IUnitOfWork<MadpayDbContext> dbContext, IAuthService authService)
        {
            _db = dbContext;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister userForRegister)
        {
            userForRegister.UserName = userForRegister.UserName.ToLower();
            if (await _db.UserRepository.UserExist(userForRegister.UserName))
                return BadRequest(new ReturnMessage()
                {
                    Status = false,
                    Title = Resource.ErrorMessages.Error,
                    Message = Resource.ErrorMessages.ExistUserMessage,
                    Code = "404"

                }
                    );

            var userToCreat = new User()
            {
                UserName = userForRegister.UserName,
                Address = "",
                City = "",
                DataOfBirth = DateTime.Now,
                Gender = true,
                IsActive = true,
                Status = true,
                Name = userForRegister.Name,
                PhoneNumber = userForRegister.PhoneNumber
            };

            var createdUser = await _authService.Register(userToCreat, userForRegister.Password);

            return StatusCode(201);
        }



    }
}

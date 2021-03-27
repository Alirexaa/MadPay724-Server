using AutoMapper;
using MadPay724.Common.ErrorAndMessge;
using MadPay724.Common.Helper.Interface;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.Auth.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace MadPay724.Presentation.Controllers.Site.Admin.V1
{
    [ApiExplorerSettings(GroupName = "SiteApiV1")]
    [Route("site/admin/v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IUtilities _utilities;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IUnitOfWork<MadpayDbContext> dbContext, IAuthService authService,
            IConfiguration config, ILogger<AuthController> logger, IMapper mapper, IUtilities utilities, UserManager<User> userManager, SignInManager<User> signInManage)
        {
            _db = dbContext;
            _authService = authService;
            _config = config;
            _logger = logger;
            _mapper = mapper;
            _utilities = utilities;
            _userManager = userManager;
            _signInManager = signInManage;
        }

        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            userForRegister.UserName = userForRegister.UserName.ToLower();

            if (await _userManager.FindByNameAsync(userForRegister.UserName) != null)
            {
                return Conflict(new ReturnMessage()
                {
                    Status = false,
                    Title = Resource.ErrorMessages.Error,
                    Message = Resource.ErrorMessages.ExistUserMessage,
                    Code = "409"
                });
            }
            var userToCreate = new User()
            {
                UserName = userForRegister.UserName,
                Address = "",
                City = "",
                DateOfBirth = DateTime.Now,
                Gender = true,
                IsActive = true,
                Status = true,
                Name = userForRegister.Name,
                PhoneNumber = userForRegister.PhoneNumber
            };

            var photoToCreate = new Photo()
            {
                UserId = userToCreate.Id,
                Description = "Profile Pic",
                Alt = "Profile Pic",
                IsMain = true,
                Url = string.Format($"{Request.Scheme}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? "" }/wwwroot/Files/Images/ProfilePic.png"),
                PublicId = "0",
            };


            var result = await _userManager.CreateAsync(userToCreate, userForRegister.Password);

            if (result.Succeeded)
            {
                await _authService.AddUserPhoto(photoToCreate);
                var userForReturn = _mapper.Map<UserDetailDto>(userToCreate);
                return CreatedAtRoute("GetUser", new { controller = "User", id = userToCreate.Id }, userForReturn);
            }
            else
            {
                _logger.LogWarning($"user : {userForRegister.Name}  Email: {userForRegister.UserName} {Resource.ErrorMessages.DbErrorRegister} ");
                return BadRequest(new ReturnMessage()
                {
                    Code = "400",
                    Message = Resource.ErrorMessages.NoRegister,
                    Status = false,
                    Title = Resource.ErrorMessages.Error
                });
            }


        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            var userFromRepo = await _userManager.FindByNameAsync(userForLoginDto.UserName);

            if (userFromRepo == null)
            {
                return Unauthorized(new ReturnMessage()
                {
                    Status = false,
                    Title = Resource.ErrorMessages.Error,
                    Message = Resource.ErrorMessages.WrongEmailOrPassword,
                    Code = "401"

                });
            }
            var result = await _signInManager.CheckPasswordSignInAsync(userFromRepo, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.Include(p => p.Photos)
                    .FirstOrDefault(u => u.NormalizedUserName == userForLoginDto.UserName.ToUpper());

                var userForReturn = _mapper.Map<UserDetailDto>(appUser);

                _logger.LogInformation($"user {userFromRepo.Name} - {userFromRepo.Id} logged in. ");

                return Ok(new
                {
                    token = _utilities.GenerateJwtToken(appUser, userForLoginDto.IsRemember),
                    userForReturn

                });
            }

            else
            {
                return Unauthorized(new ReturnMessage()
                {
                    Status = false,
                    Title = Resource.ErrorMessages.Error,
                    Message = Resource.ErrorMessages.WrongEmailOrPassword,
                    Code = "401"

                });
            }




        }
    }
}

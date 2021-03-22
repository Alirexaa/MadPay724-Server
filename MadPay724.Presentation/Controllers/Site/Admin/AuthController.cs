using MadPay724.Common.ErrorAndMessge;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.Auth.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


namespace MadPay724.Presentation.Controllers.Site.Admin
{
    [ApiExplorerSettings(GroupName = "SiteApi")]
    [Authorize]
    [Route("site/admin/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUnitOfWork<MadpayDbContext> dbContext, IAuthService authService,
            IConfiguration config,ILogger<AuthController> logger)
        {
            _db = dbContext;
            _authService = authService;
            _config = config;
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            userForRegister.UserName = userForRegister.UserName.ToLower();
            if (await _db.UserRepository.UserExist(userForRegister.UserName))
                return StatusCode(409, new ReturnMessage()
                {
                    Status = false,
                    Title = Resource.ErrorMessages.Error,
                    Message = Resource.ErrorMessages.ExistUserMessage,
                    Code = "409"

                }
                    );

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
                UserId =userToCreate.Id,
                Description= "Profile Pic",
                Alt= "Profile Pic",
                IsMain= true,
                Url= string.Format($"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}/wwwroot/Files/Images/ProfilePic.png"),
                PublicId="0",
            };

            var createdUser = await _authService.Register(userToCreate, photoToCreate, userForRegister.Password);

            return StatusCode(201);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {

            var userFromRepo = await _authService.Login(userForLoginDto.UserName, userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized(new ReturnMessage()
                {
                    Status = false,
                    Title = Resource.ErrorMessages.Error,
                    Message = Resource.ErrorMessages.WrongEmailOrPassword,
                    Code = "401"

                });

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDes = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = userForLoginDto.IsRemember ? DateTime.Now.AddDays(1) : DateTime.Now.AddHours(2),
                SigningCredentials = creds
            };
            var tokenHandeler = new JwtSecurityTokenHandler();
            var token = tokenHandeler.CreateToken(tokenDes);

            _logger.LogInformation($"user {userFromRepo.Name} - {userFromRepo.Id} logged in. ");

            return Ok(new
            {
                token = tokenHandeler.WriteToken(token)
            });
        }
    }
}

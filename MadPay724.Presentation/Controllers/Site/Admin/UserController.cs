using AutoMapper;
using MadPay724.Common.Helper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Data.Models;
using MadPay724.Presentation.Helper.Filters;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.UserServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MadPay724.Presentation.Controllers.Site.Admin
{
    [ApiExplorerSettings(GroupName = "SiteApi")]
    [Authorize]
    //[ServiceFilter(typeof(LogFilter))]
    [Route("site/admin/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUnitOfWork<MadpayDbContext> dbContext, IMapper mapper,
            IUserService userService, ILogger<UserController> logger)
        {
            _db = dbContext;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db.UserRepository.GetManyAsync(null, null, "BankCards,Photos");
            var usersToReturn = _mapper.Map<IEnumerable<UsersListDto>>(users);
            return Ok(usersToReturn);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value == id)
            {
                var user = await _db.UserRepository.GetManyAsync(p => p.Id == id, null, "Photos");
                var userToReturn = _mapper.Map<UserDetailDto>(user.SingleOrDefault());
                return Ok(userToReturn);
            }
            else
            {
                _logger.LogWarning($"impermissin action :user {User.FindFirst(ClaimTypes.NameIdentifier).Value} attempted to get information if user: {id}");
                return Unauthorized();
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserForUpdateDto userForUpdate)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value == id)
            {
                var userFromRepo = await _db.UserRepository.GetByIdAsync(id);
                _mapper.Map(userForUpdate, userFromRepo);
                _db.UserRepository.Update(userFromRepo);
                if (await _db.SaveAsync())
                {
                    return Ok();

                }
                else
                {
                    return StatusCode(406);
                }
            }
            else
            {
                _logger.LogWarning($"impermissin action :user {User.FindFirst(ClaimTypes.NameIdentifier).Value} attempted to Update UserProfile of user {id}");
                return Unauthorized();
            }
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("ChangeUserPassword/{id}")]
        [HttpPut]
        public async Task<IActionResult> ChangeUserPassword(string id, PasswordForChangeDto passwordForChangeDto)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value == id)
            {
                var userFromReo = await _userService.GetUserForChangingPassword(id, passwordForChangeDto.OldPassword);
                if (userFromReo == null)
                {
                    return BadRequest(Resource.ErrorMessages.WrongPassword);
                }

                var result = await _userService.UpdateUserPassword(userFromReo, passwordForChangeDto.NewPassword);

                if (result)
                    return Ok(Resource.InformationMessages.ChangedPassword);
                else return BadRequest(Resource.ErrorMessages.NoChangedPassword);
            }
            else
            {
                _logger.LogWarning($"impermissin action :user {User.FindFirst(ClaimTypes.NameIdentifier).Value} attempted to change password of user {id}");
                return Unauthorized();
            }

        }


        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[Route("GetProfileUser/{id}")]
        //[HttpGet]
        //public async Task<IActionResult> GetProfileUser(string id)
        //{
        //    if (User.FindFirst(ClaimTypes.NameIdentifier).Value == id)
        //    {
        //        var user = await _db.UserRepository.GetManyAsync(p => p.Id == id, null, "Photos");
        //        var userToRetun = _mapper.Map<UserDetailDto>(user.SingleOrDefault());
        //        return Ok(userToRetun);
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}

    }

}

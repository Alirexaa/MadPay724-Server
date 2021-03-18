using AutoMapper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MadPay724.Presentation.Controllers.Site.Admin
{
    [ApiExplorerSettings(GroupName = "SiteApi")]
    [Authorize]
    [Route("site/admin/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IMapper _mapper;
        public UserController(IUnitOfWork<MadpayDbContext> dbContext, IMapper mapper)
        {
            _db = dbContext;
            _mapper = mapper;
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
            var user = await _db.UserRepository.GetManyAsync(p => p.Id == id, null, "Photos");
            var userToRetun = _mapper.Map<UserDetailDto>(user.SingleOrDefault());
            return Ok(userToRetun);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id,UserForUpdateDto userForUpdate)
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

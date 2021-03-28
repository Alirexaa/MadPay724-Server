using AutoMapper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.UserServices.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadPay724.Presentation.Controllers.Site.Admin.V1
{
    [ApiExplorerSettings(GroupName = "SiteApiV1")]
    //[ServiceFilter(typeof(LogFilter))]
    [Route("site/adminuser/v1/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger<AdminUserController> _logger;
        private readonly MadpayDbContext _madpayDb;

        public AdminUserController(IUnitOfWork<MadpayDbContext> dbContext, IMapper mapper,
            IUserService userService, ILogger<AdminUserController> logger, MadpayDbContext madpayDb)
        {
            _db = dbContext;
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
            _madpayDb = madpayDb;
        }

        //[AllowAnonymous]
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet(Name = nameof(GetUsers))]
        public async Task<IActionResult> GetUsers()
        {
            //var users = await _db.UserRepository.GetManyAsync(null, null, "BankCards,Photos");
            var users = await (from user in _madpayDb.Users
                               orderby user.UserName
                               select new
                               {
                                   Id = user.Id,
                                   UserName = user.UserName,
                                   Roles = (from userRole in user.UserRoles
                                            join role in _madpayDb.Roles
                                            on userRole.RoleId
                                            equals role.Id
                                            select role.Name
                                             )

                               }).ToListAsync();

            //var usersToReturn = _mapper.Map<IEnumerable<UsersListDto>>(users);
            //var collectionLink = Link.ToCollection(nameof(GetUsers));
            //var collection = new Collection<UsersListDto>()
            //{
            //    Self = collectionLink,
            //    Value = usersToReturn.ToArray()
            //};
            return Ok(users);
        }



    }

}

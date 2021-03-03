using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Infrastructure;
using MadPay724.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadPay724.Presentation.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ValueController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        public ValueController(IUnitOfWork<MadpayDbContext> dbContext)
        {
            _db = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            //var user = new User()
            //{
            //    Address = "",
            //    City = "",
            //    DataOfBirth="",
            //    Gender="",
            //    IsActive=true,
            //    Name="",

            //    PasswordHash=new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, },
            //    PasswordSalt=new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, },

            //    PhoneNumber="",
            //    Status=true,
            //    UserName=""
            //};
            //await _db.UserRepository.InsertAsync(user);
            //await _db.SaveAsync();
            //var model = await _db.UserRepository.GetAllAsync(user);
            //return Ok(model);

            return  Ok("");
        }
        
    }
}

using MadPay724.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Services.Site.Admin.Auth.Interface
{
    public interface IAuthService
    {
        Task<User> Register(User user, Photo photo, string password);
        Task<User> Login(string username, string password);
        Task<bool> AddUserPhoto(Photo photo);
    }
}

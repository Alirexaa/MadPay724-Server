using MadPay724.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Services.Site.Admin.UserServices.Interface
{
    public interface IUserService
    {
        Task<User> GetUserForChangingPassword(string id , string password);
        Task<bool> UpdateUserPassword(User userFromReo, string newPassword);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Services.Site.Admin.Auth.Interface
{
    public interface IAuthService
    {
        Task<Data.Models.User> Register(Data.Models.User user, string password);
        Task<Data.Models.User> Login(string username, string password);
    }
}

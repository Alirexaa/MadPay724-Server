using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Data.Models;

namespace MadPay724.Presentation.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDetailDto>();
            CreateMap<User, UsersListDto>();
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MadPay724.Common.Helper;
using MadPay724.Data.Dto.Common.ION;
using MadPay724.Data.Dto.Site.Admin.BankCard;
using MadPay724.Data.Dto.Site.Admin.Photo;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Data.Models;

namespace MadPay724.Presentation.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //CreateMap<User, UsersListDto>()
            //    .ForMember(dest => dest.Self, opt =>
            //    opt.MapFrom(src => Link.To(nameof(Controllers.Site.Admin.V1.UserController.GetUser), new { id = src.Id })));


            CreateMap<User, UserDetailDto>()
                .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.ToAge())
                );
            CreateMap<User, UsersListDto>();
            CreateMap<Photo, PhotoForUserDetailDto>();
            CreateMap<BankCard, BankCardForUserDetailDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<PhotoFromUserProfileDto, Photo>();
            CreateMap<Photo, PhotoForReturnUserProfileDto>();
        }

    }

}

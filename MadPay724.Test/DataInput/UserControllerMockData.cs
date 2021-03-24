using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Test.DataInput
{
    public static class UserControllerMockData
    {
        public static IEnumerable<User> GetUser()
        {
            return new List<User>()
            {
                new User
                {
                    UserName ="alireza@gmail.com",
                    Name ="TEST",
                    PhoneNumber ="123456",
                    Address ="TEST",
                    City ="TEST",
                    Gender =true,
                    IsActive = true,
                    Status =  true,
                    DateOfBirth = DateTime.Now,
                    Photos = new List<Photo>
                    {
                        new Photo
                        {
                            Url = "TEST",
                            Alt = "TEST",
                            IsMain = true,
                            Description = "TEST",
                        }
                    }

                }
            };


        }
        public static UserDetailDto GetUserDetailDto()
        {

            return new UserDetailDto
            {
                Id = "5a3a2a02-7bbf-41f1-b401-25e7be899d24",
                UserName = "alireza@gmail.com",
                Name = "TEST",
                PhoneNumber = "123456",
                Address = "TEST",
                City = "TEST",
                Gender = true,
                PhotoUrl = "TESt",
                Age = "15",
                LastActive = DateTime.Now

            };


        }

    }
}

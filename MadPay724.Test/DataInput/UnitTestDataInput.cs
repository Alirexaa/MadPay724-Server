using MadPay724.Data.Dto.Services;
using MadPay724.Data.Dto.Site.Admin.Photo;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Test.DataInput
{
    public static class UnitTestDataInput
    {
        public static readonly string SiteAdminVersion = "V1";
        public static readonly string aToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1NDY3Y2VlZS01MjE5LTQxYTItOGQzMS05OGEzYmY3Mjk1M2EiLCJ1bmlxdWVfbmFtZSI6InBvcGVjb3BlbGFuZEBiYXJrYXJhbWEuY29tIiwibmJmIjoxNjE2ODU2MjU1LCJleHAiOjE2MTY5NDI2NTUsImlhdCI6MTYxNjg1NjI1NX0.R9nNsqdwbTUDaAdVax9_YaitClBO3aAjQiSvjuhGBMM";

        public static readonly string userLoggedInUserName = "loveochoa@barkarama.com";
        public static readonly string userLoggedInPassword = "123456789";
        public static readonly string userLoggedInId = "07bc3f0a-6bd4-4ba8-a246-b271f5a97d22";
        public static readonly string userLoggedInPhotoId = "7d1641a4-066b-40e4-ab5c-760536983554";
        public static readonly string userUnLoggedInId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24asdsad";



        public static readonly IEnumerable<User> users = new List<User>()
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



        public static readonly UserDetailDto userDetailDto = new UserDetailDto
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










    public static readonly UserForRegisterDto userForRegister_CanRegister = new UserForRegisterDto
    {
        UserName = "Johniaaaa@gmail.com",
        Name = "John",
        Password = "123456789",
        PhoneNumber = "00123456789"
    };

    public static readonly UserForRegisterDto userForRegister_CantRegister_UserExist = new UserForRegisterDto
    {
        Name = "alireza",
        Password = "12345678",
        PhoneNumber = "11111111",
        UserName = "popecopeland@barkarama.com"
    };

    public static readonly UserForLoginDto userForLogin_CanLogin = new UserForLoginDto
    {
        UserName = "loveochoa@barkarama.com",
        Password = "123456789"
    };

    public static readonly UserForLoginDto userForLogin_Cant_Login = new UserForLoginDto
    {
        Password = "123456321789",
        UserName = "alireza@gmail.com"
    };


    public static readonly UserForUpdateDto userForUpdate = new UserForUpdateDto
    {
        Name = "Ahmad",
        Address = "Street 2",
        PhoneNumber = "11111111",
        Gender = true,
        City = "string"
    };

    public static readonly PasswordForChangeDto passwordForChange_CorrectOldPassword = new PasswordForChangeDto
    {
        NewPassword = "123456789",
        OldPassword = "123456789"
    };

    public static readonly PasswordForChangeDto passwordForChange_WrongOldPassword = new PasswordForChangeDto
    {
        NewPassword = "123456789",
        OldPassword = "123453216789"
    };

    public static readonly UserForUpdateDto userForUpdate_Fail_MoldelState = new UserForUpdateDto
    {
        Name = string.Empty,
        Address = string.Empty,
        PhoneNumber = string.Empty,
        Gender = true,
        City = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tatio"
    };

    public static readonly PasswordForChangeDto passwordForChange = new PasswordForChangeDto
    {
        OldPassword = "123456789",
        NewPassword = "123456789"

    };

    public static readonly PasswordForChangeDto passwordForChange_Fail_ModelState = new PasswordForChangeDto
    {
        NewPassword = string.Empty,
        OldPassword = string.Empty
    };

    public static readonly FileUploadedDto fileUploaded_Success = new FileUploadedDto
    {
        Status = true,
        IsUplodedToLocal = true,
        Message = "UnitTest",
        PublicId = "1",
        Url = "UnitTest"
    };
    public static readonly Photo photoFromRepo_UploadedCloudinary = new Photo
    {
        Id = "5a3a2a02-7bbf-41f1-b401-25e7be899d24",
        PublicId = "1315642312346",
        Url = "UnitTest"
    };

    public static readonly PhotoForReturnUserProfileDto photoForReturnUserProfile = new PhotoForReturnUserProfileDto
    {
        Alt = "UnitTest",
        Description = "UnitTest",
        Url = "UnitTest"

    };
    public static readonly PhotoFromUserProfileDto photoFromUserProfileDto_Valid = new PhotoFromUserProfileDto
    {

    };
    public static readonly FileUploadedDto fileUploaded_Fail = new FileUploadedDto
    {
        Status = false,
        Message = "UnitTest",

    };


}
}

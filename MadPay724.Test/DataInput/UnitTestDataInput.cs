using MadPay724.Data.Dto.Site.Admin.Photo;
using MadPay724.Data.Dto.Site.Admin.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Test.DataInput
{
    public static class UnitTestDataInput
    {
        public static readonly string aToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1YTNhMmEwMi03YmJmLTQxZjEtYjQwMS0yNWU3YmU4OTlkMjQiLCJ1bmlxdWVfbmFtZSI6ImFsaXJlemFAZ21haWwuY29tIiwibmJmIjoxNjE2NTg3MjQ2LCJleHAiOjE2MTY2NzM2NDYsImlhdCI6MTYxNjU4NzI0Nn0.W0v5wKc62JnmNZvOR6YMK8eA8cirBpPPm8zQsfBTqzw";

        public static readonly string userLoggedInUserName = "alireza@gmail.com";
        public static readonly string userLoggedInPassword = "123456789";
        public static readonly string userLoggedInId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
        public static readonly string userLoggedInPhotoId = "7d1641a4-066b-40e4-ab5c-760536983554";
        public static readonly string userUnLoggedInId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24asdsad";


        public static readonly UserForRegisterDto userForRegister_CanRegister = new UserForRegisterDto
        {
            UserName = "John@gmail.com",
            Name = "John",
            Password = "123456789",
            PhoneNumber = "00123456789"
        };

        public static readonly UserForRegisterDto userForRegister_CantRegister_UserExist = new UserForRegisterDto
        {
            Name = "alireza",
            Password = "12345678",
            PhoneNumber = "11111111",
            UserName = "alireza@gmail.com"
        };

        public static readonly UserForLoginDto userForLogin_CanLogin = new UserForLoginDto
        {
            UserName = "alireza@gmail.com",
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

    }
}

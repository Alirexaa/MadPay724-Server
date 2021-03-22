using FluentAssertions;
using MadPay724.Common.ErrorAndMessge;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Presentation;
using MadPay724.Test.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MadPay724.Test.Controllers
{
    public class UserControllerTests : IClassFixture<TestClientProvider<Startup>>
    {
        private HttpClient _client;
        private readonly string AToken;
        public UserControllerTests(TestClientProvider<Startup> testClientProvider)
        {
            _client = testClientProvider.Client;
            AToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1YTNhMmEwMi03YmJmLTQxZjEtYjQwMS0yNWU3YmU4OTlkMjQiLCJ1bmlxdWVfbmFtZSI6ImFsaXJlemFAZ21haWwuY29tIiwibmJmIjoxNjE2NDM3MDQ1LCJleHAiOjE2MTY0NDQyNDUsImlhdCI6MTYxNjQzNzA0NX0.e5I0o7U1ayH4FhEIjKNRgFTnfRXbmodmUYWHTijMUbI";
            //"userName":"alireza@gmail.com"
            //"password":"123456789"
            // Id :"5a3a2a02-7bbf-41f1-b401-25e7be899d24"
        }
        //[Fact]
        //public async Task GetUsers_Unauthorized_User_Cant_GetUsers()
        //{
        //    //Arrange
        //    var request = "/site/admin/user";
        //    //Act
        //    var response = await _client.GetAsync(request);
        //    //Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}
        //[Fact]
        //public async Task GetUsers_authorized_User_Can_GetUsers()
        //{
        //    //Arrange
        //    var request = "/site/admin/user";
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
        //AToken);
        //    //Act
        //    var response = await _client.GetAsync(request);
        //    //Assert
        //    response.EnsureSuccessStatusCode();
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}
        #region GetUserTests
        [Fact]
        public async Task GetUser_Cant_GetAnotherUser()
        {
            //Arrange
            string anotherUserId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24aa";
            var request = "/site/admin/user/" + anotherUserId;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act

            var response = await _client.GetAsync(request);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task GetUser_Can_GetHimSelfUser()
        {
            //Arrange
            string userIdHimSelf = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            //Act
            var request = "/site/admin/user/" + userIdHimSelf;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            var response = await _client.GetAsync(request);
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        #endregion
        #region GetUserTests
        [Fact]
        public async Task UpdateUser_Cant_UpdateAnotherUser()
        {
            //Arrange
            string anotherUserId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24aa";
            var request = new
            {
                Url = "/site/admin/user/" + anotherUserId,
                Body = new UserForUpdateDto
                {
                    Name = "Ahmad",
                    Address = "Street 2",
                    PhoneNumber = "11111111",
                    Gender = true,
                    City = "string"
                }
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = response.Content.ReadAsStringAsync();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task UpdateUser_Can_UpdateHimSelfUser()
        {
            //Arrange
            string UserIdHimSelf = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            var request = new
            {
                Url = "/site/admin/user/" + UserIdHimSelf,
                Body = new UserForUpdateDto
                {
                    Name = "Ahmad",
                    Address = "Street 2",
                    PhoneNumber = "11111111",
                    Gender = true,
                    City = "string"
                }
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = response.Content.ReadAsStringAsync();
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdateUser_ModelStateError()
        {
            //Arrange
            string userIdHimSelf = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            var request = new
            {
                Url = "/site/admin/user/" + userIdHimSelf,
                Body = new UserForUpdateDto
                {
                    Name = string.Empty,
                    Address = string.Empty,
                    PhoneNumber = string.Empty,
                    Gender = true,
                    City = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tatio"
                }
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);

            var controller = new ModelStateControllerTests();

            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = response.Content.ReadAsStringAsync();

            controller.ValidateModelState(request.Body);
            var modelState = controller.ModelState;

            //Assert
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal(4, modelState.Keys.Count());
            Assert.False(modelState.IsValid);
            Assert.True(modelState.ContainsKey("Name") && modelState.ContainsKey("Name") &&
                modelState.ContainsKey("Name") && modelState.ContainsKey("Name"));

        }
        #endregion
        #region ChangeUserPasswordTests
        [Fact]
        public async Task ChangeUserPassword_Cant_ChangeUserPasswordAnotherUser()
        {
            //Arrange
            string anotherUserId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24aa";
            var request = new
            {
                Url = "/site/admin/user/ChangeUserPassword/" + anotherUserId,
                Body = new PasswordForChangeDto
                {
                    NewPassword = "123456789",
                    OldPassword = "123456789"
                }
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task ChangeUserPassword_Can_ChangeUserPasswordHimSelfUser()
        {
            //Arrange
            string UserIdHimSelf = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            var request = new
            {
                Url = "/site/admin/user/ChangeUserPassword/" + UserIdHimSelf,
                Body = new PasswordForChangeDto
                {
                    NewPassword = "123456789",
                    OldPassword = "123456789"
                }
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task ChangeUserPassword_ModelStateError()
        {
            //Arrange
            string userIdHimSelf = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            var request = new
            {
                Url = "/site/admin/user/ChangeUserPassword/" + userIdHimSelf,
                Body = new PasswordForChangeDto
                {
                    NewPassword = string.Empty,
                    OldPassword = string.Empty
                }
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);

            var controller = new ModelStateControllerTests();

            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            controller.ValidateModelState(request.Body);
            var modelState = controller.ModelState;

            //Assert
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal(2, modelState.Keys.Count());
            Assert.False(modelState.IsValid);
            Assert.True(modelState.ContainsKey("NewPassword") && modelState.ContainsKey("OldPassword"));

        }
        [Fact]
        public async Task ChangeUserPassword_Cant_WrongOldPassword()
        {
            //Arrange
            string UserIdHimSelf = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            var request = new
            {
                Url = "/site/admin/user/ChangeUserPassword/" + UserIdHimSelf,
                Body = new PasswordForChangeDto
                {
                    NewPassword = "123456789",
                    OldPassword = "123453216789"
                }
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();
            var valueObj = JsonConvert.DeserializeObject<ReturnMessage>(value);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.False(valueObj.Status);
            Assert.Equal(Resource.ErrorMessages.WrongPassword, valueObj.Message);
        }

        #endregion

    
    }
}

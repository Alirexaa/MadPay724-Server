using FluentAssertions;
using MadPay724.Presentation;
using MadPay724.Test.Providers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            AToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1YTNhMmEwMi03YmJmLTQxZjEtYjQwMS0yNWU3YmU4OTlkMjQiLCJ1bmlxdWVfbmFtZSI6ImFsaXJlemFAZ21haWwuY29tIiwibmJmIjoxNjE2NDIxNzYwLCJleHAiOjE2MTY0Mjg5NjAsImlhdCI6MTYxNjQyMTc2MH0.sWhkg_nL9ocAMYm8b5SM6YIbw3eAJAeOWY8XR8xYtJs";
            //"userName":"alireza@gmail.com"
            //"password":"123456789"
            // Id :"5a3a2a02-7bbf-41f1-b401-25e7be899d24"
        }
        //[Fact]
        //public async void GetUsers_Unauthorized_User_Cant_GetUsers()
        //{
        //    //Arrange
        //    var request = "/site/admin/user";
        //    //Act
        //    var response = await _client.GetAsync(request);
        //    //Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}
        //[Fact]
        //public async void GetUsers_authorized_User_Can_GetUsers()
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
        public async void GetUser_Cant_GetAnotherUser()
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
        public async void GetUser_Can_GetHimSelfUser()
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
        public async void UpdateUser_Cant_UpdateAnotherUser()
        {
            //Arrange
            string anotherUserId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24aa";
            var request = new
            {
                Url = "/site/admin/user/" + anotherUserId,
                Body = new
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

            var response = await _client.PutAsync(request.Url,ContentHelper.GetStringContent(request.Body));
            var value = response.Content.ReadAsStringAsync();
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async void UpdateUser_Can_UpdateHimSelfUser()
        {
            //Arrange
            string UserIdHimSelf = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            var request = new
            {
                Url = "/site/admin/user/" + UserIdHimSelf,
                Body = new
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
        #endregion
        #region ChangeUserPasswordTests
        #endregion


    }
}

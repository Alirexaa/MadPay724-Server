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
            AToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI1YTNhMmEwMi03YmJmLTQxZjEtYjQwMS0yNWU3YmU4OTlkMjQiLCJ1bmlxdWVfbmFtZSI6ImFsaXJlemFAZ21haWwuY29tIiwibmJmIjoxNjE2NDEzNTE5LCJleHAiOjE2MTY0MjA3MTksImlhdCI6MTYxNjQxMzUxOX0.lZDW77ZdpJ_-qSsALOp_UveOOdMnNE0_3mHxcokQk_Q";
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
        [Fact]
        public async void GetUser_Cant_GetAnotherUser()
        {
            //Arrange
            string anotherUserId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24aa";
            var request = "/site/admin/user/" + anotherUserId;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act

            var response =  await _client.GetAsync(request);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async void GetUser_Can_GetSelfUser()
        {
            //Arrange
            string userId = "5a3a2a02-7bbf-41f1-b401-25e7be899d24";
            //Act
            var request = "/site/admin/user/" + userId;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            var response = await _client.GetAsync(request);
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}

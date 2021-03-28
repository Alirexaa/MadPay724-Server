using FluentAssertions;
using MadPay724.Common.ErrorAndMessge;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Presentation;
using MadPay724.Test.DataInput;
using MadPay724.Test.IntegrationTests.Providers;
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

namespace MadPay724.Test.IntegrationTests.Controllers
{
    public class UserControllerTests : IClassFixture<TestClientProvider<Startup>>
    {
        private readonly HttpClient _client;
        private readonly string AToken;
        public UserControllerTests(TestClientProvider<Startup> testClientProvider)
        {
            _client = testClientProvider.Client;
            AToken = UnitTestDataInput.aToken;
        }
        //[Fact]
        //public async Task GetUsers_Unauthorized_User_Cant_GetUsers()
        //{
        //    //Arrange
        //    var request = $"/site/user/{UnitTestDataInput.SiteVersion}/user";
        //    //Act
        //    var response = await _client.GetAsync(request);
        //    //Assert
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        //}
        //[Fact]
        //public async Task GetUsers_authorized_User_Can_GetUsers()
        //{
        //    //Arrange
        //    var request = $"/site/user/{UnitTestDataInput.SiteVersion}/user";
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
            string UserIdAnotherUser = UnitTestDataInput.userUnLoggedInId;
            var request = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/" + UserIdAnotherUser;
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
            string userIdHimSelf = UnitTestDataInput.userLoggedInId;
            //Act
            var request = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/" + userIdHimSelf;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            var response = await _client.GetAsync(request);
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        #endregion
        #region UpdateUserTests
        [Fact]
        public async Task UpdateUser_Cant_UpdateAnotherUser()
        {
            //Arrange
            string UserIdAnotherUser = UnitTestDataInput.userUnLoggedInId;
            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/" + UserIdAnotherUser,
                Body = UnitTestDataInput.userForUpdate
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
            string UserIdHimSelf = UnitTestDataInput.userLoggedInId;
            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/" + UserIdHimSelf,
                Body = UnitTestDataInput.userForUpdate
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
            string userIdHimSelf = UnitTestDataInput.userLoggedInId;
            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/" + userIdHimSelf,
                Body = UnitTestDataInput.userForUpdate_Fail_MoldelState
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);


            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            

        }
        #endregion
        #region ChangeUserPasswordTests
        [Fact]
        public async Task ChangeUserPassword_Cant_ChangeUserPasswordAnotherUser()
        {
            //Arrange
            string UserIdAnotherUser = UnitTestDataInput.userUnLoggedInId;
            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/ChangeUserPassword/" + UserIdAnotherUser,
                Body = UnitTestDataInput.passwordForChange_CorrectOldPassword
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
            string UserIdHimSelf = UnitTestDataInput.userLoggedInId;
            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/ChangeUserPassword/" + UserIdHimSelf,
                Body = UnitTestDataInput.passwordForChange_CorrectOldPassword
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
            string userIdHimSelf = UnitTestDataInput.userLoggedInId;
            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/ChangeUserPassword/" + userIdHimSelf,
                Body = UnitTestDataInput.passwordForChange_Fail_ModelState
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);

           

            //Act

            var response = await _client.PutAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            

            //Assert
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            

        }
        [Fact]
        public async Task ChangeUserPassword_Cant_WrongOldPassword()
        {
            //Arrange
            string UserIdHimSelf = UnitTestDataInput.userLoggedInId;
            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/user/ChangeUserPassword/" + UserIdHimSelf,
                Body = UnitTestDataInput.passwordForChange_WrongOldPassword
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

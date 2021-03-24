using FluentAssertions;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Presentation;
using MadPay724.Test.DataInput;
using MadPay724.Test.IntegrationTests.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MadPay724.Test.IntegrationTests.Controllers
{
    public class AuthControllerTests : IClassFixture<TestClientProvider<Startup>>
    {
        private readonly HttpClient _client;
        public AuthControllerTests(TestClientProvider<Startup> testClientProvider)
        {
            _client = testClientProvider.Client;
            
        }
        #region LoginTests
        [Fact]
        public async Task Login_User_CanLogin()
        {
            //Arrange
            var request = new
            {
                Url = "/site/admin/Auth/login",
                Body = UnitTestDataInput.userForLogin_CanLogin
            };
            //Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
        [Fact]
        public async Task Login_User_CantLogin()
        {
            //Arrange
            var request = new
            {
                Url = "/site/admin/Auth/login",
                Body = UnitTestDataInput.userForLogin_Cant_Login
            };
            //Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }
        [Theory]
        [InlineData("", "")]
        [InlineData("aaaa.com", "123456")]
        public async Task Login_ModelStateError(string userName, string password)
        {
            //Arrange
            var request = new
            {
                Url = "/site/admin/Auth/login",
                Body = new UserForLoginDto
                {
                    UserName = userName,
                    Password = password
                }
            };

            //Act

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
        #region RegisterTests
        [Fact]
        public async Task Register_Cant_UserExist()
        {
            //Arrange
            var request = new
            {
                Url = "/site/admin/Auth/register",
                Body = UnitTestDataInput.userForRegister_CantRegister_UserExist
            };

            //Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
        [Fact]
        public async Task Register_Can()
        {
            //Arrange
            var request = new
            {
                Url = "/site/admin/Auth/register",
                Body = UnitTestDataInput.userForRegister_CanRegister
            };

            //Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact]
        public async Task Register_ModelStateError()
        {
            //Arrange
            var request = new
            {
                Url = "/site/admin/Auth/register",
                Body = new UserForRegisterDto
                {
                    Name = "",
                    Password = "",
                    PhoneNumber = "",
                    UserName = "",
                }
            };

            
            //Act

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

           

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            

        }
        #endregion
    }
}

using FluentAssertions;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Presentation;
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
    public class AuthControllerTests: IClassFixture<TestClientProvider<Startup>>
    {
        private readonly HttpClient _client;
        public AuthControllerTests(TestClientProvider<Startup> testClientProvider)
        {
            _client = testClientProvider.Client;
            //"userName":"alireza@gmail.com"
            //"password":"123456789"
            // Id :"5a3a2a02-7bbf-41f1-b401-25e7be899d24"
        }
        #region LoginTests
        [Fact]
        public async Task Login_User_CanLogin()
        {
            //Arrange
            var request = new
            {
                Url = "/site/admin/Auth/login",
                Body= new UserForLoginDto
                {
                    Password = "123456789",
                    UserName = "alireza@gmail.com"
                }
            };
            //Act
            var response = await _client.PostAsync(request.Url,ContentHelper.GetStringContent(request.Body));
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
                Body = new UserForLoginDto
                {
                    Password = "123456321789",
                    UserName = "alireza@gmail.com"
                }
            };
            //Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }
        [Theory]
        [InlineData("","")]
        [InlineData("aaaa.com", "123456")]
        public async Task Login_ModelStateError(string userName , string password)
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

            var controller = new ModelStateControllerTests();

            //Act

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            controller.ValidateModelState(request.Body);
            var modelState = controller.ModelState;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal(2, modelState.Keys.Count());
            Assert.False(modelState.IsValid);
            Assert.True(modelState.ContainsKey("Password") && modelState.ContainsKey("UserName"));

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
                Body = new UserForRegisterDto
                {
                    Name = "alireza",
                    Password = "12345678",
                    PhoneNumber = "11111111",
                    UserName = "alireza@gmail.com"
                }
            };

            //Act
            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
        [Fact]
        public async Task Register_Can_()
        {
            //Arrange
            var request = new
            {
                Url = "/site/admin/Auth/register",
                Body = new UserForRegisterDto
                {
                    Name = "test",
                    Password = "12345678",
                    PhoneNumber = "11111111",
                    UserName = "test@test.com"
                }
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

            var controller = new ModelStateControllerTests();

            //Act

            var response = await _client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            controller.ValidateModelState(request.Body);
            var modelState = controller.ModelState;

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            Assert.Equal(4, modelState.Keys.Count());
            Assert.False(modelState.IsValid);
            Assert.True(modelState.ContainsKey("Password") && modelState.ContainsKey("UserName") && modelState.ContainsKey("Password") && modelState.ContainsKey("UserName"));

        }
        #endregion
    }
}

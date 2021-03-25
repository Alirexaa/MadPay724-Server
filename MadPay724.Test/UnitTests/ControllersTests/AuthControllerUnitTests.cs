using MadPay724.Data.DatabaseContext;
using MadPay724.Presentation.Controllers.Site.Admin.V1;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.Auth.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using System.Threading.Tasks;
using Xunit;
using MadPay724.Data.Models;
using MadPay724.Test.DataInput;
using Microsoft.AspNetCore.Mvc;
using MadPay724.Data.Dto.Site.Admin.User;
using AutoMapper;
using MadPay724.Test.IntegrationTests.Providers;
using Microsoft.AspNetCore.Http;
using MadPay724.Common.ErrorAndMessge;

namespace MadPay724.Test.UnitTests.ControllersTests
{

    public class AuthControllerUnitTests
    {
        private readonly Mock<IUnitOfWork<MadpayDbContext>> _moqRepo;
        private readonly Mock<IAuthService> _moqAuthService;
        private readonly Mock<IConfiguration> _moqConfig;
        private readonly Mock<IConfigurationSection> _moqConfigSection;
        private readonly Mock<ILogger<AuthController>> _moqLogger;
        private readonly AuthController _controller;
        private readonly Mock<IMapper> _moqMapper;

        public AuthControllerUnitTests()
        {
            _moqRepo = new Mock<IUnitOfWork<MadpayDbContext>>();
            _moqAuthService = new Mock<IAuthService>();
            _moqConfig = new Mock<IConfiguration>();
            _moqConfigSection = new Mock<IConfigurationSection>();
            _moqLogger = new Mock<ILogger<AuthController>>();
            _moqMapper = new Mock<IMapper>();
            _controller = new AuthController(_moqRepo.Object, _moqAuthService.Object, _moqConfig.Object, _moqLogger.Object, _moqMapper.Object);

        }

        #region LoginTets

        [Fact]
        public async Task Login_Success()
        {
            //Arrange
            _moqAuthService.Setup(o => o.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(UnitTestDataInput.users.First());

            _moqConfigSection.Setup(o => o.Value).Returns("1234567890abcdefghijklmnopqrstwxrz");
            _moqConfig.Setup(o => o.GetSection(It.IsAny<string>())).Returns(_moqConfigSection.Object);
            //Act
            var result = await _controller.Login(UnitTestDataInput.userForLogin_CanLogin);
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task Login_Fail()
        {
            //Arrange
            _moqAuthService.Setup(o => o.Login(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());

            //Act
            var result = await _controller.Login(UnitTestDataInput.userForLogin_Cant_Login);
            var UnAthorizedResult = result as UnauthorizedObjectResult;

            //Assert
            Assert.NotNull(UnAthorizedResult);
            Assert.Equal(401, UnAthorizedResult.StatusCode);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("aaaa.com", "123456")]
        public void Login_Fail_ModelStateError(string userName, string password)
        {
            //Arrange
            var controller = new ModelStateControllerTests();
            //Act 
            var model = new UserForLoginDto
            {
                UserName = userName,
                Password = password
            };
            controller.ValidateModelState(model);
            var modelState = controller.ModelState;

            //Assert
            Assert.Equal(2, modelState.Keys.Count());
            Assert.False(modelState.IsValid);
            Assert.True(modelState.ContainsKey("Password") && modelState.ContainsKey("UserName"));
        }

        #endregion

        #region Register

        [Fact]
        public async Task Register_Success()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.UserExist(It.IsAny<string>())).ReturnsAsync(false);
            _moqAuthService.Setup(o => o.Register(It.IsAny<User>(), It.IsAny<Photo>(), It.IsAny<string>())).ReturnsAsync(UnitTestDataInput.users.First());
            _moqMapper.Setup(o => o.Map<UserDetailDto>(It.IsAny<User>())).Returns(UnitTestDataInput.userDetailDto);
            //Act

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "test";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var result = await _controller.Register(UnitTestDataInput.userForRegister_CanRegister);
            var createAtRouteResult = result as CreatedAtRouteResult;

            //Assert
            Assert.NotNull(createAtRouteResult);
            Assert.Equal(201, createAtRouteResult.StatusCode);
            Assert.IsType<UserDetailDto>(createAtRouteResult.Value);
        }
        [Fact]
        public async Task Register_Fail()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.UserExist(It.IsAny<string>())).ReturnsAsync(true);

            //Act
            var result = await _controller.Register(UnitTestDataInput.userForRegister_CanRegister);
            var conflictResult = result as ConflictObjectResult;

            //Assert
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task Register_Fail_DbError()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.UserExist(It.IsAny<string>())).ReturnsAsync(false);
            _moqAuthService.Setup(o => o.Register(It.IsAny<User>(), It.IsAny<Photo>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
 
            //Act

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "test";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var result = await _controller.Register(UnitTestDataInput.userForRegister_CanRegister);
            var createAtRouteResult = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(createAtRouteResult);
            Assert.Equal(400, createAtRouteResult.StatusCode);
            Assert.IsType<ReturnMessage>(createAtRouteResult.Value);
        }

        [Fact]
        public void Register_Fail_ModelStateError()
        {
            //Arrange
            var controller = new ModelStateControllerTests();
            var model = new UserForRegisterDto
            {
                Name = "",
                Password = "",
                PhoneNumber = "",
                UserName = "",
            };
            //Act
            controller.ValidateModelState(model);
            var modelState = controller.ModelState;

            //Assert 
            Assert.Equal(4, modelState.Keys.Count());
            Assert.False(modelState.IsValid);
            Assert.True(modelState.ContainsKey("Password") && modelState.ContainsKey("UserName") && modelState.ContainsKey("Password") && modelState.ContainsKey("UserName"));
        }

        #endregion

    }
}

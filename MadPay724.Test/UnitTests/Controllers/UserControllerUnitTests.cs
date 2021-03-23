using AutoMapper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Presentation.Controllers.Site.Admin;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.UserServices.Interface;
using MadPay724.Test.UnitTests.Mock.Data;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using MadPay724.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System;

namespace MadPay724.Test.UnitTests.Controllers
{
    public class UserControllerUnitTests
    {
        private readonly Mock<IUnitOfWork<MadpayDbContext>> _moqRepo;
        private readonly Mock<IMapper> _moqMapper;
        private readonly Mock<IUserService> _moqUserService;
        private readonly Mock<ILogger<UserController>> _moqLogger;
        private readonly UserController _controller;
        public UserControllerUnitTests()
        {
            _moqRepo = new Mock<IUnitOfWork<MadpayDbContext>>();
            _moqMapper = new Mock<IMapper>();
            _moqUserService = new Mock<IUserService>();
            _moqLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_moqRepo.Object, _moqMapper.Object, _moqUserService.Object, _moqLogger.Object);
            //"userName":"alireza@gmail.com"
            //"password":"123456789"
            // Id :"5a3a2a02-7bbf-41f1-b401-25e7be899d24"
        }
        #region GetUserTests
        [Fact]
        public async Task GetUser_Can_GetHimSelfUser()
        {
            //Arrange
            var user = UserControllerData.GetUser();
            var userDetailDto = UserControllerData.GetUserDetailDto();
            _moqRepo.Setup(o => o.UserRepository.GetManyAsync(It.IsAny<Expression<Func<User,bool>>>(), It.IsAny<Func<IQueryable<User>,IOrderedQueryable<User>>>(), It.IsAny<string>())).ReturnsAsync(user);
            _moqMapper.Setup(o => o.Map<UserDetailDto>(It.IsAny<User>())).Returns(userDetailDto);

            //Act
            var result = await _controller.GetUser(It.IsAny<string>());
            var okResult = result as OkObjectResult;
            //Assert
            Assert.NotNull(okResult);
            Assert.IsType<UserDetailDto>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task GetUser_Cant_GetAnotherUser()
        {
            //Arrange

            //Act

            //Assert
        }
        #endregion
        #region UpdateUserTests
        [Fact]
        public async Task UpdateUser_Cant_UpdateAnotherUser()
        {
            //Arrange

            //Act


            //Assert
            //var value = response.Content.ReadAsStringAsync();
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task UpdateUser_Can_UpdateHimSelfUser()
        {
            //Arrange

            //Act


            //Assert
            //var value = response.Content.ReadAsStringAsync();
            //response.EnsureSuccessStatusCode();
            //response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task UpdateUser_ModelStateError()
        {
            //Arrange

            //Arrange

            //Act



            //Assert
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            //response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //Assert.Equal(4, modelState.Keys.Count());
            //Assert.False(modelState.IsValid);
            //Assert.True(modelState.ContainsKey("Name") && modelState.ContainsKey("Name") &&
            //    modelState.ContainsKey("Name") && modelState.ContainsKey("Name"));

        }
        #endregion
        #region ChangeUserPasswordTests
        [Fact]
        public async Task ChangeUserPassword_Cant_ChangeUserPasswordAnotherUser()
        {
            //Arrange



            //Assert
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task ChangeUserPassword_Can_ChangeUserPasswordHimSelfUser()
        {
            //Arrange

            //Act



            //Assert
            //response.EnsureSuccessStatusCode();
            //response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task ChangeUserPassword_ModelStateError()
        {
            //Arrange


            //Act



            //Assert
            //response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            //response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //Assert.Equal(2, modelState.Keys.Count());
            //Assert.False(modelState.IsValid);
            //Assert.True(modelState.ContainsKey("NewPassword") && modelState.ContainsKey("OldPassword"));

        }
        [Fact]
        public async Task ChangeUserPassword_Cant_WrongOldPassword()
        {
            //Arrange

            //Act



            //Assert
            //response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //Assert.False(valueObj.Status);
            //Assert.Equal(Resource.ErrorMessages.WrongPassword, valueObj.Message);
        }

        #endregion
    }
}

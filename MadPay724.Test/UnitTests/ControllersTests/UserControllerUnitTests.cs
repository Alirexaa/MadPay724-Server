using AutoMapper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.User;
using MadPay724.Presentation.Controllers.Site.Admin.V1;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.UserServices.Interface;
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
using MadPay724.Test.DataInput;
using MadPay724.Test.IntegrationTests.Providers;

namespace MadPay724.Test.UnitTests.ControllersTests
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
        public async Task GetUser_Succsess()
        {
            //Arrange
            var users = UnitTestDataInput.users;
            var userDetailDto = UnitTestDataInput.userDetailDto;
            _moqRepo.Setup(o => o.UserRepository.GetManyAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(), It.IsAny<string>())).ReturnsAsync(users);
            _moqMapper.Setup(o => o.Map<UserDetailDto>(It.IsAny<User>())).Returns(userDetailDto);

            //Act
            var result = await _controller.GetUser(It.IsAny<string>());
            var okResult = result as OkObjectResult;
            //Assert
            Assert.NotNull(okResult);
            Assert.IsType<UserDetailDto>(okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }
        #endregion

        #region UpdateUserTests
        [Fact]
        public async Task UpdateUser_Successs()
        {
            //Arrange
            var users = UnitTestDataInput.users;
            //var userDetailDto = UserControllerMockData.GetUserDetailDto();
            _moqRepo.Setup(o => o.UserRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(users.First());
            _moqRepo.Setup(o => o.UserRepository.Update(It.IsAny<User>()));
            _moqMapper.Setup(o => o.Map(It.IsAny<UserForUpdateDto>(), It.IsAny<User>())).Returns(users.First());
            _moqRepo.Setup(o => o.SaveAsync()).ReturnsAsync(true);

            //Act
            var result = await _controller.UpdateUser(It.IsAny<string>(), It.IsAny<UserForUpdateDto>());
            var okResult = result as OkResult;
            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            _moqRepo.Verify(o => o.UserRepository.Update(It.IsAny<User>()), Times.Once());

        }

        [Fact]
        public async Task UpdateUser_Fail()
        {
            //Arrange
            var users = UnitTestDataInput.users;
            //var userDetailDto = UserControllerMockData.GetUserDetailDto();
            _moqRepo.Setup(o => o.UserRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(users.First());
            _moqRepo.Setup(o => o.UserRepository.Update(It.IsAny<User>()));
            _moqMapper.Setup(o => o.Map(It.IsAny<UserForUpdateDto>(), It.IsAny<User>())).Returns(users.First());
            _moqRepo.Setup(o => o.SaveAsync()).ReturnsAsync(false);

            //Act
            var result = await _controller.UpdateUser(It.IsAny<string>(), It.IsAny<UserForUpdateDto>());
            var badResult = result as BadRequestResult;
            //Assert
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);

        }

        [Fact]
        public void UpdateUser_ModelStateError()
        {
            //Arrange
            var controller = new ModelStateControllerTests();

            //Act
            controller.ValidateModelState(UnitTestDataInput.userForUpdate_Fail_MoldelState);
            var modelState = controller.ModelState;


            //Assert
            Assert.Equal(4, modelState.Keys.Count());
            Assert.False(modelState.IsValid);
            Assert.True(modelState.ContainsKey("Name") && modelState.ContainsKey("Name") &&
                modelState.ContainsKey("Name") && modelState.ContainsKey("Name"));
        }
        #endregion
        #region ChangeUserPasswordTests
        [Fact]
        public async Task ChangeUserPassword_Success()
        {
            //Arrange
            _moqUserService.Setup(o => o.GetUserForChangingPassword(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(UnitTestDataInput.users.First());
            _moqUserService.Setup(o => o.UpdateUserPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            //Act
            var result = await _controller.ChangeUserPassword(It.IsAny<string>(), UnitTestDataInput.passwordForChange);
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public void ChangeUserPassword_ModelStateError()
        {
            //Arrange
            var controller = new ModelStateControllerTests();

            //Act
            controller.ValidateModelState(UnitTestDataInput.passwordForChange_Fail_ModelState);
            var modelState = controller.ModelState;


            //Assert
            Assert.Equal(2, modelState.Keys.Count());
            Assert.False(modelState.IsValid);
            Assert.True(modelState.ContainsKey("NewPassword") && modelState.ContainsKey("OldPassword"));

        }
        [Fact]
        public async Task ChangeUserPassword_Fail_WrongOldPassword()
        {
            //Arrange
            _moqUserService.Setup(o => o.GetUserForChangingPassword(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
            _moqUserService.Setup(o => o.UpdateUserPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
            //Act
            var result = await _controller.ChangeUserPassword(It.IsAny<string>(),UnitTestDataInput.passwordForChange );
            var badResult = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
        }

        #endregion
    }
}

using MadPay724.Common.Helper.Interface;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.UserServices.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MadPay724.Test.UnitTests.ServicesTests
{
    public class UserServiceUnitTests
    {
        private readonly Mock<IUnitOfWork<MadpayDbContext>> _moqRepo;
        private readonly Mock<IUtilities> _moqUtilities;
        private readonly UserService _service;
        public UserServiceUnitTests()
        {
            _moqRepo = new Mock<IUnitOfWork<MadpayDbContext>>();
            _moqUtilities = new Mock<IUtilities>();
            _service = new UserService(_moqRepo.Object, _moqUtilities.Object);
        }

        #region GetUserForChangingPassword

        [Fact]
        public async Task GetUserForChangingPassword_Success()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _moqUtilities.Setup(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);

            //Act
            var result = await _service.GetUserForChangingPassword(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
        }

        [Fact]
        public async Task GetUserForChangingPassword_Fail_NotUserExist()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());
  

            //Act
            var result = await _service.GetUserForChangingPassword(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.Null(result);

        }

        [Fact]
        public async Task GetUserForChangingPassword_Fail_WrongPassword()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new User());
            _moqUtilities.Setup(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            //Act
            var result = await _service.GetUserForChangingPassword(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.Null(result);
        }

        #endregion

        #region UpdateUserPassword
        [Fact]
        public async Task UpdateUserPassword_Success()
        {
            //Arrange
            byte[] passwordHash, passwordSalt;
            _moqUtilities.Setup(o => o.CreatePasswordHash(It.IsAny<string>(), out passwordHash, out passwordSalt));
            _moqRepo.Setup(o => o.UserRepository.Update(It.IsAny<User>()));
            _moqRepo.Setup(o => o.SaveAsync()).ReturnsAsync(true);
            //Act
            var result = await _service.UpdateUserPassword(new User(), It.IsAny<string>());
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task UpdateUserPassword_Fail()
        {
            //Arrange
            byte[] passwordHash, passwordSalt;
            _moqUtilities.Setup(o => o.CreatePasswordHash(It.IsAny<string>(), out passwordHash, out passwordSalt));
            _moqRepo.Setup(o => o.UserRepository.Update(It.IsAny<User>()));
            _moqRepo.Setup(o => o.SaveAsync()).ReturnsAsync(false);
            //Act
            var result = await _service.UpdateUserPassword(new User(), It.IsAny<string>());
            //Assert
            Assert.False(result);
        }

        #endregion
    }
}

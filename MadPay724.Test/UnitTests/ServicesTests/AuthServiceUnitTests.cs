using MadPay724.Common.Helper.Interface;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Site.Admin.Auth.Service;
using MadPay724.Test.DataInput;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MadPay724.Test.UnitTests.ServicesTests
{
    public class AuthServiceUnitTests
    {
        private readonly Mock<IUnitOfWork<MadpayDbContext>> _moqRepo;
        private readonly Mock<IUtilities> _moqUtilitis;
        private readonly AuthService _service;
        public AuthServiceUnitTests()
        {
            _moqRepo = new Mock<IUnitOfWork<MadpayDbContext>>();
            _moqUtilitis = new Mock<IUtilities>();
            _service = new AuthService(_moqRepo.Object, _moqUtilitis.Object);
        }

        [Fact]
        public async Task Login_Success()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.GetManyAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<string>())).ReturnsAsync(UnitTestDataInput.users);

            _moqUtilitis.Setup(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);

            //Act

            var result = await _service.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);


        }

        #region LoginTests
        [Fact]
        public async Task Login_Fail_NoExistUser()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.GetManyAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<string>())).ReturnsAsync(Enumerable.Empty<User>());

            //Act

            var result = await _service.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.Null(result);

        }
        [Fact]
        public async Task Login_Fail_WronPassword()
        {
            //Arrange
            _moqRepo.Setup(o => o.UserRepository.GetManyAsync(It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                It.IsAny<string>())).ReturnsAsync(UnitTestDataInput.users);

            _moqUtilitis.Setup(o => o.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(false);

            //Act

            var result = await _service.Login(It.IsAny<string>(), It.IsAny<string>());

            //Assert
            Assert.Null(result);
        }

        #endregion

        #region RegisterTests
        [Fact]
        public async Task Register_Success()
        {
            //Arrenge
            byte[] passwordHash, passwordSalt;
            _moqUtilitis.Setup(o => o.CreatePasswordHash(It.IsAny<string>(), out passwordHash, out passwordSalt));
            _moqRepo.Setup(o => o.UserRepository.InsertAsync(It.IsAny<User>()));
            _moqRepo.Setup(o => o.PhotoRepository.InsertAsync(It.IsAny<Photo>()));
            _moqRepo.Setup(o => o.SaveAsync()).ReturnsAsync(true);
            //Act

            var result = await _service.Register(new User(), It.IsAny<Photo>(), It.IsAny<string>());

            //Assert 
            Assert.NotNull(result);
            Assert.IsType<User>(result);

        }
        [Fact]
        public async Task Register_Fail_DbError()
        {
            //Arrenge
            byte[] passwordHash, passwordSalt;
            _moqUtilitis.Setup(o => o.CreatePasswordHash(It.IsAny<string>(), out passwordHash, out passwordSalt));
            _moqRepo.Setup(o => o.UserRepository.InsertAsync(It.IsAny<User>()));
            _moqRepo.Setup(o => o.PhotoRepository.InsertAsync(It.IsAny<Photo>()));
            _moqRepo.Setup(o => o.SaveAsync()).ReturnsAsync(false);
            //Act

            var result = await _service.Register(new User(), It.IsAny<Photo>(), It.IsAny<string>());

            //Assert 
            Assert.Null(result);

        }

        #endregion
    }
}

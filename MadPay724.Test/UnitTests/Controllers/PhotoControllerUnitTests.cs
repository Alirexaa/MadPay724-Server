using AutoMapper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Upload.Interface;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Xunit;
using System.Threading.Tasks;
using MadPay724.Data.Models;
using MadPay724.Data.Dto.Site.Admin.Photo;
using MadPay724.Presentation.Controllers.Site.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MadPay724.Test.DataInput;
using MadPay724.Data.Dto.Services;
using System.Linq;
using System.Linq.Expressions;

namespace MadPay724.Test.UnitTests.Controllers
{
    public class PhotoControllerUnitTests
    {
        private readonly Mock<IUnitOfWork<MadpayDbContext>> _moqRepo;
        private readonly Mock<IMapper> _moqMapper;
        private readonly Mock<IUploadService> _moqUploadService;
        private readonly Mock<IWebHostEnvironment> _moqEnv;
        private readonly PhotoController _controller;
        private readonly Mock<IFormFile> _file;
        public PhotoControllerUnitTests()
        {
            _moqRepo = new Mock<IUnitOfWork<MadpayDbContext>>();
            _moqMapper = new Mock<IMapper>();
            _moqUploadService = new Mock<IUploadService>();
            _moqEnv = new Mock<IWebHostEnvironment>();
            _controller = new PhotoController(_moqRepo.Object, _moqMapper.Object, _moqUploadService.Object, _moqEnv.Object);
            _file = new Mock<IFormFile>();
        }

        [Fact]
        public async Task GetPhoto_Success()
        {
            //Arrange
            _moqRepo.Setup(o => o.PhotoRepository.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(UnitTestDataInput.Users.First().Photos.First()) ;
            _moqMapper.Setup(o => o.Map<PhotoForReturnUserProfileDto>(It.IsAny<Photo>())).Returns(UnitTestDataInput.photoForReturnUserProfile);

            //Act
            var result = await _controller.GetPhoto(It.IsAny<string>());
            var okResult = result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);


        }

        [Fact]
        public async Task ChangeUserPhoto_Success()
        {
            //Arrange
            var photoFromRepo = UnitTestDataInput.photoFromRepo_UploadedCloudinary;
            _moqUploadService.Setup(o => o.UploadProfileImage(It.IsAny<FormFile>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(UnitTestDataInput.fileUploaded_Success);

            _moqRepo.Setup(o => o.PhotoRepository.GetAsync(It.IsAny<Expression<Func<Photo,bool>>>())).ReturnsAsync(UnitTestDataInput.photoFromRepo_UploadedCloudinary);

            _moqEnv.Setup(o => o.WebRootPath).Returns(It.IsAny<string>());

            _moqUploadService.Setup(o => o.DeleteFileFromCloudinary(UnitTestDataInput.photoFromRepo_UploadedCloudinary.PublicId)).ReturnsAsync(It.IsAny<FileDeletedDto>());

            _moqUploadService.Setup(o => o.DeleteFileFromLocal(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            _moqMapper.Setup(o => o.Map(It.IsAny<PhotoFromUserProfileDto>(), It.IsAny<Photo>()));
            _moqRepo.Setup(o => o.PhotoRepository.Update(It.IsAny<Photo>()));
            _moqRepo.Setup(o => o.SaveAsync()).ReturnsAsync(true);

            _moqMapper.Setup(o => o.Map<PhotoForReturnUserProfileDto>(It.IsAny<Photo>())).Returns(UnitTestDataInput.photoForReturnUserProfile);

            //Act

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var result = await _controller.ChangeUserPhoto(It.IsAny<string>(), new PhotoFromUserProfileDto
            {
                File = It.IsAny<FormFile>()
            });
            var createdAtRouteResult = result as CreatedAtRouteResult;

            //Assert 
            Assert.NotNull(createdAtRouteResult);
            Assert.Equal(201, createdAtRouteResult.StatusCode);
            Assert.IsType<PhotoForReturnUserProfileDto>(createdAtRouteResult.Value);
        }
        [Fact]
        public async Task ChangeUserPhoto_Fail()
        {
            //Arrange
            var photoFromRepo = UnitTestDataInput.photoFromRepo_UploadedCloudinary;
            _moqUploadService.Setup(o => o.UploadProfileImage(It.IsAny<FormFile>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(UnitTestDataInput.fileUploaded_Fail);

            _moqEnv.Setup(o => o.WebRootPath).Returns(It.IsAny<string>());

            //Act

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "";
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            var result = await _controller.ChangeUserPhoto(It.IsAny<string>(), new PhotoFromUserProfileDto
            {
                File = It.IsAny<FormFile>()
            });
            var badRequestObjectResult = result as BadRequestObjectResult;

            //Assert 
            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);
           
        }
    }
}

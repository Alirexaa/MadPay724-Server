using CloudinaryDotNet;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Services;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Upload.Service;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MadPay724.Test.UnitTests.ServicesTests
{
    public class UploadServiceUnitTests
    {
        private readonly Mock<IUnitOfWork<MadpayDbContext>> _moqRepo;
        private readonly Setting _setting;
        private readonly UploadService _service;
        private readonly Mock<IFormFile> _moqFile;
        public UploadServiceUnitTests()
        {
            _moqRepo = new Mock<IUnitOfWork<MadpayDbContext>>();
            _setting = new Setting()
            {
                CloudinaryAPIKey = "UnitTest",
                CloudinaryAPISecret = "UnitTest",
                CloudinaryCloudName = "UnitTest"
            };
            _moqRepo.Setup(x => x.SettingRepository.GetById(It.IsAny<short>()))
              .Returns(_setting);
            _service = new UploadService(_moqRepo.Object);
            _moqFile = new Mock<IFormFile>();
            
            _moqRepo.Setup(x => x.SettingRepository.GetById(It.IsAny<short>()))
              .Returns(_setting);


        }

        [Fact]
        public async Task UploadProfileImageToLocal_Success()
        {
            //Arrange

            var content = "UnitTest";
            var fileName = "UnitTest.jpg";

            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);

            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            _moqFile.Setup(o => o.OpenReadStream()).Returns(ms);
            _moqFile.Setup(o => o.FileName).Returns(fileName);
            _moqFile.Setup(o => o.Length).Returns(ms.Length);
            _moqFile.Setup(o => o.CopyToAsync(It.IsAny<Stream>(), CancellationToken.None));


            //Act
            var result = await _service.UploadProfileImageToLocal(_moqFile.Object, "1", "C:\\Users\\Alierza\\source\\repos\\MadPay724\\MadPay724.Presentation\\wwwroot", "http://");


            //Assert
            Assert.NotNull(result);
            Assert.IsType<FileUploadedDto>(result);
            Assert.True(result.Status);
        }

        [Fact]
        public async Task UploadProfilePicToLocal_Fail_WrongFile()
        {
            //Arrange------------------------------------------------------------------------------------------------------------------------------


            _moqFile.Setup(x => x.Length).Returns(0);

            //Act----------------------------------------------------------------------------------------------------------------------------------
            var result = await _service.UploadProfileImageToLocal(_moqFile.Object,
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert-------------------------------------------------------------------------------------------------------------------------------
            Assert.NotNull(result);
            Assert.IsType<FileUploadedDto>(result);
            Assert.False(result.Status);

        }
    }
}

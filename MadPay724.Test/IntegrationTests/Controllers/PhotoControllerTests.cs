using FluentAssertions;
using MadPay724.Data.Dto.Site.Admin.Photo;
using MadPay724.Presentation;
using MadPay724.Test.DataInput;
using MadPay724.Test.IntegrationTests.Providers;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MadPay724.Test.IntegrationTests.Controllers
{

    public class PhotoControllerTests : IClassFixture<TestClientProvider<Startup>>
    {
        private readonly HttpClient _client;
        private readonly string AToken;
        public PhotoControllerTests(TestClientProvider<Startup> testClientProvider)
        {
            _client = testClientProvider.Client;
            AToken = UnitTestDataInput.aToken;
        }
        #region GetPhotoTests
        [Fact]
        public async Task GetPhoto_Can_GetUserHimSelf()
        {
            //Arrange
            string userIdHimSelf = UnitTestDataInput.userLoggedInId;
            string photoId = UnitTestDataInput.userLoggedInPhotoId;
            var request = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/Users/{userIdHimSelf}/photos/{photoId}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act
            var response = await _client.GetAsync(request);
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task GetPhoto_Cant_GetAnotherUser()
        {
            //Arrange
            string userIdAnotherUser = UnitTestDataInput.userUnLoggedInId;
            string photoId = UnitTestDataInput.userLoggedInPhotoId;
            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/Users/{userIdAnotherUser}/photos/{photoId}"
            };
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            //Act
            var response = await _client.GetAsync(request.Url);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        #endregion
        #region ChangeUserPhotoTests
        [Fact]
        public async Task ChangeUserPhoto_Can_ChangeUserProfilePhotoHimSelf()
        {
            //Arrange
            string userIdHimSelf = UnitTestDataInput.userLoggedInId;
            var fileMock = new Mock<IFormFile>();
            var fileName = "5a3a2a02-7bbf-41f1-b401-25e7be899d24.jpg";



            Bitmap image = new Bitmap(50, 50);
            Graphics imageData = Graphics.FromImage(image);
            imageData.DrawLine(new Pen(Color.Red), 0, 0, 50, 50);
            MemoryStream memoryStream = new MemoryStream();
            byte[] bitmapData;
            using (memoryStream)
            {
                image.Save(memoryStream, ImageFormat.Bmp);
                bitmapData = memoryStream.ToArray();

            }



            var ms = new MemoryStream(bitmapData);
            var writer = new StreamWriter(ms);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(o => o.OpenReadStream()).Returns(ms);
            fileMock.Setup(o => o.FileName).Returns(fileName);
            fileMock.Setup(o => o.Length).Returns(ms.Length);


            byte[] data;
            using (var br = new BinaryReader(fileMock.Object.OpenReadStream()))
                data = br.ReadBytes((int)fileMock.Object.OpenReadStream().Length);

            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multipartFormData = new MultipartFormDataContent();
            multipartFormData.Add(bytes, "File", fileMock.Object.FileName);

            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/Users/{userIdHimSelf}/photos",
                Body = new PhotoFromUserProfileDto
                {
                    PublicId = "1",
                    Url = "https://google.com"
                }
            };
            multipartFormData.Add(ContentHelper.GetStringContent(request.Body));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            var response = await _client.PostAsync(request.Url, multipartFormData);
            //Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        [Fact]
        public async Task ChangeUserPhoto_Cant_ChangeUserProfilePhotoAnotherUser()
        {
            //Arrange
            string userIdAnotherUser = UnitTestDataInput.userUnLoggedInId;
            string photoId = UnitTestDataInput.userLoggedInPhotoId;
            var request = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/Users/{userIdAnotherUser}/photos/{photoId}";
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            var response = await _client.GetAsync(request);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task ChangeUserPhoto_Cant_ChangeUserProfilePhoto_WrongFile()
        {
            //Arrange
            string userIdHimSelf = UnitTestDataInput.userLoggedInId;
            var fileMock = new Mock<IFormFile>();
            var fileName = "5a3a2a02-7bbf-41f1-b401-25e7be899d24.jpg";


            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Flush();
            ms.Position = 0;

            fileMock.Setup(o => o.OpenReadStream()).Returns(ms);
            fileMock.Setup(o => o.FileName).Returns(fileName);
            fileMock.Setup(o => o.Length).Returns(ms.Length);


            byte[] data;
            using (var br = new BinaryReader(fileMock.Object.OpenReadStream()))
                data = br.ReadBytes((int)fileMock.Object.OpenReadStream().Length);

            ByteArrayContent bytes = new ByteArrayContent(data);
            MultipartFormDataContent multipartFormData = new MultipartFormDataContent();
            multipartFormData.Add(bytes, "File", fileMock.Object.FileName);

            var request = new
            {
                Url = $"/site/user/{UnitTestDataInput.SiteAdminVersion}/Users/{userIdHimSelf}/photos",
                Body = new PhotoFromUserProfileDto
                {
                    PublicId = "1",
                    Url = "https://google.com"
                }
            };
            multipartFormData.Add(ContentHelper.GetStringContent(request.Body));
            //Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AToken);
            var response = await _client.PostAsync(request.Url, multipartFormData);
            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion
    }
}

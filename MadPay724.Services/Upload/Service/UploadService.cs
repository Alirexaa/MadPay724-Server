using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Services;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Upload.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Services.Upload.Service
{
    class UploadService : IUploadService
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly Cloudinary _cloudinary;
        private readonly Setting _setting;

        public UploadService(IUnitOfWork<MadpayDbContext> db)
        {
            _db = db;
            _setting = _db.SettingRepository.GetById(1);
            Account acc = new Account(
                _setting.CloudinaryCloudName,
                _setting.CloudinaryAPIKey,
                _setting.CloudinaryAPISecret);
            _cloudinary = new Cloudinary(acc);
        }


        public Task<FileUploadedDto> UploadFile(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<FileUploadedDto> UploadToLoacl(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public async Task<FileUploadedDto> UploadToCloudinary(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {

                try
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.Name, stream),
                            Transformation = new Transformation().Width(250).Height(250).Crop("file").Gravity("face")
                        };
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        if (uploadResult.StatusCode==HttpStatusCode.OK)
                        {
                            return new FileUploadedDto() 
                            {
                                Status =true,
                                Message =Resource.InformationMessages.FileUploadSuccess
                            };
                        }
                        else
                        {
                            return new FileUploadedDto()
                            {
                                Status = false,
                                Message = uploadResult.Error.Message
                            };
                        }

                    }
                }
                catch (Exception ex)
                {

                    return new FileUploadedDto()
                    {
                        Status = false,
                        Message = ex.Message
                    };
                }


            }
            else
            {
                return new FileUploadedDto()
                {
                    Status = false,
                    Message = Resource.ErrorMessages.NotExistFile
                };
            }
        }
    }
}

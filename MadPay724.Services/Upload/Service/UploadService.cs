﻿using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MadPay724.Common.ErrorAndMessge;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Services;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Upload.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Services.Upload.Service
{
    public class UploadService : IUploadService
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly Cloudinary _cloudinary;
        private readonly Setting _setting;

        public UploadService(IUnitOfWork<MadpayDbContext> db)
        {
            _db = db;
            _setting = _db.SettingRepository.GetById((short)1);
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

        public async Task<FileUploadedDto> UploadToLocal(IFormFile file,string userId, string webRootPath, string baseUrl)
        {
            if (file.Length > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string fileExtention = Path.GetExtension(fileName);
                    string newFileName = string.Format($"{userId}{fileExtention}");
                    string storagePath = Path.Combine(webRootPath, "Files/Images/Profiles");
                    string fileFullPath = Path.Combine(storagePath, newFileName);


                    using (var stream = new FileStream(fileFullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return new FileUploadedDto()
                    {
                        Status = true,
                        Message = Resource.InformationMessages.FileUploadSuccess,
                        PublicId = "0",
                        Url = string.Format($"{baseUrl}/wwwroot/Files/Images/Profiles/{newFileName}"),

                    };
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
                            Transformation = new Transformation().Width(250).Height(250).Crop("fill").Gravity("face")
                        };
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        if (uploadResult.StatusCode == HttpStatusCode.OK)
                        {
                            return new FileUploadedDto()
                            {
                                Status = true,
                                Message = Resource.InformationMessages.FileUploadSuccess,
                                Url = uploadResult.Url.ToString(),
                                PublicId = uploadResult.PublicId

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

        public async Task<FileDeletedDto> DeleteFileFromCloudinary(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                var deleteResult = await _cloudinary.DestroyAsync(deleteParams);
                if (deleteResult.StatusCode == HttpStatusCode.OK)
                {
                    return new FileDeletedDto()
                    {
                        Status = true,
                        Message = Resource.InformationMessages.FileDeletedSuccess
                    };
                }
                else
                {
                    return new FileDeletedDto()
                    {
                        Status = false,
                        Message = deleteResult.Error.Message
                    };
                }
            }
            catch (Exception ex)
            {

                return new FileDeletedDto()
                {
                    Status = false,
                    Message = ex.Message
                };
            }

        }
    }
}

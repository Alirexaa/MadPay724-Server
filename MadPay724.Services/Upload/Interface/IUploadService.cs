using MadPay724.Data.Dto.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadPay724.Services.Upload.Interface
{
    public interface IUploadService
    {
        Task<FileUploadedDto> UploadProfileImageToCloudinary(IFormFile file, string userId);
        Task<FileUploadedDto> UploadProfileImageToLocal(IFormFile file,string userId, string webRootPath, string baseUrl);
        Task<FileUploadedDto> UploadProfileImage(IFormFile file, string userId, string webRootPath, string baseUrl);
        Task<FileDeletedDto> DeleteFileFromCloudinary(string publicId);
    }
}

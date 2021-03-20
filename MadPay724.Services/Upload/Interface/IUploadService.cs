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
        Task<FileUploadedDto> UploadToCloudinary(IFormFile file);
        Task<FileUploadedDto> UploadToLoacl(IFormFile file);
        Task<FileUploadedDto> UploadFile(IFormFile file);
    }
}

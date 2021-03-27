using AutoMapper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.Photo;
using MadPay724.Presentation.Helper.Filters;
using MadPay724.Repo.Infrastructure;
using MadPay724.Services.Upload.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MadPay724.Presentation.Controllers.Site.Admin.V1
{
    [ApiExplorerSettings(GroupName = "SiteApiV1")]
    [Route("site/admin/v1/users/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IWebHostEnvironment _env;

        public PhotoController(IUnitOfWork<MadpayDbContext> dbContext, IMapper mapper, IUploadService uploadService,
            IWebHostEnvironment env)
        {
            _db = dbContext;
            _mapper = mapper;
            _uploadService = uploadService;
            _env = env;
        }

        [HttpPost]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> ChangeUserPhoto(string userId, [FromForm] PhotoFromUserProfileDto photoFromUserProfileDto)
        {
            

            var file = photoFromUserProfileDto.File;
            //var uploadResult = await _uploadService.UploadProfileImageToCloudinary(file,userId);
            string baseUrl = string.Format($"{Request.Scheme ?? ""}://{Request.Host.Value ?? ""}{Request.PathBase.Value ?? ""}");

            //var uploadResult = await _uploadService.UploadProfileImageToLocal(file,userId,_env.WebRootPath,baseUrl);

            var uploadResult = await _uploadService.UploadProfileImage(file, userId, _env.WebRootPath, baseUrl);

            if (uploadResult.Status)
            {
                photoFromUserProfileDto.Url = uploadResult.Url;
                photoFromUserProfileDto.PublicId = uploadResult.IsUplodedToLocal ? "1" : uploadResult.PublicId;

                var photoFromRepository = await _db.PhotoRepository.GetAsync(p => p.UserId == userId && p.IsMain == true);

                //Delete image if it is in Cloudinary
                if (photoFromRepository?.PublicId != null && photoFromRepository?.PublicId != "0" && photoFromRepository?.PublicId != "1")
                {
                    var deletedResult = await _uploadService.DeleteFileFromCloudinary(photoFromRepository.PublicId);
                }
                //delete image if extention of oldImage and newImage is different
                //if extention of oldImage and newImage is same automatic oldImage Deleded
                if (photoFromRepository?.PublicId == "1" && uploadResult.IsUplodedToLocal && photoFromRepository.Url != photoFromUserProfileDto.Url)
                {

                    var deletedResult = _uploadService.DeleteFileFromLocal(photoFromRepository.Url.Split("/").Last(), _env.WebRootPath, "Files\\Images\\Profiles");
                }
                //delete oldimage if oldImage is in local and newImage in cloudinary  
                if (photoFromRepository?.PublicId == "1" && !uploadResult.IsUplodedToLocal)
                {
                    var deletedResult = _uploadService.DeleteFileFromLocal(photoFromRepository.Url.Split("/").Last(), _env.WebRootPath, "Files\\Images\\Profiles");
                }

                _mapper.Map(photoFromUserProfileDto, photoFromRepository);

                _db.PhotoRepository.Update(photoFromRepository);

                if (await _db.SaveAsync())
                {
                    var photoForReturn = _mapper.Map<PhotoForReturnUserProfileDto>(photoFromRepository);
                    return CreatedAtRoute("GetPhoto", routeValues: new { id = photoFromRepository.Id }, value: photoForReturn);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest(uploadResult.Message);
            }
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        [ServiceFilter(typeof(UserCheckIdFilter))]
        public async Task<IActionResult> GetPhoto(string id)
        {
            var photoFromRepository = await _db.PhotoRepository.GetByIdAsync(id);
            var photo = _mapper.Map<PhotoForReturnUserProfileDto>(photoFromRepository);

            return Ok(photo);
        }
    }
}

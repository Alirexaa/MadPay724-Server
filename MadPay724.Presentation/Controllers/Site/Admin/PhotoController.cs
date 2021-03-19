using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MadPay724.Common.Helper;
using MadPay724.Data.DatabaseContext;
using MadPay724.Data.Dto.Site.Admin.Photo;
using MadPay724.Data.Models;
using MadPay724.Repo.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MadPay724.Presentation.Controllers.Site.Admin
{
    [ApiExplorerSettings(GroupName = "SiteApi")]
    [Authorize]
    [Route("site/admin/users/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IUnitOfWork<MadpayDbContext> _db;
        private readonly IMapper _mapper;
        private readonly Cloudinary _clodinary;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

        public PhotoController(IUnitOfWork<MadpayDbContext> dbContext ,IMapper mapper,
             IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _db = dbContext;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            Account acc = new Account(_cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.APIKey,
                _cloudinaryConfig.Value.APISecret);

            _clodinary = new Cloudinary(acc);

        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPhoto(string userId,[FromForm] PhotoFromUserProfileDto photoFromUserProfileDto)
        {
            if(User.FindFirst(ClaimTypes.NameIdentifier).Value != userId)
            {
               return Unauthorized();
            }

            var file = photoFromUserProfileDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var strem=file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, strem),
                        Transformation = new Transformation().Width(250).Height(250).Crop("fill").Gravity("face")
                    };
                    uploadResult = await _clodinary.UploadAsync(uploadParams);
                }
            }

            photoFromUserProfileDto.Url = uploadResult.Url.ToString();
            photoFromUserProfileDto.PublicId = uploadResult.PublicId;

            var photoFromRepository = await _db.PhotoRepository.GetAsync(p => p.UserId == userId && p.IsMain == true);

            //Delete image if it exist in Cloudinary
            if(photoFromRepository.PublicId !=null && photoFromRepository.PublicId != "0")
            {
                var deleteParams = new DeletionParams(photoFromRepository.PublicId);
                var deleteResult = _clodinary.Destroy(deleteParams);
            }

             _mapper.Map(photoFromUserProfileDto, photoFromRepository);

             _db.PhotoRepository.Update(photoFromRepository);

            if(await _db.SaveAsync())
            {
                var photoForReturn = _mapper.Map<PhotoForReturnUserProfileDto>(photoFromRepository);
                return CreatedAtRoute("GetPhoto",routeValues: new {id= photoFromRepository.Id },value: photoForReturn);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}",Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(string id)
        {
            var photoFromRepository = await _db.PhotoRepository.GetByIdAsync(id);
            var photo = _mapper.Map<PhotoForReturnUserProfileDto>(photoFromRepository);

            return Ok(photo);
        }
    }
}

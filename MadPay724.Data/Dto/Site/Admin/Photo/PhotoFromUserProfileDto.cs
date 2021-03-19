using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Dto.Site.Admin.Photo
{
    public class PhotoFromUserProfileDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; } = "Profile Pic";
        public string Alt { get; set; } = "Profile Pic";
        public string PublicId { get; set; } 
        public bool IsMain { get; set; } = true;



    }
}

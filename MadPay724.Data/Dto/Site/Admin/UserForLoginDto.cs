﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MadPay724.Data.Dto.Site.Admin
{
    public class UserForLoginDto
    {
        [Required]
        [EmailAddress(ErrorMessageResourceName = "WrogEmailFormatMessage", ErrorMessageResourceType = typeof(Resource.ErrorMessages))]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        public bool IsRemmeber { get; set; }
    }
}
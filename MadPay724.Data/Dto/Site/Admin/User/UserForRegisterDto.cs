using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MadPay724.Data.Dto.Site.Admin.User
{
    public class UserForRegisterDto
    {
        [Required]
        [EmailAddress(ErrorMessageResourceName = "WrogEmailFormatMessage", ErrorMessageResourceType =typeof(Resource.ErrorMessages))]
        public string UserName { get; set; }

        [Required]
        [StringLength(maximumLength: 16, MinimumLength = 8, ErrorMessageResourceType = typeof( Resource.ErrorMessages),
            ErrorMessageResourceName = "PasswordLengthMessage")]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
    }
}

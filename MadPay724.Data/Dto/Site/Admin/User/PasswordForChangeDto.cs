using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MadPay724.Data.Dto.Site.Admin.User
{
    public class PasswordForChangeDto
    {
        [Required]
        [StringLength(maximumLength:16,MinimumLength =8)]
        public string OldPassword { get; set; }
        [Required]
        [StringLength(maximumLength: 16, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}

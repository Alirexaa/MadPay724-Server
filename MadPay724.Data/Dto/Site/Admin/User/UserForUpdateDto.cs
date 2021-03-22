using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MadPay724.Data.Dto.Site.Admin.User
{
    public class UserForUpdateDto
    {
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Address { get; set; }
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string PhoneNumber { get; set; }

        [Required]
        public bool Gender { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string City { get; set; }

    }
}

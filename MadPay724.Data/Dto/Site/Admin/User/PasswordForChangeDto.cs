using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Dto.Site.Admin.User
{
    public class PasswordForChangeDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

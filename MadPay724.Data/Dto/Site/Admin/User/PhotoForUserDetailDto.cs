﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Dto.Site.Admin.User
{
    public class PhotoForUserDetailDto
    {
        public string Url { get; set; }

        public string Description { get; set; }

        public string Alt { get; set; }

        public bool IsMain { get; set; }
    }
}

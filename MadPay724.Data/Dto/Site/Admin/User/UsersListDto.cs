﻿using MadPay724.Data.Dto.Common.ION;
using MadPay724.Data.Dto.Site.Admin.BankCard;
using MadPay724.Data.Dto.Site.Admin.Photo;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Dto.Site.Admin.User
{
    public class UsersListDto /*: BaseDto*/
    {

        //public Link UpdateUser { get; set; }
        //public Link ChangeUserPassword { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public ICollection<PhotoForUserDetailDto> Photos { get; set; }

        public ICollection<BankCardForUserDetailDto> BankCards { get; set; }

    }
}

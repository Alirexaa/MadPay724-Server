using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Dto.Common.ION
{
    public abstract class BaseDto : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}

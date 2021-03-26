using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Dto.Common.ION
{
    public class Collection<T>: BaseDto
    {
        public T[] Value { get; set; }
    }
}

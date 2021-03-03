using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Common.ErrorAndMessge
{
    public class ReturnMessage
    {
        public bool Status { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Models
{
    public class Log :BaseEntity<string>
    {
        public Log()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public string Application { get; set; }
        public DateTime Logged { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Models
{
    public class BaseEntity <T>
    {
        [System.ComponentModel.DataAnnotations.Key]
        public T Id { get; set; }
      
      
        [System.ComponentModel.DataAnnotations.Required]
        public DateTime CreatedDate { get; set; }
        
        
        [System.ComponentModel.DataAnnotations.Required]
        public DateTime ModifiedDate { get; set; }
    }
}

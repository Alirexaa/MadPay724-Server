using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Models
{
    public class BankCard :BaseEntity<string>
    {
        public BankCard()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;

        }

        [System.ComponentModel.DataAnnotations.Required]
        public string BankName { get; set; }

        public string Shaba { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(maximumLength: 16,MinimumLength = 16)]
        public string CardNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ExpiredDateMonth { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ExpiredDateYear { get; set; }

       
        
        [System.ComponentModel.DataAnnotations.Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}

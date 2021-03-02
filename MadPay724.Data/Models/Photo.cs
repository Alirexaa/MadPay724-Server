using System;
using System.Collections.Generic;
using System.Text;

namespace MadPay724.Data.Models
{
    public class Photo : BaseEntity<string>
    {
        public Photo()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;

        }
        [System.ComponentModel.DataAnnotations.Required]
        public string Url { get; set; }

        public string Description { get; set; }

        public string Alt { get; set; }


        [System.ComponentModel.DataAnnotations.Required]
        public bool IsMain { get; set; }


        [System.ComponentModel.DataAnnotations.Required]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}

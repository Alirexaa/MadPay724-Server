using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MadPay724.Data.Models
{
    public class User:BaseEntity<string>
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;

        }


        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string Name { get; set; }
       
        
        [Required]
        public string UserName { get; set; }

        
        [Required]
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Address { get; set; }

        
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string PhoneNumber { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public bool   Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastActive { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string City { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool Status { get; set; }

        public ICollection<Photo> Photos { get; set; }
        public ICollection<BankCard> BankCards { get; set; }

    }
}

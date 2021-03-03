using System;
using System.Collections.Generic;
using System.Text;

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
        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string UserName { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Address { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string PhoneNumber { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public byte[] PasswordHash { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public byte[] PasswordSalt { get; set; }


        public bool   Gender { get; set; }
        public DateTime DataOfBirth { get; set; }
        public string City { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public bool IsActive { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public bool Status { get; set; }

        public ICollection<Photo> Phohos { get; set; }
        public ICollection<BankCard> BankCards { get; set; }

    }
}

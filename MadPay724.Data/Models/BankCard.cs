using System;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [StringLength(maximumLength:0,MinimumLength =50)]
        public string BankName { get; set; }


        
        [Required]
        [StringLength(maximumLength:0,MinimumLength =100)]
        public string OwnerName { get; set; }


        [StringLength(maximumLength: 0, MinimumLength = 50)]
        public string Shaba { get; set; }
        
        [Required]
        [StringLength(maximumLength: 16,MinimumLength = 16)]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ExpireDateMonth { get; set; }

        [Required]
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ExpireDateYear { get; set; }

       
        
        //[System.ComponentModel.DataAnnotations.Required]
        //public int UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 0)]
        public string Url { get; set; }

        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Description { get; set; }

        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Alt { get; set; }


        [Required]
        public bool IsMain { get; set; }


        //[System.ComponentModel.DataAnnotations.Required]
        //public int UserId { get; set; }

        [Required]
        public User User { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Url { get; set; }
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Description { get; set; }
        [StringLength(maximumLength: 500, MinimumLength = 0)]
        public string Alt { get; set; }
        [Required]
        public bool IsMain { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public string PublicId { get; set; }
    }
}

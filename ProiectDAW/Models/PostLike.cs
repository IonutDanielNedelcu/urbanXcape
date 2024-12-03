
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    [NotMapped]
    public class PostLike
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int PostId { get; set; }
        [Required]
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }  
    }
}

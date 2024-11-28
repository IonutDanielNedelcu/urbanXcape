using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    
    public class PostLocation
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public virtual Post Post { get; set; }
        [Required]
        public virtual Location Location { get; set; }
    }
}

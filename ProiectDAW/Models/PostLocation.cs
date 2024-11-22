using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class PostLocation
    {
        [Required]
        public int IdPost { get; set; }
        [Required]
        public int IdLocation { get; set; }
        [Required]
        public virtual Post Post { get; set; }
        [Required]
        public virtual Location Location { get; set; }
    }
}

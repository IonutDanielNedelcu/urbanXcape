using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Rating
    {
        [Key]
        public int IdUser { get; set; }
        [Required]
        public int IdLocation { get; set; }
        [Required]
        public int Grade { get; set; }
        public string? Text { get; set; }
        [Required]
        public virtual Location Location { get; set; }
       // public virtual User User { get; set; }
    }
}

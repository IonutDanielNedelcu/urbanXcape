using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Rating
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public int LocationId { get; set; }
        [Required]
        public int Grade { get; set; }
        public string? Text { get; set; }
        [Required]
        public virtual Location Location { get; set; }
       // public virtual User User { get; set; }
    }
}

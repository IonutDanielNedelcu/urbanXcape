using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        public virtual ICollection<ApplicationUser>? Users { get; set; }
    }
}

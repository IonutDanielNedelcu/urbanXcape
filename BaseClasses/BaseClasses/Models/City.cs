using System.ComponentModel.DataAnnotations;

namespace BaseClasses.Models
{
    public class City
    {
        [Key]
        public int IdCity { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        public virtual ICollection<User>? Users { get; set; }
    }
}

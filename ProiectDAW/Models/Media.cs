using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Media
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public int IdPost { get; set; }
        [Required]
        virtual public Post Post { get; set; }
    }
}

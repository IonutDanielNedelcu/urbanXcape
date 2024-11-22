using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    [NotMapped]
    public class PostLike
    {
        [Required]
        public int IdUser { get; set; }
        [Required]
        public int IdPost { get; set; }
        [Required]
        public virtual Post Post { get; set; }
        //public virtual User User { get; set; }  
    }
}

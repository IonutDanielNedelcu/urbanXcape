using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Likes { get; set; }
        [Required]
        public int IdUser { get; set; }
        [Required]
        public int IdPost { get; set; }
        [Required]
        public virtual Post Post { get; set; }
        //public virtual User User { get; set; }
    }
}

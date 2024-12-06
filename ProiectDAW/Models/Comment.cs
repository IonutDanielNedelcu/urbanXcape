
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
        
        public string? UserId { get; set; }
        
        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<CommentLike>? CommentLikes { get; set; }
    }
}

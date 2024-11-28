using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProiectDAW.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Likes { get; set; }
        [Required]
        public int UserId { get; set; }
        public int? GroupId { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Media>? Media { get; set; }
        [Required]
        public virtual ICollection<PostLocation> PostLocations { get; set; }
        public virtual ICollection<PostLike>? PostLikes { get; set; }
        //public virtual User User { get; set; }
        //public virtual Group? Group { get; set; }

    }
}

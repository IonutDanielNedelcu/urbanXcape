
using System.ComponentModel.DataAnnotations;


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
        public string UserId { get; set; }
        public int? GroupId { get; set; }

        public string? Image { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Media>? Media { get; set; }
        [Required]
        public virtual ICollection<PostLocation> PostLocations { get; set; }
        public virtual ICollection<PostLike>? PostLikes { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Group? Group { get; set; }

    }
}

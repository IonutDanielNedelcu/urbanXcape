

namespace ProiectDAW.Models
{
    public class CommentLike
    {
        public string UserId { get; set; }
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}

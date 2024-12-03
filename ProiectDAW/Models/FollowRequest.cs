using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    
    public class FollowRequest
    {
        // Cheie primara compusa din IdFollower si IdFollowed

        [Required]
        public string FollowerId { get; set; } // userul care a trimis cererea

        [Required]
        public string FollowedId { get; set; } // userul urmarit

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool Accepted { get; set; }

        public virtual ApplicationUser? Follower { get; set; } // userul care a trimis cererea

        public virtual ApplicationUser? Followed { get; set; } // userul urmarit
    }
}

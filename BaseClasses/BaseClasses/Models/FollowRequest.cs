using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseClasses.Models
{
    [NotMapped]
    public class FollowRequest
    {
        // Cheie primara compusa din IdFollower si IdFollowed

        [Required]
        public int IdFollower { get; set; } // userul care a trimis cererea

        [Required]
        public int IdFollowed { get; set; } // userul urmarit

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool Accepted { get; set; }

        public virtual User? Follower { get; set; } // userul care a trimis cererea

        public virtual User? Followed { get; set; } // userul urmarit
    }
}

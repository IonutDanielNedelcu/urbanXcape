
using Microsoft.AspNetCore.Identity;
using ProiectDAW.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    public class ApplicationUser : IdentityUser
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        //[Required]
        //public string Username { get; set; }

        //[Required]
        //public string Email { get; set; }

        public string? Phone { get; set; }

        //[Required]
        //public string Password { get; set; }

        //[Required]
        public string? Description { get; set; }

        public string? ProfilePic { get; set; }

        [Required]
        public bool Privacy { get; set; }

        //[Required]
        //public bool Administrator { get; set; }

        public int? CityId { get; set; }
        public virtual City? City { get; set; }

        public virtual ICollection<FollowRequest>? Following { get; set; } // pe cine urmareste 

        public virtual ICollection<FollowRequest>? Followers { get; set; } // de cine e urmarit

        public virtual ICollection<GroupRequest>? GroupRequests { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }

        public virtual ICollection<Group>? ModeratedGroups { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Rating>? Ratings { get; set; }

        public virtual ICollection<PostLike>? PostLikes { get; set; }
        public virtual ICollection<CommentLike>? CommentLikes { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}

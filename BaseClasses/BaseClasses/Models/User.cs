using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseClasses.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Phone { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Description { get; set; }

        public string? ProfilePic { get; set; }

        [Required]
        public bool Privacy { get; set; }

        [Required]
        public bool Administrator { get; set; }

        public virtual City? IdHome { get; set; }

        [NotMapped]
        public virtual ICollection<FollowRequest>? Following { get; set; } // pe cine urmareste 

        [NotMapped]
        public virtual ICollection<FollowRequest>? Followers { get; set; } // de cine e urmarit

        public virtual ICollection<GroupRequest>? GroupRequests { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }

    }
}


using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        //[Required]
        public string? ModeratorId { get; set; }

        public virtual User? Moderator { get; set; }

        public virtual ICollection<GroupRequest>? GroupRequests { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
    }
}

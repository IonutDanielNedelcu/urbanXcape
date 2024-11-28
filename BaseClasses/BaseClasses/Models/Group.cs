using System.ComponentModel.DataAnnotations;

namespace BaseClasses.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public virtual User? ModeratorId { get; set; }

        public virtual ICollection<GroupRequest>? GroupRequests { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }
    }
}

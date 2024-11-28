using System.ComponentModel.DataAnnotations;

namespace BaseClasses.Models
{
    public class Group
    {
        [Key]
        public int IdGroup { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public virtual User? IdModerator { get; set; }

        public virtual ICollection<GroupRequest>? GroupRequests { get; set; }

        public virtual ICollection<UserGroup>? UserGroups { get; set; }
    }
}

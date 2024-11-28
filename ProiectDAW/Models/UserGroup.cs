
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    
    public class UserGroup
    {
        public string? UserId { get; set; }

        public int? GroupId { get; set; }

        public virtual User? User { get; set; }

        public virtual Group? Group { get; set; }
    }
}

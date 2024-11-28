
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectDAW.Models
{
    
    public class GroupRequest
    {
        // Cheie primara compusa din IdUser si IdGroup

        public string UserId { get; set; }

        public int GroupId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? Text { get; set; }

        public virtual User? User { get; set; }

        public virtual Group? Group { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace BaseClasses.Models
{
    public class GroupRequest
    {
        // Cheie primara compusa din IdUser si IdGroup

        public int UserId { get; set; }

        public int GroupId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? Text { get; set; }

        public virtual User? User { get; set; }

        public virtual Group? Group { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace BaseClasses.Models
{
    public class GroupRequest
    {
        // Cheie primara compusa din IdUser si IdGroup

        public int IdUser { get; set; }

        public int IdGroup { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? Text { get; set; }

        public virtual User? User { get; set; }

        public virtual Group? Group { get; set; }

    }
}

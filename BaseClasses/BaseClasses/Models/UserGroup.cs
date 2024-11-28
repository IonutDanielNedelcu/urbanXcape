namespace BaseClasses.Models
{
    public class UserGroup
    {
        public int? IdUser { get; set; }

        public int? IdGroup { get; set; }

        public virtual User? User { get; set; }

        public virtual Group? Group { get; set; }
    }
}

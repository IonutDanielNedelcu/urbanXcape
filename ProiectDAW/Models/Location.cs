using System.ComponentModel.DataAnnotations;

namespace ProiectDAW.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        //public string Name { get; set; }
        //public string? Description { get; set; } //optional
        [Required]
        public int RatingCounter { get; set; } //nr de rating uri
        [Required]
        public float RatingScore { get; set; }
        [Required]
        public string Address { get; set; }
        //[Required]
        //public bool Valid { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

        [Required]
        public virtual ICollection<Rating> Ratings { get; set; }
        [Required]
        public virtual ICollection<PostLocation> PostLocations { get; set; }  

    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Models;

namespace ProiectDAW.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<PostLocation> PostLocations { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Rating> Ratigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity<PostLocation>()
                .HasKey(pl => new {pl.IdPost, pl.IdLocation});//pl = o instanță a entității PostLocation (param de intrare)
                                                              //new {pl.IdPost, pl.IdLocation} = tip anonim (anonymous type) care conține valorile din proprietățile IdPost și IdLocation
            // definire relatii cu modelele Post si Location (FK)
            modelBuilder.Entity<PostLocation>()
                .HasOne(pl => pl.Post)
                .WithMany(pl => pl.PostLocations)
                .HasForeignKey(pl => pl.IdPost);

            modelBuilder.Entity<PostLocation>()
                .HasOne(pl => pl.Location)
                .WithMany(pl => pl.PostLocations)
                .HasForeignKey(pl => pl.IdLocation);


            //modelBuilder.Entity<PostLike>()
            //    .HasKey(pl => new { pl.IdUser, pl.IdPost });
        }
    }
}


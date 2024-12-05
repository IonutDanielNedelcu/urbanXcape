using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProiectDAW.Models;

namespace ProiectDAW.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Rating> Ratigs { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<FollowRequest> FollowRequests { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupRequest> GroupRequests { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }    
        public DbSet<CommentLike> CommentLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // definire primary key compus
            modelBuilder.Entity<PostLocation>()
                .HasKey(pl => new { pl.PostId, pl.LocationId });//pl = o instanță a entității PostLocation (param de intrare)
                                                                //new {pl.IdPost, pl.IdLocation} = tip anonim (anonymous type) care conține valorile din proprietățile IdPost și IdLocation
                                                                // definire relatii cu modelele Post si Location (FK)
            modelBuilder.Entity<PostLocation>()
                .HasOne(pl => pl.Post)
                .WithMany(pl => pl.PostLocations)
                .HasForeignKey(pl => pl.PostId);

            modelBuilder.Entity<PostLocation>()
                .HasOne(pl => pl.Location)
                .WithMany(pl => pl.PostLocations)
                .HasForeignKey(pl => pl.LocationId);


            modelBuilder.Entity<PostLike>()
                .HasKey(pl => new { pl.UserId, pl.PostId });

            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(pl => pl.UserId);

            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(pl => pl.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            // FollowRequest PK
            modelBuilder.Entity<FollowRequest>()
                .HasKey(fr => new { fr.FollowerId, fr.FollowedId });

            // FollowRequest FK
            modelBuilder.Entity<FollowRequest>()
                .HasOne(fr => fr.Follower) // instanta de User care urmareste
                .WithMany(u => u.Following)
                .HasForeignKey(fr => fr.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FollowRequest>()
                .HasOne(fr => fr.Followed) // instanta de User care e urmarit
                .WithMany(u => u.Followers)
                .HasForeignKey(fr => fr.FollowedId);


            modelBuilder.Entity<Comment>()
               .HasOne(c => c.Post)
               .WithMany(p => p.Comments)
               .HasForeignKey(c => c.PostId)
               .OnDelete(DeleteBehavior.Cascade);


            // GroupRequest PK
            modelBuilder.Entity<GroupRequest>()
                .HasKey(gr => new { gr.UserId, gr.GroupId });

            // GroupRequest FK
            modelBuilder.Entity<GroupRequest>()
                .HasOne(gr => gr.User)
                .WithMany(gr => gr.GroupRequests)
                .HasForeignKey(gr => gr.UserId);

            modelBuilder.Entity<GroupRequest>()
                .HasOne(gr => gr.Group)
                .WithMany(gr => gr.GroupRequests)
                .HasForeignKey(gr => gr.GroupId);

            // UserGroup PK
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            // UserGroup FK
            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(ug => ug.UserGroups)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(ug => ug.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            modelBuilder.Entity<CommentLike>()
                .HasKey(cl => new { cl.UserId, cl.CommentId });

            modelBuilder.Entity<CommentLike>()
                .HasOne(cl => cl.User)
                .WithMany(u => u.CommentLikes)
                .HasForeignKey(cl => cl.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CommentLike>()
                .HasOne(cl => cl.Comment)
                .WithMany(c => c.CommentLikes)
                .HasForeignKey(cl => cl.CommentId);

            modelBuilder.Entity<Rating>()
                .HasKey(r => new { r.UserId, r.LocationId });

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Location)
                .WithMany(l => l.Ratings)
                .HasForeignKey(r => r.LocationId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.NoAction);



        }
    }
}


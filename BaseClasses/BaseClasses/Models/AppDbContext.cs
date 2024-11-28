using Microsoft.EntityFrameworkCore;

namespace BaseClasses.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        //public DbSet<FollowRequest> FollowRequests { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupRequest> GroupRequests { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
            // FollowRequest PK
            modelBuilder.Entity<FollowRequest>()
                .HasKey(fr => new { fr.IdFollower, fr.IdFollowed });

            // FollowRequest FK
            modelBuilder.Entity<FollowRequest>()
                .HasOne(fr => fr.Follower) // instanta de User care urmareste
                .WithMany(u => u.Following)
                .HasForeignKey(fr => fr.IdFollower)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FollowRequest>()
                .HasOne(fr => fr.Followed) // instanta de User care e urmarit
                .WithMany(u => u.Followers)
                .HasForeignKey(fr => fr.IdFollowed)
                .OnDelete(DeleteBehavior.Restrict);
            */

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
        }
    }
}

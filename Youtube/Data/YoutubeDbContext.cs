using Microsoft.EntityFrameworkCore;
using System.Linq;
using Youtube.Models;

namespace Youtube.Data
{
    public class YoutubeDbContext : DbContext
    {
        public YoutubeDbContext(DbContextOptions options)
            :base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(v => v.UserVideos)
                                       .WithOne(u => u.Owner);
            modelBuilder.Entity<Follow>().HasKey(table => new {
                table.FollowerId,
                table.FollowingId
            });
            
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<LikeDislikeVideo> LikeDislikeVideos { get; set; }
        public DbSet<LikeDislikeComment> LikeDislikeComments { get; set; }
        public DbSet<Follow> Follow { get; set; }

    }
}

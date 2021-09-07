using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;


namespace BlogApi.Models
{
    public class BlogApiContext : DbContext
    {
        public BlogApiContext(DbContextOptions<BlogApiContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity => { entity.Property(e => e.Title).IsRequired(); });
            modelBuilder.Entity<Post>().HasData(
                new Post { Title = "post1", PostId = 1 },
                new Post { Title = "post2", PostId = 2 },
                new Post { Title = "post3", PostId = 3 });
            modelBuilder.Entity<Comment>(

                entity =>
                {
                    entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey("PostId");
                });

            modelBuilder.Entity<Comment>().HasData(
                new Comment { CommentId = 1, PostId = 1, CommentText = "comment1", CommentDate = new DateTime(2021, 5, 5, 6, 12, 52) },
                new Comment { CommentId = 2, PostId = 1, CommentText = "comment2", CommentDate = new DateTime(2021, 6, 5, 6, 12, 52) },
                new Comment { CommentId = 3, PostId = 2, CommentText = "comment3", CommentDate = new DateTime(2021, 5, 5, 6, 12, 52) },
                new Comment { CommentId = 4, PostId = 2, CommentText = "comment4", CommentDate = DateTime.Now },
                new Comment { CommentId = 5, PostId = 3, CommentText = "comment5", CommentDate = new DateTime(2021, 8, 5, 6, 12, 52) },
                new Comment { CommentId = 6, PostId = 3, CommentText = "comment6", CommentDate = new DateTime(2021, 9, 5, 6, 12, 52) }
                );
        }

    }
}
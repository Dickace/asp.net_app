using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;


namespace BlogApi.Models
{
    public class BlogApiContext : DbContext
    {
        public BlogApiContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}blog.db";
        }



        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public string DbPath { get; private set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

    }
}
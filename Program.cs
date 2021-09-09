using BlogApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new BlogApiContext())
            {
                if (db.Posts.Any())
                {
                    db.Posts.RemoveRange(db.Posts);
                    db.Comments.RemoveRange(db.Comments);
                    db.SaveChangesAsync();
                }
                    var newPost1 = new Post { Title = "post1" };
                    var newPost2 = new Post { Title = "post2" };
                    var newPost3 = new Post { Title = "post3" };
                    db.Posts.AddRange(newPost1, newPost2, newPost3);
                    db.Comments.AddRange(
                        new Comment { CommentText = "comment1 for post1", CommentDate = new DateTime(2020, 2, 9),PostId= newPost1.ID, Post = newPost1 },
                        new Comment { CommentText = "comment2 for post1", CommentDate = new DateTime(2020, 3, 8), PostId = newPost1.ID, Post = newPost1 },
                        new Comment { CommentText = "comment3 for post1", CommentDate = new DateTime(2020, 4, 7), PostId = newPost1.ID, Post = newPost1 },
                        new Comment { CommentText = "comment1 for post2", CommentDate = new DateTime(2020, 5, 6), PostId = newPost2.ID, Post = newPost2 },
                        new Comment { CommentText = "comment2 for post2", CommentDate = new DateTime(2020, 6, 5), PostId = newPost2.ID, Post = newPost2 },
                        new Comment { CommentText = "comment3 for post2", CommentDate = new DateTime(2020, 7, 4), PostId = newPost2.ID, Post = newPost2 },
                        new Comment { CommentText = "comment1 for post3", CommentDate = new DateTime(2020, 8, 3), PostId = newPost3.ID, Post = newPost3 },
                        new Comment { CommentText = "comment2 for post3", CommentDate = new DateTime(2020, 9, 2), PostId = newPost3.ID, Post = newPost3 }
                        );
                    db.SaveChangesAsync();
                var lastComments = new List<Comment>();
                var result = new List<string>();

                foreach (var p in db.Posts)
                {
                    lastComments.Add(p.Comments.OrderBy(x => x.CommentDate)
                        .LastOrDefault());
                }
                lastComments.OrderBy(c => c.CommentDate).ToList();
                foreach (var c in lastComments)
                {
                    result.Insert(0, c.Post.Title + " - " + c.CommentText + " (" + c.CommentDate.ToString("d MMMM", CultureInfo.CreateSpecificCulture("ru-RU")) + ") ");
                }
              
                foreach(var i in result)
                {
                    Console.WriteLine(i);

                }
               
               
                
            }
            var host = CreateHostBuilder(args).Build();
            host.Run();

        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

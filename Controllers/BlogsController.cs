using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApi.Models;
using System.Globalization;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BlogApiContext _context;

        public BlogsController(BlogApiContext context)
        {
            _context = context;
        }
        [HttpGet("lastComments")]
        public async Task<ActionResult<List<string>>> GetLastComments()
        {
            var result = new List<string>();
            var lastComments = new List<Comment>();
            var posts = await _context.Posts.ToListAsync();
            foreach(var post in posts)
            {
                var lastComment = await _context.Comments
                    .Where(c => c.PostId == post.PostId)
                    .OrderBy( x => x.CommentDate)
                    .LastOrDefaultAsync();
                lastComments.Add(lastComment);
            }
            lastComments = lastComments.OrderBy(c => c.CommentDate).ToList();
            foreach (var comment in lastComments)
            {
                result.Insert(0,posts.Find(p => p.PostId == comment.PostId).Title + " - " + comment.CommentText.ToString() + " (" + comment.CommentDate.ToString("d MMMM", CultureInfo.CreateSpecificCulture("ru-RU")) + ") ");
            }
            return result;
        }// GET: api/Blogs
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Blogs/5
        [HttpGet("posts/{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpGet("comments/{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

       
        [HttpPost("posts")]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.PostId }, post);
        }

        [HttpPost("comments")]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.CommentId }, comment);
        }
    }
}

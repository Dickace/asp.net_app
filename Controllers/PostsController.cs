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
    [Route("api/Blogs")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly BlogApiContext _context;

        public PostsController(BlogApiContext context)
        {
            _context = context;
        }
        [HttpGet("lastcomments")]
        public async Task<ActionResult<List<string>>> GetLastComments()
        {
            var result = new List<string>();

            var lac = _context.Comments.GroupBy(c => c.PostId)
                                        .Select(g => new { postid = g.Key, commentDate = g.Max(c => c.CommentDate) })
                                        .Join(_context.Comments,
                                        gc => new { pi = gc.postid,cd = gc.commentDate },
                                        c => new { pi = c.PostId,cd = c.CommentDate },
                                        (gc, c) => new
                                        {
                                            gc.commentDate,
                                            c.CommentText,
                                            c.PostId
                                        })
                                        .Join(_context.Posts,
                                        cc => cc.PostId,
                                        p => p.ID,
                                        (cc, p) => new
                                        {
                                            title = p.Title,
                                            commentText = cc.CommentText,
                                            commentDate = cc.commentDate,
                                        })
                                        .OrderByDescending(lc => lc.commentDate)
                                        .Take(5);

            foreach (var c in lac)
            {
                result.Add(c.title + " - " + c.commentText + " (" + c.commentDate.ToString("d MMMM", CultureInfo.CreateSpecificCulture("ru-RU")) + ") ");
            }
            return result;
        }
        // GET: api/Posts
        [HttpGet("posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }
        // GET: api/Comments
        [HttpGet("comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.ToListAsync();
        }

        // GET: api/Posts/5
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

            return CreatedAtAction(nameof(GetPost), new { id = post.ID }, post);
        }

        [HttpPost("comments")]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetComment), new { id = comment.CommentId, }, comment);
        }



    }
}

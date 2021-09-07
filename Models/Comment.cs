using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentText { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }

}
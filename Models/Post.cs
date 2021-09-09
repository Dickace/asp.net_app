using System.Collections.Generic;

namespace BlogApi.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}

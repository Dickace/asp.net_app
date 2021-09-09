# asp.net_app
#### ASP.NET Web Api for Tomsk MCC. Using EFC code-first with DB Sqlite.
#### Requried API endpoint
GET /api/blogs/lastcomments get all posts with last comment sorted by last comment date 
##### API Endpoints:
GET /api/blogs/comments Gets all comments\
GET /api/blogs/comment/{id} Get comment by id\
POST /api/blogs/comments Create a comment with text, publicationDate, and PostId
request example: 
<pre>
{
    CommentText: "Comment1",
    CommentDate: "2021-02-15T00:00:00Z",
    PostId: 1
}
</pre>
GET /api/blogs/posts Gets all Posts\
GET /api/blogs/posts/{id} Get post by id\
POST /api/blogs/comments Create a post with title\
request example:
<pre> 
{  
    Title: "Post1",
}
</pre>

using System;
using System.Linq;
using System.Reflection;

namespace DB
{
    public interface IPostsRepository
    {
        int Create(string title, string content, int authorId);
        Post Update(int id, string title, string content, int? authorId );
        Post Get(int id);

        BloggingContext GetContext();
    }

    public class PostsRepository : IPostsRepository
    {
        private BloggingContext _db;
        
        public PostsRepository(BloggingContext db)
        {
            _db = db;
        }

        public Post Get(int id)
        {
           return _db.Posts.FirstOrDefault(x => x.PostId == id);
        }

        public BloggingContext GetContext()
        {
            return _db;
        }

        public int Create(string title, string content, int authorId)
        {
            var post = new Post
            {
                Title = title,
                Content = content,
                AuthorId = authorId
            };
        
            _db.Posts.Add(post);

            _db.SaveChanges();

            return post.AuthorId;
        }
    
        public Post Update(int id, string title, string content, int? authorId )
        {
            var post = _db.Posts.FirstOrDefault(x => x.PostId == id);

            if (!String.IsNullOrEmpty(title))
                post.Title = title;
            
            if (!String.IsNullOrEmpty(content))
                post.Content = content;
            
            if (authorId.HasValue)
                post.AuthorId = (int) authorId;
            
            _db.SaveChanges();

            return post;
        }
    }
}
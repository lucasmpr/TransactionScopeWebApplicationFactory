using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DB
{
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    
    public class BloggingContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseLazyLoadingProxies();
        }
    }

    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual List<Post> Posts { get; set; }
    }

    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        
        public int AuthorId { get; set; }
        public virtual Author? Author { get; set; }
    }
    
     
}
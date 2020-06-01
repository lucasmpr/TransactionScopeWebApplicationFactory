using System.Linq;

namespace DB
{
    public interface IAuthorRepository
    {
        int Create(string name);
        Author Update(int id, string name);
    }

    public class AuthorRepository : IAuthorRepository
    {
        private BloggingContext _db;
        
        public AuthorRepository(BloggingContext db)
        {
            _db = db;
        }
        
        public int Create(string name)
        {
            var author = new Author()
            {
                Name = name
            };
            
            _db.Authors.Add(author);

            _db.SaveChanges();

            return author.AuthorId;
        }
        
        public Author Update(int id, string name)
        {
            var author = _db.Authors.FirstOrDefault(x => x.AuthorId == id);
            author.Name = name;
            _db.SaveChanges();

            return author;
        }
    }
}
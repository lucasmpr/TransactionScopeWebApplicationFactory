using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Transactions;
using DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    [Route("[controller]")]
    [ApiController]
    public class Home : Controller
    {
        private IAuthorRepository _authorRepository;
        private IPostsRepository _postsRepository;

        public Home(IAuthorRepository authorRepository, IPostsRepository postsRepository)
        {
            _authorRepository = authorRepository;
            _postsRepository = postsRepository;
        }
        
        [Route("dostuff")]
        [HttpPost]
        public IActionResult dostuff()
        {
            using var transaction =  new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() {IsolationLevel = IsolationLevel.RepeatableRead});
            
            var author = _authorRepository.Create(RandomString(10));
            var posts = _postsRepository.Create(RandomString(10), RandomString(10), author);
            
            var post = _postsRepository.Get(posts);
            
            var authorAgain = post;

            return Ok(JsonSerializer.Serialize(authorAgain));
        }
        
        
        //From StackOverflow: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    
    [Route("[controller]")]
    [ApiController]
    public class NotHome : Controller
    {
        private IAuthorRepository _authorRepository;
        private IPostsRepository _postsRepository;

        public NotHome(IAuthorRepository authorRepository, IPostsRepository postsRepository)
        {
            _authorRepository = authorRepository;
            _postsRepository = postsRepository;
        }
        
        [Route("dostuff")]
        [HttpPost]
        public IActionResult dostuff()
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() {IsolationLevel = IsolationLevel.RepeatableRead});
            
            var author = _authorRepository.Create(RandomString(10));
            var posts = _postsRepository.Create(RandomString(10), RandomString(10), author);
            
            Thread.Sleep(3000);
            
            var post = _postsRepository.Get(posts);
            transaction.Complete();
            return Ok(post);
        }
        
        
        //From StackOverflow: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
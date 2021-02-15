using Forum.Models;
using Forum.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {

        private readonly ApplicationDbContext _db;

        public PostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public bool CheckIfPostIsFirst(Post post)
        {
            var firstPost = _db.Post.Where(m => m.TopicId == post.TopicId).OrderBy(m => m.PostDate).FirstOrDefault();

            if (firstPost == null || (firstPost.Id == post.Id))
                return true;

            return false;
        }

        public async Task<Post> GetLastPostForCategory(int categoryId)
        {
            return await _db.Post.Where(m => m.CategoryId == categoryId).Include(m => m.User).Include(m => m.Topic).OrderByDescending(m => m.PostDate).FirstOrDefaultAsync();
        }

        public async Task<Post> GetLastPostForTopic(int topicId)
        {
            return await _db.Post.Where(m => m.TopicId == topicId).Include(m => m.User).OrderByDescending(m => m.PostDate).FirstOrDefaultAsync();
        }

        public void UpdatePostContent(int id, string postMessage)
        {
            var postToEdit = _db.Post.Find(id);

            postToEdit.PostMessage = postMessage;
        }
    }
}

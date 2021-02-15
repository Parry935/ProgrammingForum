using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Interfaces.Data
{
    public interface IPostRepository : IRepository<Post>
    {

        Task<Post> GetLastPostForCategory(int categoryId);

        Task<Post> GetLastPostForTopic(int topicId);
        void UpdatePostContent(int id, string postMessage);
        bool CheckIfPostIsFirst(Post post);
    }
}

using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Interfaces.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void increasPostCount(int categoryId);

        void increasTopicAndPostCount(int categoryId);

        void decreasePostCount(int categoryId);

        void decreaseTopicAndPostCount(int categoryId, int topicId);
    }
}

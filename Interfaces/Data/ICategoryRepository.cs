using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Interfaces.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void IncreasePostCount(int categoryId);

        void IncreaseTopicAndPostCount(int categoryId);

        void DecreasePostCount(int categoryId);

        void DecreaseTopicAndPostCount(int categoryId, int topicId);
    }
}

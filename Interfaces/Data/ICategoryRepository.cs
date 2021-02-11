using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Interfaces.Data
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void addPostCount(int categoryId);

        void addTopicAndPostCount(int categoryId);
    }
}

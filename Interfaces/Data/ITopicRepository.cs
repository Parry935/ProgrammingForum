using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Interfaces.Data
{
    public interface ITopicRepository : IRepository<Topic>
    {
        void addPostCount(int topicId);
    }
}

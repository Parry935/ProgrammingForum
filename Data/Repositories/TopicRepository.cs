using Forum.Models;
using Forum.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Repositories
{
    public class TopicRepository : Repository<Topic>, ITopicRepository
    {
        private readonly ApplicationDbContext _db;

        public TopicRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //crate post
        public void IncreasePostCount(int topicId)
        {
            var topic = _db.Topic.Find(topicId);

            topic.CountPosts += 1;

            _db.Topic.Update(topic);
        }

        //delete post
        public void DecreasePostCount(int topicId)
        {
            var topic = _db.Topic.Find(topicId);

            topic.CountPosts -= 1;

            _db.Topic.Update(topic);
        }
    }
}

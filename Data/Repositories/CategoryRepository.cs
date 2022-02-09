using Forum.Interfaces.Data;
using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //crate post
        public void IncreasePostCount(int categoryId)
        {
            var category = _db.Category.Find(categoryId);

            category.CountPosts += 1;

            _db.Category.Update(category);
        }

        //create topic
        public void IncreaseTopicAndPostCount(int categoryId)
        {
            var category = _db.Category.Find(categoryId);

            category.CountPosts += 1;
            category.CountTopics += 1;

            _db.Category.Update(category);
        }

        //delete post
        public void DecreasePostCount(int categoryId)
        {
            var category = _db.Category.Find(categoryId);

            category.CountPosts -= 1;

            _db.Category.Update(category);
        }

        //delete topic
        public void DecreaseTopicAndPostCount(int categoryId, int topicId)
        {
            var category = _db.Category.Find(categoryId);

            var cntPost = _db.Topic.Find(topicId).CountPosts;

            category.CountPosts -= cntPost;
            category.CountTopics -= 1;

            _db.Category.Update(category);
        }
    }
}

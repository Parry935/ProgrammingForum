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

        public void addPostCount(int categoryId)
        {
            var category = _db.Category.Find(categoryId);

            category.CountPosts += 1;

            _db.Category.Update(category);
        }

        public void addTopicAndPostCount(int categoryId)
        {
            var category = _db.Category.Find(categoryId);

            category.CountPosts += 1;
            category.CountTopics += 1;

            _db.Category.Update(category);
        }
    }
}

using Forum.Data.Repositories;
using Forum.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _dbContext;

        //Repositories
        public ICategoryRepository Category { get; private set; }

        public ITopicRepository Topic { get; private set; }

        public IPostRepository Post { get; private set; }

        public IUserRepository User { get; private set; }

        public ILikeRepository Like { get; private set; }

        public IReportRepository Report { get; private set; }


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            Category = new CategoryRepository(_dbContext);
            Topic = new TopicRepository(_dbContext);
            Post = new PostRepository(_dbContext);
            User = new UserRpository(_dbContext);
            Like = new LikeRepository(_dbContext);
            Report = new ReportRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}

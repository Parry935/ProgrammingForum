using Forum.Interfaces.Data;
using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Repositories
{
    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}

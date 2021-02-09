using Forum.Models;
using Forum.Interfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Repositories
{
    public class UserRpository : Repository<User>, IUserRepository
    {
        public UserRpository(ApplicationDbContext db) : base(db)
        {
        }
    }
}

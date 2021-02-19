using Forum.Interfaces.Data;
using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}

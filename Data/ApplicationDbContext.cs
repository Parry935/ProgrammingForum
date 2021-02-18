using System;
using System.Collections.Generic;
using System.Text;
using Forum.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Report> Report { get; set; }
    }
}

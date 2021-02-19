using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModel
{
    public class UserProfileVM
    {
        public User User { get; set; }
        public int CountTopics { get; set; }
        public int CountPosts { get; set; }
        public int Reputation { get; set; }
        public IEnumerable<Post> RecentPosts { get; set; }
    }
}

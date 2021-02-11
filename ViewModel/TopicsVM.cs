using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModel
{
    public class TopicsVM
    {
        public IEnumerable<TopicWithPostVM> TopicsWithLastPosts { get; set; }
        public Category Category { get; set; }
    }
}

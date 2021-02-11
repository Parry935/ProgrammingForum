using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModel
{
    public class TopicWithPostVM
    {
        public Post Post { get; set; }
        public Topic Topic { get; set; }
    }
}

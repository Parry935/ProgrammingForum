using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModel
{
    public class TopicVM
    {
        public IEnumerable<Topic> Topics { get; set; }
        public Category Category { get; set; }
    }
}

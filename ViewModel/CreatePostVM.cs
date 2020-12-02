using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModel
{
    public class CreatePostVM
    {
        public IEnumerable<Post> Posts { get; set; }
        public Post NewPost { get; set; }
    }
}

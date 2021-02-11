using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModel
{
    public class HomeVM
    {
        public Category Category { get; set; }
        public Post LastPost { get; set; }
    }
}

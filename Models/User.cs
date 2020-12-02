using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Nick { get; set; }
        public string Image { get; set; }

    }
}

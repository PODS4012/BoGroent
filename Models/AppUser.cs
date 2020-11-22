using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Models
{
    public class AppUser : IdentityUser
    {
        public string  Name { get; set; }
        public string Address { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Models
{
    public class Login
    {
        [Required, MinLength(7, ErrorMessage = "Minimum length is 7")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}

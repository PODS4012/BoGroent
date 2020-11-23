using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Models
{
    public class RoleEdit
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required"), Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoGroent.Models
{
    public class UserEdit
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please provide full name"), StringLength(100, MinimumLength = 4)]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please provide email"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please provide a phone number"), DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please provide address"), StringLength(100, MinimumLength = 6)]
        public string Address { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password), MinLength(4, ErrorMessage = "Minimum length is 4")]
        public string Password { get; set; }

        public UserEdit() { }

        public UserEdit(AppUser appUser)
        {
            Id = appUser.Id;
            Name = appUser.Name;
            Email = appUser.Email;
            PhoneNumber = appUser.PhoneNumber;
            Address = appUser.Address;
            UserName = appUser.UserName;
            Password = appUser.PasswordHash;
        }
    }
}

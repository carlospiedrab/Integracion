using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Dtos.User
{
    public class AddOrUpdateAppUserDto
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; } = string.Empty;
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Role { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Dtos
{
    public record RegisterDto
    (
        [Required(ErrorMessage = "Username is required")]
        string Username,

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        string Email,

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).{6,}$",
            ErrorMessage = "Password must be at least 6 characters long and must contain at least one letter and one number")]
        string Password
    );
}

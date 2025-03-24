using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } // ✅ Fixed casing & validation

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain at least one letter and one number.")]
        public string Password { get; set; } // ✅ Fixed casing
    }
}

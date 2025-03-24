using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Full name must be at most 100 characters long.")]
        public string FullName { get; set; }



        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must include at least one letter, one number, and be at least 8 characters long.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class ResetPasswordRequestDto
    {
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$",
            ErrorMessage = "Password must contain at least one letter, one number, and be at least 8 characters long.")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

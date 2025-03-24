using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class ForgetPasswordRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } // ✅ Fixed casing & validation
    }
}

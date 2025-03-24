using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class UserDto
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } // ✅ Fixed to match `User` entity

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; } // ✅ Fixed validation issue

        public string Token { get; set; } // ✅ Token moved to end for readability
    }
}

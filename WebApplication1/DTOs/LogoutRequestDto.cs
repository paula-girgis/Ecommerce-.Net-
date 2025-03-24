using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Api.DTOs
{
    public class LogoutRequestDto
    {
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; }
    }
}

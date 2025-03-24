using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Core.Entities;

public class User : IdentityUser<Guid>
{
    [Required]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public override string Email { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Cart? Cart { get; set; }
    public ICollection<Token>? Tokens { get; set; }
}

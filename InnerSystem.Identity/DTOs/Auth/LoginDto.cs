using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Auth;

public class LoginDto
{
	[EmailAddress]
	[Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
	[MinLength(8)]
	[MaxLength(30)]
	public string? Password { get; set; }
}

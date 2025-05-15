using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Auth;

public class ForgetPasswordDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Email is wrong.")]
    public string Email { get; set; }
}

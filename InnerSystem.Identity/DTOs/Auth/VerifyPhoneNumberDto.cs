using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Auth;

public class VerifyPhoneNumberDto
{
    [EmailAddress(ErrorMessage = "you should enter email address.")]
    [Required]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Code coming from email is required! ")]
    public string? Code { get; set; }
}

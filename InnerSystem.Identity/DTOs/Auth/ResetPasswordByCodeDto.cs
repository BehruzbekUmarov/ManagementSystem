using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Auth;

public class ResetPasswordByCodeDto
{
	[Required]
	[EmailAddress(ErrorMessage = "You have to enter email. ")]
    public string Email { get; set; }
    [DataType(DataType.Password)]
	[Required(ErrorMessage = "You have to enter new password.")]
	public string NewPassword { get; set; }

	[Required(ErrorMessage = "You have to enter verification code")]
	public string Code { get; set; }
}

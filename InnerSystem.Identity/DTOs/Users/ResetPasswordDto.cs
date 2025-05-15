using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Users;

public class ResetPasswordDto
{
	[Required]
	public Guid UserId { get; set; }

	[Required]
	public string NewPassword { get; set; }

	[Required]
	public string ConfirmPassword { get; set; }

	[Required]
	public string OldPassword { get; set; }
}

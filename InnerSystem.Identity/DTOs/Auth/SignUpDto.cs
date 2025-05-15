using InnerSystem.Identity.Attributes;
using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Auth;

public class SignUpDto
{
	[EmailAddress(ErrorMessage = "this should be real email")]
    public string Email { get; set; }
    [Required(ErrorMessage = "This field is Required.")]
	[StringLength(100, ErrorMessage = "Minimum Length = 8 !", MinimumLength = 8)]
	[DataType(DataType.Password)]
	public string Password { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is Required.")]
	[DataType(DataType.Password)]
	[Display(Name = "Confirm password")]
	[Compare("Password", ErrorMessage = "It is not the same Password!")]
	public string ConfirmPassword { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is Required.")]
	public string FirstName { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is Required.")]
	public string LastName { get; set; } = string.Empty;

	[Required(ErrorMessage = "This field is Required.")]
	[AllowedRoles]
	public string Role { get; set; } = string.Empty;
}

using InnerSystem.Identity.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Users;

public class CreateUserDto
{
	[Required]
	public string FirstName { get; set; }

	[Required]
	public string LastName { get; set; }

	[Required]
	public string RoleName { get; set; }

	[Required]
	public string Password { get; set; }
	[Required]
	public Branch Branch { get; set; }
	[Required]
	public decimal Salary { get; set; }

	public string? Email { get; set; }
	public GenderEnum? Gender { get; set; }
	public DateTime BirthDate { get; set; }
}

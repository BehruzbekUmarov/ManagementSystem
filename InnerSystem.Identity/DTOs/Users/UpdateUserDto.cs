using InnerSystem.Identity.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Users;

public class UpdateUserDto
{
	public Guid Id { get; set; }

	[Required]
	public string FirstName { get; set; }

	[Required]
	public string LastName { get; set; }

	public string? Email { get; set; }
	public GenderEnum? Gender { get; set; }
	public DateTime BirthDate { get; set; }
}

using InnerSystem.Identity.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.DTOs.Users;

public class UserDto
{
	[Required]
	public Guid Id { get; set; }

	public string FirstName { get; set; }
	public string LastName { get; set; }
	public decimal Salary { get; set; }
	public GenderEnum? Gender { get; set; } = GenderEnum.Male;
	public DateTime BirthDate { get; set; }
	public Branch Branch { get; set; } = Branch.Bosh_Ofis;
	public DateTime CreateDate { get; set; } = DateTime.Now;
	public int GivenPoint { get; set; } = 0;
	public bool IsActive { get; set; } = true;
}

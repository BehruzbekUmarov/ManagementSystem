using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Api.DTOs.Assignment;

public class CreateAssignmentDto
{
	[Required]
	[StringLength(100, MinimumLength = 3)]
	public string Title { get; set; } = null!;

	[Required]
	[StringLength(1000, MinimumLength = 5)]
	public string Description { get; set; } = null!;

	[Required]
	public Guid AssignedToId { get; set; }
}

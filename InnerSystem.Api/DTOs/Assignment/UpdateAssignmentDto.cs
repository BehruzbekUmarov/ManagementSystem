using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Api.DTOs.Assignment;

public class UpdateAssignmentDto
{
	[Required]
	[StringLength(100, MinimumLength = 3)]
	public string Title { get; set; } = null!;

	[Required]
	[StringLength(10000, MinimumLength = 5)]
	public string Description { get; set; } = null!;

	[Required]
	public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;

	[Required]
	public Guid AssignedToId { get; set; }
}

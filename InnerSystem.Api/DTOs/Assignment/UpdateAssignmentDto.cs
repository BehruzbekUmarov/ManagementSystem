namespace InnerSystem.Api.DTOs.Assignment;

public class UpdateAssignmentDto
{
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;

	public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;

	public Guid AssignedToId { get; set; }
}

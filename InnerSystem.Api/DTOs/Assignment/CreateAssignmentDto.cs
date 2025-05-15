namespace InnerSystem.Api.DTOs.Assignment;

public class CreateAssignmentDto
{
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;

	public Guid AssignedToId { get; set; }
}

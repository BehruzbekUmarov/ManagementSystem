namespace InnerSystem.Api.DTOs.Assignment;

public class AssignmentDto
{
	public Guid Id { get; set; }
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;

	public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;

	public Guid AssignedToId { get; set; }

	public Guid CreatedById { get; set; }

	public bool IsDeleted { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

	public DateTime? UpdatedDate { get; set; }
}

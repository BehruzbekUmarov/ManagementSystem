using InnerSystem.Api.Entities.Base;
using InnerSystem.Identity.Models;

namespace InnerSystem.Api.Entities;

public class Assignment : BaseEntity
{
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;

	public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;

	public Guid AssignedToId { get; set; }

    public Guid CreatedById { get; set; }
}

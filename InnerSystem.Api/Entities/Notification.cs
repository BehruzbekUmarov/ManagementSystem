using InnerSystem.Api.Entities.Base;
using InnerSystem.Identity.Models;

namespace InnerSystem.Api.Entities;

public class Notification : BaseEntity
{
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;
	public bool IsRead { get; set; } = false;

	public Guid UserId { get; set; }
}

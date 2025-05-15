using InnerSystem.Api.Entities.Base;
using InnerSystem.Identity.Models;

namespace InnerSystem.Api.Entities;

public class Comment : BaseEntity
{
	public string Content { get; set; } = null!;

	// Relationships
	public Guid PostId { get; set; }
	public Post Post { get; set; } = null!;

	public Guid AuthorId { get; set; }
}

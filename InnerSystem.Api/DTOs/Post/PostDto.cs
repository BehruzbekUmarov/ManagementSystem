using InnerSystem.Api.Enums;

namespace InnerSystem.Api.DTOs.Post;

public class PostDto
{
	public Guid Id { get; set; }
	
	public string Title { get; set; } = null!;
	public string Body { get; set; } = null!;
	public string? Image { get; set; }

	public PostStatus Status { get; set; }

	public Guid AuthorId { get; set; }

	public bool IsDeleted { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

	public DateTime? UpdatedDate { get; set; }
}

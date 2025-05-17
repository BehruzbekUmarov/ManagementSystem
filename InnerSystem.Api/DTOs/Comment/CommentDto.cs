using InnerSystem.Api.DTOs.Post;

namespace InnerSystem.Api.DTOs.Comment;

public class CommentDto
{
	public Guid Id { get; set; }
	
	public string Content { get; set; } = null!;

	//public Guid PostId { get; set; }
	public ShallowPostDto Post { get; set; }

	public Guid AuthorId { get; set; }

	public bool IsDeleted { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

	public DateTime? UpdatedDate { get; set; }
}

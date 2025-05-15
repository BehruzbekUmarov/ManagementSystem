namespace InnerSystem.Api.DTOs.Comment;

public class UpdateCommentDto
{
	public string Content { get; set; } = null!;

	public Guid PostId { get; set; }
}

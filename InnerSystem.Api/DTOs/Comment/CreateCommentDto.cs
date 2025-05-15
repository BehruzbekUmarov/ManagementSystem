namespace InnerSystem.Api.DTOs.Comment;

public class CreateCommentDto
{
	public string Content { get; set; } = null!;

	public Guid PostId { get; set; }
}

namespace InnerSystem.Api.DTOs.Comment;

public class CommentForPostDto
{
	public Guid Id { get; set; }
	public string Content { get; set; } = null!;
	public Guid AuthorId { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}

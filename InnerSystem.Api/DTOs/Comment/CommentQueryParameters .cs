namespace InnerSystem.Api.DTOs.Comment;

public class CommentQueryParameters
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public Guid? PostId { get; set; }
	public Guid? AuthorId { get; set; }

	public string? SortBy { get; set; } = "CreatedDate";
	public bool IsDescending { get; set; } = true;
}

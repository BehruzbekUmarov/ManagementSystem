using InnerSystem.Api.Enums;

namespace InnerSystem.Api.DTOs.Post;

public class PostQueryParameters
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public string? Title { get; set; }
	public PostStatus? Status { get; set; }

	public string? SortBy { get; set; } = "CreatedDate";
	public bool IsDescending { get; set; } = true;
}

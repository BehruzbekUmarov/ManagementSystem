using InnerSystem.Api.Enums;

namespace InnerSystem.Api.DTOs.Post;

public class CreatePostDto
{
	public string Title { get; set; } = null!;
	public string Body { get; set; } = null!;
	public string? Image { get; set; }
}

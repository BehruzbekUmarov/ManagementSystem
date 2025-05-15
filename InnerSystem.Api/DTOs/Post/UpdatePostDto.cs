namespace InnerSystem.Api.DTOs.Post;

public class UpdatePostDto
{
	public string Title { get; set; } = null!;
	public string Body { get; set; } = null!;
	public string? Image { get; set; }
}

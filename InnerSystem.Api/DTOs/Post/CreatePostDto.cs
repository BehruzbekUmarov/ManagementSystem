using InnerSystem.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Api.DTOs.Post;

public class CreatePostDto
{
	[Required(ErrorMessage = "Title is required.")]
	[StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters.")]
	public string Title { get; set; } = null!;

	[Required(ErrorMessage = "Body is required.")]
	[StringLength(50000, MinimumLength = 10, ErrorMessage = "Body must be between 10 and 50000 characters.")]
	public string Body { get; set; } = null!;
	public IFormFile? Image { get; set; } = default!;
}

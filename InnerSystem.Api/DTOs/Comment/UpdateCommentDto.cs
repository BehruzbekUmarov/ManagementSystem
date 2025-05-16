using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Api.DTOs.Comment;

public class UpdateCommentDto
{
	[Required]
	[StringLength(10000, MinimumLength = 1, ErrorMessage = "Content must be between 1 and 1000 characters.")]
	public string Content { get; set; } = null!;

	[Required(ErrorMessage = "PostId is required.")]
	public Guid PostId { get; set; }
}

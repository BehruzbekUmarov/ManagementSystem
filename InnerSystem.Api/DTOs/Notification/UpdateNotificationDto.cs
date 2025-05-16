using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Api.DTOs.Notification;

public class UpdateNotificationDto
{
	[Required(ErrorMessage = "Title is required.")]
	[StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters.")]
	public string Title { get; set; } = null!;

	[Required(ErrorMessage = "Description is required.")]
	[StringLength(10000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 10000 characters.")]
	public string Description { get; set; } = null!;
}

namespace InnerSystem.Api.DTOs.Notification;

public class NotificationDto
{
	public Guid Id { get; set; }
	
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;
	public bool IsRead { get; set; } = false;

	public Guid UserId { get; set; }

	public bool IsDeleted { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

	public DateTime? UpdatedDate { get; set; }
}

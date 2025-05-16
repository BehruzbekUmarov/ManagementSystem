namespace InnerSystem.Api.DTOs.Notification;

public class NotificationQueryParameters
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;

	public Guid? UserId { get; set; }

	public string? SortBy { get; set; } = "CreatedDate";
	public bool IsDescending { get; set; } = true;

	public bool? IsRead { get; set; }
}

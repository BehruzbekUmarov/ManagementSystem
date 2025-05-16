using InnerSystem.Api.DTOs.Notification;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Repositories.Interfaces;

public interface INotificationRepository : IGenericRepository<Notification>
{
	Task<IEnumerable<Notification>> GetUnreadByUserAsync(Guid userId);
	Task<IEnumerable<Notification>> GetFilteredNotificationsAsync(NotificationQueryParameters parameters);
}

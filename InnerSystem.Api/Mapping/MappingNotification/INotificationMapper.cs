using InnerSystem.Api.DTOs.Notification;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Mapping.MappingNotification;

public interface INotificationMapper
{
	public Notification MapToEntity(CreateNotificationDto dto);
	public void MapToExistingEntity(UpdateNotificationDto dto, Notification entity);
	public NotificationDto MapToDto(Notification entity);
	public List<NotificationDto> MapToDtoList(IEnumerable<Notification> entities);
}

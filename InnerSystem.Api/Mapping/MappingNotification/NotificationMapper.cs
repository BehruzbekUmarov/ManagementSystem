using InnerSystem.Api.DTOs.Notification;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Mapping.MappingNotification;

public class NotificationMapper : INotificationMapper
{
	public Notification MapToEntity(CreateNotificationDto dto)
	{
		return new Notification
		{
			Title = dto.Title,
			Description = dto.Description
		};
	}

	// Update DTO to Existing Entity
	public void MapToExistingEntity(UpdateNotificationDto dto, Notification entity)
	{
		entity.Title = dto.Title;
		entity.Description = dto.Description;
	}

	// Entity to DTO
	public NotificationDto MapToDto(Notification entity)
	{
		return new NotificationDto
		{
			Id = entity.Id,
			Title = entity.Title,
			Description = entity.Description,
			IsRead = entity.IsRead,
			UserId = entity.UserId,
			IsDeleted = entity.IsDeleted,
			CreatedDate = entity.CreatedDate,
			UpdatedDate = entity.UpdatedDate
		};
	}

	// List of Entities to List of DTOs
	public List<NotificationDto> MapToDtoList(IEnumerable<Notification> entities)
	{
		return entities.Select(MapToDto).ToList();
	}
}

using InnerSystem.Api.DTOs.Notification;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Mapping.MappingNotification;
using InnerSystem.Api.Repositories.Interfaces;
using InnerSystem.Identity.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

/// <summary>
/// Controller for managing user notifications. Includes operations to create, update, delete, and retrieve notifications.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
	private readonly INotificationRepository _notificationRepository;
	private readonly INotificationMapper _notificationMapper;
	private readonly IEnvironmentAccessor _environmentAccessor;

	public NotificationController(INotificationRepository notificationRepository, INotificationMapper notificationMapper, IEnvironmentAccessor environmentAccessor)
	{
		_notificationRepository = notificationRepository;
		_notificationMapper = notificationMapper;
		_environmentAccessor = environmentAccessor;
	}

	/// <summary>
	/// Retrieves a notification by its ID.
	/// </summary>
	[HttpGet("{id}")]
	[ProducesResponseType(typeof(NotificationDto), 200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	public async Task<ActionResult> GetById(Guid id)
	{
		try
		{
			var notification = await _notificationRepository.GetByIdAsync(id);
			if (notification == null)
				return NotFound();

			var toDto = _notificationMapper.MapToDto(notification);

			return Ok(toDto);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Retrieves all notifications with optional filtering.
	/// </summary>
	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<NotificationDto>), 200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	public async Task<ActionResult> GetAll([FromQuery] NotificationQueryParameters parameters)
	{
		try
		{
			var notifications = await _notificationRepository.GetFilteredNotificationsAsync(parameters);
			if (notifications == null) return NotFound();

			var toDto = _notificationMapper.MapToDtoList(notifications);

			return Ok(toDto);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Retrieves all unread notifications for a specific user.
	/// </summary>
	[Authorize(Roles = "Admin, Manager")]
	[HttpGet("unread/{userId}")]
	[ProducesResponseType(typeof(IEnumerable<NotificationDto>), 200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	public async Task<ActionResult> GetUnreadByUser(Guid userId)
	{
		try
		{
			var unread = await _notificationRepository.GetUnreadByUserAsync(userId);
			if (unread == null) return NotFound();

			var toDto = _notificationMapper.MapToDtoList(unread);
			return Ok(toDto);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Creates a new notification.
	/// </summary>
	[Authorize(Roles = "Admin, Manager")]
	[HttpPost]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	[ProducesResponseType(500)]
	public async Task<ActionResult> Create([FromBody] CreateNotificationDto notification)
	{
		try
		{
			var toEntity = _notificationMapper.MapToEntity(notification);

			if (Guid.TryParse(_environmentAccessor.UserId(), out var userId))
			{
				toEntity.UserId = userId;
			}
			else
			{
				return BadRequest("Invalid user ID in context.");
			}

			await _notificationRepository.AddAsync(toEntity);
			var saved = await _notificationRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not save notification.");

			return Ok(saved);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Updates an existing notification.
	/// </summary>
	[Authorize(Roles = "Admin, Manager")]
	[HttpPut("{id}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateNotificationDto updatedNotification)
	{
		try
		{
			var existing = await _notificationRepository.GetByIdAsync(id);
			if (existing == null) return NotFound();

			_notificationMapper.MapToExistingEntity(updatedNotification, existing);

			_notificationRepository.Update(existing);
			var saved = await _notificationRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not update notification.");

			return Ok();
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Deletes a notification by ID.
	/// </summary>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	public async Task<ActionResult> Delete(Guid id)
	{
		try
		{
			var notification = await _notificationRepository.GetByIdAsync(id);
			if (notification == null) return NotFound();

			_notificationRepository.Delete(notification);
			var saved = await _notificationRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not delete notification.");

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}
}

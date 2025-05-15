using InnerSystem.Api.DTOs.Notification;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
	private readonly INotificationRepository _notificationRepository;

	public NotificationController(INotificationRepository notificationRepository)
	{
		_notificationRepository = notificationRepository;
	}

	[HttpGet("{id}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		try
		{
			var notification = await _notificationRepository.GetByIdAsync(id);
			if (notification == null)
				return NotFound();

			return Ok(notification);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet]
	public async Task<ActionResult> GetAll()
	{
		try
		{
			var notifications = await _notificationRepository.GetAllAsync();
			return Ok(notifications);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet("unread/{userId}")]
	public async Task<ActionResult> GetUnreadByUser(Guid userId)
	{
		try
		{
			var unread = await _notificationRepository.GetUnreadByUserAsync(userId);
			return Ok(unread);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateNotificationDto notification)
	{
		try
		{
			var notificationEntity = new Notification
			{
				Title = notification.Title,
				Description = notification.Description
			};

			await _notificationRepository.AddAsync(notificationEntity);
			var saved = await _notificationRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not save notification.");

			return Ok(saved);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateNotificationDto updatedNotification)
	{
		try
		{
			var existing = await _notificationRepository.GetByIdAsync(id);
			if (existing == null) return NotFound();

			existing.Description = updatedNotification.Description;
			existing.Title = updatedNotification.Title;

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

	[HttpDelete("{id}")]
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

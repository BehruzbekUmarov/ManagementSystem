using InnerSystem.Api.Data;
using InnerSystem.Api.DTOs.Notification;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnerSystem.Api.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
	private readonly AppDbContext _context;

	public NotificationRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(Guid userId)
	{
		return await _context.Notifications
			.Where(n => n.UserId == userId)
			.OrderByDescending(n => n.CreatedDate)
			.ToListAsync();
	}

	public async Task<IEnumerable<Notification>> GetUnreadByUserAsync(Guid userId)
	{
		return await _context.Notifications
			.Where(n => n.UserId == userId && !n.IsRead)
			.OrderByDescending(n => n.CreatedDate)
			.ToListAsync();
	}

	public async Task<IEnumerable<Notification>> GetFilteredNotificationsAsync(NotificationQueryParameters parameters)
	{
		var query = _context.Notifications.AsQueryable();

		// Filtering
		if (parameters.UserId.HasValue)
			query = query.Where(n => n.UserId == parameters.UserId.Value);

		if (parameters.IsRead.HasValue)
			query = query.Where(n => n.IsRead == parameters.IsRead.Value);

		// Sorting
		query = parameters.SortBy?.ToLower() switch
		{
			"title" => parameters.IsDescending ? query.OrderByDescending(n => n.Title) : query.OrderBy(n => n.Title),
			"isread" => parameters.IsDescending ? query.OrderByDescending(n => n.IsRead) : query.OrderBy(n => n.IsRead),
			_ => parameters.IsDescending ? query.OrderByDescending(n => n.CreatedDate) : query.OrderBy(n => n.CreatedDate)
		};

		// Pagination
		int skip = (parameters.PageNumber - 1) * parameters.PageSize;
		query = query.Skip(skip).Take(parameters.PageSize);

		return await query.ToListAsync();
	}
}


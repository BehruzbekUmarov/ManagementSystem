using InnerSystem.Api.Data;
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
}


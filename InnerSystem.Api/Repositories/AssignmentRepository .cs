using InnerSystem.Api.Data;
using InnerSystem.Api.DTOs.Assignment;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnerSystem.Api.Repositories;

public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
{
	private readonly AppDbContext _context;

	public AssignmentRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Assignment>> GetAssignmentsByUserAsync(Guid userId)
	{
		return await _context.Assignments
		.Where(a => a.AssignedToId == userId)
			.ToListAsync();
	}

	public async Task<IEnumerable<Assignment>> GetAssignmentsWithStatusAsync(InnerSystem.Api.Enums.TaskStatus status)
	{
		return await _context.Assignments
			.Where(a => a.Status == status)
			.ToListAsync();
	}

	public async Task<IEnumerable<Assignment>> GetAllPaginatedAsync(AssignmentQueryParameters parameters)
	{
		var query = _context.Assignments.AsQueryable();

		// Filtering
		if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
		{
			query = query.Where(a => a.Title.Contains(parameters.SearchTerm) || a.Description.Contains(parameters.SearchTerm));
		}
		else
		{
			query = query.OrderBy(a => a.Title); // default sort
		}

		// Pagination
		query = query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
					 .Take(parameters.PageSize);

		return await query.ToListAsync();
	}
}

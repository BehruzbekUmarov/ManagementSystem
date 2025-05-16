using InnerSystem.Api.Data;
using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnerSystem.Api.Repositories;

public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
	private readonly AppDbContext _context;

	public CommentRepository(AppDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Comment>> GetCommentsByAuthorAsync(Guid authorId)
	{
		return await _context.Comments
			.Where(x => x.AuthorId == authorId)
			.OrderByDescending(x => x.CreatedDate)
			.ToListAsync();
	}

	public async Task<IEnumerable<Comment>> GetCommentsByPostAsync(Guid postId)
	{
		return await _context.Comments
			.Where(x => x.PostId == postId)
			.OrderByDescending(x => x.CreatedDate)
			.ToListAsync();
	}

	public async Task<IEnumerable<Comment>> GetFilteredCommentsAsync(CommentQueryParameters parameters)
	{
		var query = _context.Comments.AsQueryable();

		// Filtering
		if (parameters.PostId.HasValue)
			query = query.Where(c => c.PostId == parameters.PostId.Value);

		if (parameters.AuthorId.HasValue)
			query = query.Where(c => c.AuthorId == parameters.AuthorId.Value);

		// Sorting
		query = parameters.SortBy?.ToLower() switch
		{
			"content" => parameters.IsDescending ? query.OrderByDescending(c => c.Content) : query.OrderBy(c => c.Content),
			"postid" => parameters.IsDescending ? query.OrderByDescending(c => c.PostId) : query.OrderBy(c => c.PostId),
			_ => parameters.IsDescending ? query.OrderByDescending(c => c.CreatedDate) : query.OrderBy(c => c.CreatedDate)
		};

		// Pagination
		int skip = (parameters.PageNumber - 1) * parameters.PageSize;
		query = query.Skip(skip).Take(parameters.PageSize);

		return await query.ToListAsync();
	}
}

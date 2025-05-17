using InnerSystem.Api.Data;
using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnerSystem.Api.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
	private readonly AppDbContext _dbContext;
	public PostRepository(AppDbContext context) : base(context)
	{
		_dbContext = context;
	}

	public async Task<IEnumerable<Post>> GetPostsWithCommentsAsync()
	{
		return await _dbContext.Set<Post>()
			.Include(p => p.Comments)
			.ToListAsync();
	}

	public async Task<Post?> GetPostWithAuthorAsync(Guid postId)
	{
		return await _dbContext.Set<Post>()
			.FirstOrDefaultAsync(p => p.Id == postId);
	}

	public async Task<IEnumerable<Post>> GetFilteredPostsAsync(PostQueryParameters parameters)
	{
		var query = _dbContext.Posts.Include(p => p.Comments).AsQueryable();

		// Filtering
		if (!string.IsNullOrWhiteSpace(parameters.Title))
		{
			query = query.Where(p => p.Title.Contains(parameters.Title));
		}

		if (parameters.Status.HasValue)
		{
			query = query.Where(p => p.Status == parameters.Status.Value);
		}

		// Sorting
		query = parameters.SortBy?.ToLower() switch
		{
			"title" => parameters.IsDescending ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
			"status" => parameters.IsDescending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
			_ => parameters.IsDescending ? query.OrderByDescending(p => p.CreatedDate) : query.OrderBy(p => p.CreatedDate)
		};

		// Pagination
		int skip = (parameters.PageNumber - 1) * parameters.PageSize;
		query = query.Skip(skip).Take(parameters.PageSize);

		return await query.ToListAsync();
	}
}

using InnerSystem.Api.Data;
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
}

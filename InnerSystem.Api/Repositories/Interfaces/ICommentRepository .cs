using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Repositories.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment>
{
	Task<IEnumerable<Comment>> GetCommentsByPostAsync(Guid postId);
	Task<IEnumerable<Comment>> GetCommentsByAuthorAsync(Guid authorId);
}

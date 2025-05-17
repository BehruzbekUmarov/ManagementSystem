using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Repositories.Interfaces;

public interface IPostRepository : IGenericRepository<Post>
{
	Task<Post?> GetPostWithAuthorAsync(Guid postId);
	Task<IEnumerable<Post>> GetFilteredPostsAsync(PostQueryParameters parameters);
}

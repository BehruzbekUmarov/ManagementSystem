using InnerSystem.Api.DTOs.Notification;
using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Enums;

namespace InnerSystem.Api.Mapping.MappingPost;

public class PostMapper : IPostMapper
{
	private readonly IHttpContextAccessor _contextAccessor;

	public PostMapper(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	public PostDto MapToDto(Post post)
	{
		var request = _contextAccessor.HttpContext?.Request;

		var imageUrl = post.Image;
		if (request != null && !string.IsNullOrEmpty(post.Image))
		{
			imageUrl = $"{request.Scheme}://{request.Host}{post.Image}";
		}

		return new PostDto
		{
			Id = post.Id,
			Title = post.Title,
			Body = post.Body,
			Image = imageUrl,
			Status = post.Status,
			AuthorId = post.AuthorId,
			IsDeleted = post.IsDeleted,
			CreatedDate = post.CreatedDate,
			UpdatedDate = post.UpdatedDate
		};
	}

	public Post MapToEntity(CreatePostDto dto)
	{
		return new Post
		{
			Title = dto.Title,
			Body = dto.Body,
			Status = PostStatus.Draft
		};
	}

	public void MapToEntity(UpdatePostDto dto, Post post)
	{
		post.Title = dto.Title;
		post.Body = dto.Body;
	}

	public List<PostDto> MapToDtoList(IEnumerable<Post> entities)
	{
		return entities.Select(MapToDto).ToList();
	}
}
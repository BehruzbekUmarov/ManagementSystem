using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.DTOs.Notification;
using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Enums;
using InnerSystem.Api.Mapping.MappingComment;

namespace InnerSystem.Api.Mapping.MappingPost;

public class PostMapper : IPostMapper
{
	private readonly IHttpContextAccessor _contextAccessor;
	private readonly ICommentMapper _commentMapper;

	public PostMapper(IHttpContextAccessor contextAccessor, ICommentMapper commentMapper)
	{
		_contextAccessor = contextAccessor;
		_commentMapper = commentMapper;
	}

	public PostDto MapToDto(Post post)
	{
		var request = _contextAccessor.HttpContext?.Request;

		var imageUrl = post.Image;
		if (request != null && !string.IsNullOrEmpty(post.Image))
		{
			imageUrl = $"{request.Scheme}://{request.Host}{post.Image}";
		}

		var commentDtos = post.Comments?
			.Where(c => !c.IsDeleted)
			.Select(c => new CommentForPostDto
			{
				Id = c.Id,
				Content = c.Content,
				AuthorId = c.AuthorId,
				IsDeleted = c.IsDeleted,
				CreatedDate = c.CreatedDate,
				UpdatedDate = c.UpdatedDate
			}).ToList() ?? new List<CommentForPostDto>();

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
			UpdatedDate = post.UpdatedDate,
			Comments = commentDtos
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
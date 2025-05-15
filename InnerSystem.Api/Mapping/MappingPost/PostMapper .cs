using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Enums;

namespace InnerSystem.Api.Mapping.MappingPost;

public class PostMapper : IPostMapper
{
	public PostDto MapToDto(Post post)
	{
		return new PostDto
		{
			Id = post.Id,
			Title = post.Title,
			Body = post.Body,
			Image = post.Image,
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
			Image = dto.Image,
			Status = PostStatus.Draft
		};
	}

	public void MapToEntity(UpdatePostDto dto, Post post)
	{
		post.Title = dto.Title;
		post.Body = dto.Body;
		post.Image = dto.Image;
	}
}
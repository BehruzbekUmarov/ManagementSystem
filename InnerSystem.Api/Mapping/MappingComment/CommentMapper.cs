using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;
using Microsoft.Extensions.Hosting;

namespace InnerSystem.Api.Mapping.MappingComment;

public class CommentMapper : ICommentMapper
{
	private readonly IHttpContextAccessor _contextAccessor;

	public CommentMapper(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}



	public CommentDto MapToDto(Comment comment)
	{
		var request = _contextAccessor.HttpContext?.Request;

		var imageUrl = comment.Post.Image;
		if (request != null && !string.IsNullOrEmpty(comment.Post.Image))
		{
			imageUrl = $"{request.Scheme}://{request.Host}{comment.Post.Image}";
		}

		return new CommentDto
		{
			Id = comment.Id,
			Content = comment.Content,
			AuthorId = comment.AuthorId,
			IsDeleted = comment.IsDeleted,
			CreatedDate = comment.CreatedDate,
			UpdatedDate = comment.UpdatedDate,
			Post = comment.Post == null ? null : new ShallowPostDto
			{
				Id = comment.Post.Id,
				Title = comment.Post.Title,
				Body = comment.Post.Body,
				Image = imageUrl,
				Status = comment.Post.Status,
				AuthorId = comment.Post.AuthorId,
				IsDeleted = comment.Post.IsDeleted,
				CreatedDate = comment.Post.CreatedDate,
				UpdatedDate = comment.Post.UpdatedDate
			}
		};
	}

	public List<CommentDto> MapToDtoList(IEnumerable<Comment> comments)
	{
		return comments.Select(MapToDto).ToList();
	}

	public Comment MapToEntity(CreateCommentDto dto)
	{
		return new Comment
		{
			Content = dto.Content,
			PostId = dto.PostId
		};
	}

	public void MapToExistingEntity(UpdateCommentDto dto, Comment entity)
	{
		entity.Content = dto.Content;
		entity.PostId = dto.PostId;
	}
}

using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Mapping.MappingComment;

public class CommentMapper : ICommentMapper
{
	public CommentDto MapToDto(Comment comment)
	{
		return new CommentDto
		{
			Id = comment.Id,
			Content = comment.Content,
			PostId = comment.PostId,
			AuthorId = comment.AuthorId,
			IsDeleted = comment.IsDeleted,
		};
	}

	public Comment MapToEntity(CreateCommentDto dto)
	{
		return new Comment
		{
			Content = dto.Content,
			PostId = dto.PostId
		};
	}

	public void MapToEntity(UpdateCommentDto dto, Comment comment)
	{
		comment.Content = dto.Content;
		comment.PostId = dto.PostId;
	}
}

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
			CreatedDate = comment.CreatedDate,
			UpdatedDate = comment.UpdatedDate
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

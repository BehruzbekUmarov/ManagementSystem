using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Mapping.MappingComment;

public interface ICommentMapper
{
	CommentDto MapToDto(Comment comment);
	Comment MapToEntity(CreateCommentDto dto);
	void MapToEntity(UpdateCommentDto dto, Comment comment);
}

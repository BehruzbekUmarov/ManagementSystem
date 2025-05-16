using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Mapping.MappingComment;

public interface ICommentMapper
{
	CommentDto MapToDto(Comment comment);
	Comment MapToEntity(CreateCommentDto dto);
	void MapToExistingEntity(UpdateCommentDto dto, Comment comment);
	List<CommentDto> MapToDtoList(IEnumerable<Comment> comments);
}

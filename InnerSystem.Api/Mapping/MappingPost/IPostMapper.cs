using InnerSystem.Api.DTOs.Assignment;
using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;

namespace InnerSystem.Api.Mapping.MappingPost;

public interface IPostMapper
{
	PostDto MapToDto(Post post);
	Post MapToEntity(CreatePostDto dto);
	void MapToEntity(UpdatePostDto dto, Post post);
	List<PostDto> MapToDtoList(IEnumerable<Post> comments);
}

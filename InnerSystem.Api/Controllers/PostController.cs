using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Mapping.MappingPost;
using InnerSystem.Api.Repositories.Interfaces;
using InnerSystem.Identity.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostController : ControllerBase
{
	private readonly IPostRepository _postRepository;
	private readonly IPostMapper _postMapper;
	private readonly IEnvironmentAccessor _environmentAccessor;

	public PostController(IPostRepository postRepository, IPostMapper postMapper, IEnvironmentAccessor environmentAccessor)
	{
		_postRepository = postRepository;
		_postMapper = postMapper;
		_environmentAccessor = environmentAccessor;
	}

	[HttpGet("{id}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		try
		{
			var post = await _postRepository.GetByIdAsync(id);
			if (post == null)
				return NotFound();

			var toDto = _postMapper.MapToDto(post);

			return Ok(toDto);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<PostDto>>> GetAll([FromQuery] PostQueryParameters parameters)
	{
		try
		{
			var posts = await _postRepository.GetFilteredPostsAsync(parameters);
			var result = posts.Select(_postMapper.MapToDto);
			return Ok(result);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet("with-comments")]
	public async Task<ActionResult> GetWithComments()
	{
		try
		{
			var posts = await _postRepository.GetPostsWithCommentsAsync();
			return Ok(posts);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet("with-author/{postId}")]
	public async Task<ActionResult> GetWithAuthor(Guid postId)
	{
		try
		{
			var post = await _postRepository.GetPostWithAuthorAsync(postId);
			if (post == null)
				return NotFound();

			return Ok(post);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreatePostDto post)
	{
		try
		{
			var entity = _postMapper.MapToEntity(post);
			if (Guid.TryParse(_environmentAccessor.UserId(), out var userId))
			{
				entity.AuthorId = userId;
			}
			else
			{
				return BadRequest("Invalid user ID in context.");
			}

			await _postRepository.AddAsync(entity);
			var saved = await _postRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not save post.");

			return Ok(saved);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdatePostDto updatedPost)
	{
		try
		{
			var existing = await _postRepository.GetByIdAsync(id);
			if (existing == null) return NotFound();

			_postMapper.MapToEntity(updatedPost, existing);

			_postRepository.Update(existing);
			var saved = await _postRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not update post.");

			return Ok();
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		try
		{
			var post = await _postRepository.GetByIdAsync(id);
			if (post == null) return NotFound();

			_postRepository.Delete(post);
			var saved = await _postRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not delete post.");

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}
}

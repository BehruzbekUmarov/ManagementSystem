using InnerSystem.Api.DTOs.Post;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Helper;
using InnerSystem.Api.Mapping.MappingPost;
using InnerSystem.Api.Repositories.Interfaces;
using InnerSystem.Identity.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

/// <summary>
/// Manages blog posts including creation, retrieval, updating, and deletion operations.
/// Only authorized users can perform certain actions based on their roles.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostController : ControllerBase
{
	private readonly IPostRepository _postRepository;
	private readonly IPostMapper _postMapper;
	private readonly IEnvironmentAccessor _environmentAccessor;
	private readonly IPostImageSaveHelper _postImageSaveHelper;

	public PostController(IPostRepository postRepository, IPostMapper postMapper, IEnvironmentAccessor environmentAccessor, IPostImageSaveHelper postImageSaveHelper)
	{
		_postRepository = postRepository;
		_postMapper = postMapper;
		_environmentAccessor = environmentAccessor;
		_postImageSaveHelper = postImageSaveHelper;
	}

	/// <summary>
	/// Retrieves a post by its unique identifier.
	/// </summary>
	/// <param name="id">The GUID of the post.</param>
	/// <returns>The post details if found; otherwise, 404 Not Found.</returns>
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

	/// <summary>
	/// Retrieves all posts with optional filtering and pagination.
	/// </summary>
	/// <param name="parameters">Query parameters for filtering and pagination.</param>
	/// <returns>A list of posts that match the query parameters.</returns>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<PostDto>>> GetAll([FromQuery] PostQueryParameters parameters)
	{
		try
		{
			var posts = await _postRepository.GetFilteredPostsAsync(parameters);
			var result = _postMapper.MapToDtoList(posts);
			return Ok(result);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Retrieves all posts including their associated comments.
	/// </summary>
	/// <returns>A list of posts with embedded comments.</returns>
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

	/// <summary>
	/// Retrieves a specific post along with its author's information.
	/// Accessible only to users in the Admin or Manager roles.
	/// </summary>
	/// <param name="postId">The GUID of the post.</param>
	/// <returns>The post with author details if found; otherwise, 404 Not Found.</returns>
	[Authorize(Roles = "Admin, Manager")]
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

	/// <summary>
	/// Creates a new post with image upload support.
	/// Accessible only to users in the Admin or Manager roles.
	/// </summary>
	/// <param name="post">The post data to create, including an image file.</param>
	/// <returns>200 OK if the post is created successfully; otherwise, appropriate error responses.</returns>
	[Authorize(Roles = "Admin, Manager")]
	[HttpPost]
	public async Task<ActionResult> Create([FromForm] CreatePostDto post)
	{
		try
		{
			var imagePath = await _postImageSaveHelper.SaveImageAsync(post.Image);
			var entity = _postMapper.MapToEntity(post);
			entity.Image = imagePath;

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

	/// <summary>
	/// Updates an existing post and optionally replaces its image.
	/// Accessible only to users in the Admin or Manager roles.
	/// </summary>
	/// <param name="id">The GUID of the post to update.</param>
	/// <param name="updatedPost">The updated post data and image file.</param>
	/// <returns>200 OK if updated successfully; otherwise, appropriate error responses.</returns>
	[Authorize(Roles = "Admin, Manager")]
	[HttpPut("{id}")]
	public async Task<ActionResult> Update(Guid id, [FromForm] UpdatePostDto updatedPost)
	{
		try
		{
			var existing = await _postRepository.GetByIdAsync(id);
			if (existing == null) return NotFound();

			var imagePath = await _postImageSaveHelper.SaveImageAsync(updatedPost.Image);
			existing.Image = imagePath;

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

	/// <summary>
	/// Deletes a post by its identifier.
	/// Only accessible by users in the Admin role.
	/// </summary>
	/// <param name="id">The GUID of the post to delete.</param>
	/// <returns>204 No Content if deleted; otherwise, appropriate error responses.</returns>
	[Authorize(Roles = "Admin")]
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

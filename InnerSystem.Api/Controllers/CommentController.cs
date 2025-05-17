using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Mapping.MappingComment;
using InnerSystem.Api.Repositories.Interfaces;
using InnerSystem.Identity.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

/// <summary>
/// Manages operations related to user comments.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentController : ControllerBase
{
	private readonly ICommentRepository _commentRepository;
	private readonly ICommentMapper _commentMapper;
	private readonly IEnvironmentAccessor _environmentAccessor;

	public CommentController(ICommentRepository commentRepository, ICommentMapper commentMapper, IEnvironmentAccessor environmentAccessor)
	{
		_commentRepository = commentRepository;
		_commentMapper = commentMapper;
		_environmentAccessor = environmentAccessor;
	}

	/// <summary>
	/// Gets a comment by its unique identifier.
	/// </summary>
	/// <param name="id">Comment ID.</param>
	/// <returns>Comment details.</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		try
		{
			var comment = await _commentRepository.GetByIdAsync(id, x => ((Comment)(object)x).Post);
			if (comment == null)
				return NotFound();

			var toDto = _commentMapper.MapToDto(comment);

			return Ok(toDto);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Gets all comments authored by a specific user.
	/// </summary>
	/// <param name="authorId">Author ID.</param>
	/// <returns>List of comments.</returns>
	[HttpGet("author/{authorId}")]
	public async Task<ActionResult> GetByAuthor(Guid authorId)
	{
		try
		{
			var comments = await _commentRepository.GetCommentsByAuthorAsync(authorId);
			if (comments == null) return NotFound();

			var toDto = _commentMapper.MapToDtoList(comments);

			return Ok(comments);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Retrieves all comments with optional filters.
	/// </summary>
	/// <param name="parameters">Filter and pagination parameters.</param>
	/// <returns>Filtered list of comments.</returns>
	[HttpGet]
	public async Task<ActionResult<IEnumerable<CommentDto>>> GetAll([FromQuery] CommentQueryParameters parameters)
	{
		try
		{
			var comments = await _commentRepository.GetFilteredCommentsAsync(parameters);
			if (comments == null) return NotFound();

			var toDto = _commentMapper.MapToDtoList(comments);

			return Ok(toDto);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Gets all comments under a specific post. Accessible by Admin and Manager roles.
	/// </summary>
	/// <param name="postId">Post ID.</param>
	/// <returns>List of comments.</returns>
	[Authorize(Roles = "Admin, Manager")]
	[HttpGet("post/{postId}")]
	public async Task<ActionResult> GetByPost(Guid postId)
	{
		try
		{
			var comments = await _commentRepository.GetCommentsByPostAsync(postId);
			return Ok(comments);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Creates a new comment.
	/// </summary>
	/// <param name="comment">Comment creation data.</param>
	/// <returns>Success result.</returns>
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateCommentDto comment)
	{
		try
		{
			var entity = _commentMapper.MapToEntity(comment);

			if (Guid.TryParse(_environmentAccessor.UserId(), out var userId))
			{
				entity.AuthorId = userId;
			}
			else
			{
				return BadRequest("Invalid user ID in context.");
			}

			await _commentRepository.AddAsync(entity);
			var saved = await _commentRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not save comment.");

			return Ok(saved);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Updates an existing comment. Accessible by Admin and Manager roles.
	/// </summary>
	/// <param name="id">Comment ID.</param>
	/// <param name="updatedComment">Updated comment data.</param>
	/// <returns>Success or error message.</returns>
	[Authorize(Roles = "Admin, Manager")]
	[HttpPut("{id}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCommentDto updatedComment)
	{
		try
		{
			var existing = await _commentRepository.GetByIdAsync(id);
			if (existing == null) return NotFound();

			existing.Content = updatedComment.Content;
			existing.PostId = updatedComment.PostId;

			_commentRepository.Update(existing);
			var saved = await _commentRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not update comment.");

			return Ok();
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	/// <summary>
	/// Deletes a comment. Accessible by Admin role only.
	/// </summary>
	/// <param name="id">Comment ID.</param>
	/// <returns>Success or error result.</returns>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		try
		{
			var comment = await _commentRepository.GetByIdAsync(id);
			if (comment == null) return NotFound();

			_commentRepository.Delete(comment);
			var saved = await _commentRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not delete comment.");

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}
}

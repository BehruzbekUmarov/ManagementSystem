using InnerSystem.Api.DTOs.Comment;
using InnerSystem.Api.Entities;
using InnerSystem.Api.Mapping.MappingComment;
using InnerSystem.Api.Repositories.Interfaces;
using InnerSystem.Identity.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
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

	[HttpGet("{id}")]
	public async Task<ActionResult> GetById(Guid id)
	{
		try
		{
			var comment = await _commentRepository.GetByIdAsync(id);
			if (comment == null)
				return NotFound();

			return Ok(comment);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet("author/{authorId}")]
	public async Task<ActionResult> GetByAuthor(Guid authorId)
	{
		try
		{
			var comments = await _commentRepository.GetCommentsByAuthorAsync(authorId);
			return Ok(comments);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet]
	public async Task<ActionResult> GetAll()
	{
		try
		{
			var comments = await _commentRepository.GetAllAsync();
			return Ok(comments);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

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

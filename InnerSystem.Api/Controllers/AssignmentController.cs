using InnerSystem.Api.DTOs.Assignment;
using InnerSystem.Api.Mapping.MappingAssignment;
using InnerSystem.Api.Repositories.Interfaces;
using InnerSystem.Identity.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AssignmentController : ControllerBase
{
	private readonly IAssignmentRepository _assignmentRepository;
	private readonly IEnvironmentAccessor _environmentAccessor;
	private readonly IAssignmentMapper _assignmentMapper;

	public AssignmentController(IAssignmentRepository assignmentRepository, IAssignmentMapper assignmentMapper, IEnvironmentAccessor environmentAccessor)
	{
		_assignmentRepository = assignmentRepository;
		_assignmentMapper = assignmentMapper;
		_environmentAccessor = environmentAccessor;
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<AssignmentDto>> GetById(Guid id)
	{
		try
		{
			var assignment = await _assignmentRepository.GetByIdAsync(id);
			if (assignment == null)
				return NotFound();

			var assignDto = _assignmentMapper.MapToDto(assignment);

			return Ok(assignDto);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAll([FromQuery] AssignmentQueryParameters parameters)
	{
		try
		{
			var assignments = await _assignmentRepository.GetAllPaginatedAsync(parameters);
			if (assignments == null || !assignments.Any())
				return NotFound();

			var result = assignments.Select(_assignmentMapper.MapToDto).ToList();
			return Ok(result);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet("user/{userId}")]
	public async Task<ActionResult> GetByUser(Guid userId)
	{
		try
		{
			var assignments = await _assignmentRepository.GetAssignmentsByUserAsync(userId);
			return Ok(assignments);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpGet("status/{status}")]
	public async Task<ActionResult> GetByStatus(Enums.TaskStatus status)
	{
		try
		{
			var assignments = await _assignmentRepository.GetAssignmentsWithStatusAsync(status);
			return Ok(assignments);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateAssignmentDto assignmentDto)
	{
		try
		{
			var entity = _assignmentMapper.MapToEntity(assignmentDto);

			if (Guid.TryParse(_environmentAccessor.UserId(), out var userId))
			{
				entity.CreatedById = userId;
			}
			else
			{
				return BadRequest("Invalid user ID in context.");
			}

			await _assignmentRepository.AddAsync(entity);
			var saved = await _assignmentRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not save assignment.");

			return Ok(saved);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}

	[HttpPut("{id}")]
	public async Task<ActionResult> Update(Guid id, [FromBody] UpdateAssignmentDto updatedAssignment)
	{
		try
		{
			var existing = await _assignmentRepository.GetByIdAsync(id);
			if (existing == null) return NotFound();

			_assignmentMapper.MapToEntity(updatedAssignment, existing);

			_assignmentRepository.Update(existing);
			var saved = await _assignmentRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not update assignment.");

			return Ok(saved);
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
			var assignment = await _assignmentRepository.GetByIdAsync(id);
			if (assignment == null) return NotFound();

			_assignmentRepository.Delete(assignment);
			var saved = await _assignmentRepository.SaveChangesAsync();
			if (!saved) return StatusCode(500, "Could not delete assignment.");

			return NoContent();
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Internal server error: {ex.Message}");
		}
	}
}

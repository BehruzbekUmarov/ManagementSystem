using InnerSystem.Api.DTOs.Assignment;
using InnerSystem.Api.Mapping.MappingAssignment;
using InnerSystem.Api.Repositories.Interfaces;
using InnerSystem.Identity.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

/// <summary>
/// Controller for managing assignments. Supports creating, updating, deleting,
/// and retrieving assignments by various filters.
/// </summary>
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

	/// <summary>
	/// Retrieves an assignment by its ID.
	/// </summary>
	/// <param name="id">The unique identifier of the assignment.</param>
	/// <returns>The assignment if found; otherwise, 404 Not Found.</returns>
	/// <response code="200">Returns the assignment.</response>
	/// <response code="404">If the assignment is not found.</response>
	/// <response code="500">If an internal error occurs.</response>
	[HttpGet("{id}")]
	[ProducesResponseType(typeof(AssignmentDto), 200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
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

	/// <summary>
	/// Retrieves a paginated list of assignments using query parameters.
	/// </summary>
	/// <param name="parameters">Filtering and pagination parameters.</param>
	/// <returns>A list of assignments if found.</returns>
	/// <response code="200">Returns the filtered list of assignments.</response>
	/// <response code="404">If no assignments are found.</response>
	/// <response code="500">If an internal error occurs.</response>
	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<AssignmentDto>), 200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
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

	/// <summary>
	/// Retrieves all assignments created by a specific user.
	/// </summary>
	/// <param name="userId">The user’s unique identifier.</param>
	/// <returns>List of user-created assignments.</returns>
	/// <response code="200">Returns the list of assignments.</response>
	/// <response code="500">If an internal error occurs.</response>
	[HttpGet("user/{userId}")]
	[ProducesResponseType(typeof(IEnumerable<AssignmentDto>), 200)]
	[ProducesResponseType(500)]
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

	/// <summary>
	/// Retrieves all assignments by status.
	/// </summary>
	/// <param name="status">The task status (e.g., Pending, Completed).</param>
	/// <returns>List of assignments with the specified status.</returns>
	/// <response code="200">Returns the filtered list of assignments.</response>
	/// <response code="500">If an internal error occurs.</response>
	[HttpGet("status/{status}")]
	[ProducesResponseType(typeof(IEnumerable<AssignmentDto>), 200)]
	[ProducesResponseType(500)]
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

	/// <summary>
	/// Creates a new assignment.
	/// </summary>
	/// <param name="assignmentDto">The assignment to create.</param>
	/// <returns>Success if created.</returns>
	/// <response code="200">Returns true if assignment is saved.</response>
	/// <response code="400">If the user ID is invalid.</response>
	/// <response code="500">If an internal error occurs.</response>
	[Authorize(Roles = "Admin, Manager")]
	[HttpPost]
	[ProducesResponseType(typeof(bool), 200)]
	[ProducesResponseType(400)]
	[ProducesResponseType(500)]
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

	/// <summary>
	/// Updates an existing assignment.
	/// </summary>
	/// <param name="id">The assignment ID.</param>
	/// <param name="updatedAssignment">The updated assignment data.</param>
	/// <returns>Success if updated.</returns>
	/// <response code="200">If the update was successful.</response>
	/// <response code="404">If the assignment was not found.</response>
	/// <response code="500">If an internal error occurs.</response>
	[Authorize(Roles = "Admin, Manager")]
	[HttpPut("{id}")]
	[ProducesResponseType(typeof(bool), 200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
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

	/// <summary>
	/// Deletes an assignment by ID.
	/// </summary>
	/// <param name="id">The ID of the assignment to delete.</param>
	/// <returns>No content if deleted.</returns>
	/// <response code="204">If successfully deleted.</response>
	/// <response code="404">If the assignment is not found.</response>
	/// <response code="500">If an internal error occurs.</response>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
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

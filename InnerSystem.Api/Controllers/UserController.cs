using InnerSystem.Identity.DTOs.Users;
using InnerSystem.Identity.DTOs;
using InnerSystem.Identity.Enums;
using InnerSystem.Identity.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InnerSystem.Api.Controllers;

/// <summary>
/// Provides endpoints for managing users, including creation, retrieval, updating,
/// role management, and account status control.
/// Only accessible by users with Admin or Manager roles.
/// </summary>
[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin, Manager")]
public class UserController(IUserService userService) : ControllerBase
{
	/// <summary>
	/// Creates a new user account.
	/// </summary>
	/// <param name="user">The user information to create.</param>
	/// <returns>200 OK if successful.</returns>
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateUserDto user)
	{
		await userService.Create(user);
		return Ok();
	}

	/// <summary>
	/// Retrieves a paginated and filtered list of users.
	/// </summary>
	/// <returns>A list of users that match the filter criteria.</returns>
	[HttpGet]
	public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 10,
		[FromQuery] GenderEnum? gender = null,
		[FromQuery] string? email = null,
		[FromQuery] string? firstName = null,
		[FromQuery] string? lastName = null,
		[FromQuery] decimal? minSalary = null,
		[FromQuery] decimal? maxSalary = null,
		[FromQuery] decimal? minPoint = null,
		[FromQuery] decimal? maxPoint = null,
		[FromQuery] DateTime? birthDateFrom = null,
		[FromQuery] DateTime? birthDateTo = null,
		[FromQuery] Branch? branch = null,
		[FromQuery] bool? isActive = null,
		[FromQuery] string? sortBy = null,
		[FromQuery] bool sortDescending = false)
	{
		var users = await userService.GetAll(pageNumber, pageSize,
			gender, email, firstName, lastName,
			minSalary, maxSalary,
			minPoint, maxPoint,
			birthDateFrom, birthDateTo,
			branch, isActive,
			sortBy, sortDescending);

		return Ok(users);
	}

	/// <summary>
	/// Retrieves a specific user by their ID.
	/// </summary>
	/// <param name="id">The ID of the user.</param>
	/// <returns>The user's data if found.</returns>
	[HttpGet("{id}")]
	public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid id)
	{
		var user = await userService.GetById(id);
		return Ok(user);
	}

	/// <summary>
	/// Retrieves the roles assigned to a specific user.
	/// </summary>
	/// <param name="userId">The ID of the user.</param>
	/// <returns>A list of role names.</returns>
	[HttpGet("{userId}")]
	public async Task<ActionResult<IEnumerable<string>>> GetRoles([FromRoute] Guid userId)
	{
		var roles = await userService.GetUserRoles(userId);
		return Ok(roles);
	}

	/// <summary>
	/// Returns the total number of users in the system.
	/// </summary>
	/// <returns>The user count.</returns>
	[HttpGet]
	public async Task<ActionResult<int>> GetCount()
	{
		var count = await userService.GetCount();
		return Ok(count);
	}

	/// <summary>
	/// Adds points to a specific user.
	/// </summary>
	/// <param name="givePoints">Point allocation details.</param>
	/// <returns>200 OK if successful.</returns>
	[HttpPost]
	public async Task<IActionResult> AddPoints([FromBody] GivePointsDto givePoints)
	{
			await userService.AddPointsAsync(givePoints);
			return Ok();
	}

	/// <summary>
	/// Activates a user's account.
	/// </summary>
	/// <param name="id">The user ID.</param>
	/// <returns>200 OK if activated successfully.</returns>
	[HttpPost("{id}")]
	public async Task<IActionResult> Activate(Guid id)
	{
		await userService.ActivateAsync(id);
		return Ok();
	}

	/// <summary>
	/// Deactivates a user's account.
	/// </summary>
	/// <param name="id">The user ID.</param>
	/// <returns>200 OK if deactivated successfully.</returns>
	[HttpPost("{id}")]
	public async Task<IActionResult> Deactivate(Guid id)
	{
		await userService.DeactivateAsync(id);
		return Ok();
	}

	/// <summary>
	/// Updates the details of an existing user.
	/// </summary>
	/// <param name="updateUser">The updated user information.</param>
	/// <returns>The updated user data.</returns>
	[HttpPut]
	public async Task<ActionResult<UserDto>> Update([FromBody] UpdateUserDto updateUser)
	{
		var result = await userService.Update(updateUser);
		return Ok(result);
	}

	/// <summary>
	/// Resets the password for a user using the current password.
	/// </summary>
	/// <param name="resetPasswordDto">The reset password DTO containing old and new passwords.</param>
	/// <returns>200 OK if reset successfully; otherwise, 400 Bad Request.</returns>
	[HttpPut]
	public async Task<ActionResult<ResetPasswordDto>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
	{
		var result = await userService.ResetPassword(resetPasswordDto);
		return result ? Ok() : BadRequest();
	}

	/// <summary>
	/// Resets the password for a user (admin-only override).
	/// </summary>
	/// <param name="userId">The user ID.</param>
	/// <param name="newPassword">The new password string.</param>
	/// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
	[HttpPut("{userId}")]
	public async Task<ActionResult> ResetPasswordForAdmin([FromRoute] Guid userId, [FromBody] string newPassword)
	{
		var result = await userService.ResetPassword(userId, newPassword);
		return result ? Ok() : BadRequest();
	}

	/// <summary>
	/// Assigns roles to a user.
	/// </summary>
	/// <param name="userRoles">User roles assignment details.</param>
	/// <returns>200 OK if successful.</returns>
	[HttpPut]
	public async Task<ActionResult> AddRolesToUser([FromBody] UserRolesDto userRoles)
	{
		await userService.AddRolesToUser(userRoles);
		return Ok();
	}

	/// <summary>
	/// Removes roles from a user (Admin only).
	/// </summary>
	/// <param name="userRoles">User roles removal details.</param>
	/// <returns>200 OK if successful.</returns>
	[Authorize(Roles = "Admin")]
	[HttpDelete]
	public async Task<ActionResult> DeleteRolesFromUser([FromBody] UserRolesDto userRoles)
	{
		await userService.DeleteRolesFromUser(userRoles);
		return Ok();
	}

	/// <summary>
	/// Deletes a user by ID (Admin only).
	/// </summary>
	/// <param name="id">The ID of the user to delete.</param>
	/// <returns>200 OK if deleted successfully; otherwise, 400 Bad Request.</returns>
	[Authorize(Roles = "Admin")]
	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete([FromRoute] Guid id)
	{
		var result = await userService.Delete(id);
		return result ? Ok() : BadRequest();
	}
}

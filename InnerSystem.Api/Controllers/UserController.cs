using InnerSystem.Identity.DTOs.Users;
using InnerSystem.Identity.DTOs;
using InnerSystem.Identity.Enums;
using InnerSystem.Identity.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateUserDto user)
	{
		await userService.Create(user);
		return Ok();
	}

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

	[HttpGet("{id}")]
	public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid id)
	{
		var user = await userService.GetById(id);
		return Ok(user);
	}

	[HttpGet("{userId}")]
	public async Task<ActionResult<IEnumerable<string>>> GetRoles([FromRoute] Guid userId)
	{
		var roles = await userService.GetUserRoles(userId);
		return Ok(roles);
	}

	[HttpGet]
	public async Task<ActionResult<int>> GetCount()
	{
		var count = await userService.GetCount();
		return Ok(count);
	}

	[HttpPost]
	public async Task<IActionResult> AddPoints([FromBody] GivePointsDto givePoints)
	{
			await userService.AddPointsAsync(givePoints);
			return Ok();
	}

	[HttpPost("{id}")]
	public async Task<IActionResult> Activate(Guid id)
	{
		await userService.ActivateAsync(id);
		return Ok();
	}

	[HttpPost("{id}")]
	public async Task<IActionResult> Deactivate(Guid id)
	{
		await userService.DeactivateAsync(id);
		return Ok();
	}

	[HttpPut]
	public async Task<ActionResult<UserDto>> Update([FromBody] UpdateUserDto updateUser)
	{
		var result = await userService.Update(updateUser);
		return Ok(result);
	}

	[HttpPut]
	public async Task<ActionResult<ResetPasswordDto>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
	{
		var result = await userService.ResetPassword(resetPasswordDto);
		return result ? Ok() : BadRequest();
	}

	[HttpPut("{userId}")]
	public async Task<ActionResult> ResetPasswordForAdmin([FromRoute] Guid userId, [FromBody] string newPassword)
	{
		var result = await userService.ResetPassword(userId, newPassword);
		return result ? Ok() : BadRequest();
	}

	[HttpPut]
	public async Task<ActionResult> AddRolesToUser([FromBody] UserRolesDto userRoles)
	{
		await userService.AddRolesToUser(userRoles);
		return Ok();
	}

	[HttpDelete]
	public async Task<ActionResult> DeleteRolesFromUser([FromBody] UserRolesDto userRoles)
	{
		await userService.DeleteRolesFromUser(userRoles);
		return Ok();
	}

	[HttpDelete("{id}")]
	public async Task<ActionResult> Delete([FromRoute] Guid id)
	{
		var result = await userService.Delete(id);
		return result ? Ok() : BadRequest();
	}
}

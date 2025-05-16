using InnerSystem.Identity.Constants;
using InnerSystem.Identity.DTOs.Users;
using InnerSystem.Identity.DTOs;
using InnerSystem.Identity.Enums;
using InnerSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using InnerSystem.Identity.Abstract;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace InnerSystem.Identity.Services.Users;

public class UserService(ManagementSIdentityDbContext dbContext,
	IMappingService mappingService,
	IHttpContextAccessor httpContextAccessor,
	UserManager<User> userManager, RoleManager<Role> roleManager,
	IEnvironmentAccessor environmentAccessor) : IUserService
{
	public async Task Create(CreateUserDto userDto)
	{
		if (userDto.RoleName != RoleNames.User && userDto.RoleName != RoleNames.Admin && userDto.RoleName != RoleNames.Manager)
			throw new Exception(
				$"Wrong Role! You have to select '{RoleNames.User}' or '{RoleNames.Admin}' or {RoleNames.Manager}");

		if (await userManager.FindByEmailAsync(userDto.Email) != null)
			throw new Exception("This user already created.");

		var newUser = mappingService.Map<User, CreateUserDto>(userDto);
		newUser.UserName = userDto.Email;
		newUser.EmailConfirmed = true;

		var result = await userManager.CreateAsync(newUser, userDto.Password);

		if (!result.Succeeded)
			throw new Exception($"Didn't Succeeded.");

		await userManager.AddToRolesAsync(newUser, new string[]
		{
			RoleNames.User,
			userDto.RoleName
		});

		dbContext.SaveChanges();
	}

	public async Task<IEnumerable<UserDto>> GetAll(
	int pageNumber,
	int pageSize,
	// Filters
	GenderEnum? gender = null,
	string? email = null,
	string? firstName = null,
	string? lastName = null,
	decimal? minSalary = null,
	decimal? maxSalary = null,
	decimal? minPoint = null,
	decimal? maxPoint = null,
	DateTime? birthDateFrom = null,
	DateTime? birthDateTo = null,
	Branch? branch = null,
	bool? isActive = null,
	// Sorting
	string? sortBy = null,
	bool sortDescending = false)
	{
		var query = dbContext.Users.AsQueryable();

		// ──────── Filtering ────────

		if (gender != null)
			query = query.Where(u => u.Gender == gender);

		if (!string.IsNullOrWhiteSpace(email))
			query = query.Where(u => u.Email.Contains(email!));

		if (!string.IsNullOrWhiteSpace(firstName))
			query = query.Where(u => u.FirstName.Contains(firstName!));

		if (!string.IsNullOrWhiteSpace(lastName))
			query = query.Where(u => u.LastName.Contains(lastName!));

		if (minSalary != null)
			query = query.Where(u => u.Salary >= minSalary.Value);

		if (maxSalary != null)
			query = query.Where(u => u.Salary <= maxSalary.Value);

		if (minPoint != null)
			query = query.Where(u => u.GivenPoint >= minPoint.Value);

		if (maxPoint != null)
			query = query.Where(u => u.GivenPoint <= maxPoint.Value);

		if (birthDateFrom != null)
			query = query.Where(u => u.BirthDate >= birthDateFrom.Value);

		if (birthDateTo != null)
			query = query.Where(u => u.BirthDate <= birthDateTo.Value);

		if (branch != null)
			query = query.Where(u => u.Branch == branch);

		if (isActive != null)
			query = query.Where(u => u.IsActive == isActive.Value);

		// ──────── Sorting ────────

		// Default sort
		sortBy ??= nameof(User.CreateDate);

		// Normalize
		sortBy = sortBy.Trim();
		bool desc = sortDescending;

		query = sortBy switch
		{
			nameof(User.FirstName) => desc ? query.OrderByDescending(u => u.FirstName)
												: query.OrderBy(u => u.FirstName),
			nameof(User.LastName) => desc ? query.OrderByDescending(u => u.LastName)
												: query.OrderBy(u => u.LastName),
			nameof(User.Email) => desc ? query.OrderByDescending(u => u.Email)
												: query.OrderBy(u => u.Email),
			nameof(User.Salary) => desc ? query.OrderByDescending(u => u.Salary)
												: query.OrderBy(u => u.Salary),
			nameof(User.Gender) => desc ? query.OrderByDescending(u => u.Gender)
												: query.OrderBy(u => u.Gender),
			nameof(User.BirthDate) => desc ? query.OrderByDescending(u => u.BirthDate)
												: query.OrderBy(u => u.BirthDate),
			nameof(User.Branch) => desc ? query.OrderByDescending(u => u.Branch)
												: query.OrderBy(u => u.Branch),
			nameof(User.GivenPoint) => desc ? query.OrderByDescending(u => u.GivenPoint)
												: query.OrderBy(u => u.GivenPoint),
			nameof(User.IsActive) => desc ? query.OrderByDescending(u => u.IsActive)
												: query.OrderBy(u => u.IsActive),
			nameof(User.CreateDate) => desc ? query.OrderByDescending(u => u.CreateDate)
												: query.OrderBy(u => u.CreateDate),
			_ => desc ? query.OrderByDescending(u => u.CreateDate)
												: query.OrderBy(u => u.CreateDate),
		};

		// ──────── Paging ────────

		if (pageNumber > 0 && pageSize > 0)
			query = query
				.Skip(pageSize * (pageNumber - 1))
				.Take(pageSize);

		// ──────── Projection & Execution ────────

		var users = await query.ToArrayAsync();

		return mappingService.Map<IEnumerable<UserDto>, IEnumerable<User>>(users);
	}


	public async Task<UserDto> GetById(Guid id)
	{
		//if (!environmentAccessor.IsAdmin(id))
		//	throw new Exception("You you have not access for view information about this user.");

		var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id) ??
			throw new Exception("User not found.");
		return mappingService.Map<UserDto, User>(user);
	}

	public async Task<UserRolesDto> GetUserRoles(Guid userId)
	{
		if (await userManager.FindByIdAsync(userId.ToString()) == null)
			throw new Exception("User not found.");

		var userRolesDto = new UserRolesDto();
		var userRoles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(userId.ToString()));
		var roles = new List<RoleDto>();

		foreach (var role in userRoles)
		{
			var userRole = await roleManager.FindByNameAsync(role);
			roles.Add(new RoleDto(userRole.Name));
		}

		userRolesDto.UserId = userId;
		userRolesDto.Roles = roles;

		return userRolesDto;
	}

	public async Task<string> GetUserFullName(Guid id)
	{
		var user = await userManager.FindByIdAsync(id.ToString()) ??
			throw new Exception("User not found.");

		return user.FirstName + " " + user.LastName;
	}

	public async Task<int> GetCount()
	{
		var count_of_users = await userManager.Users.CountAsync();
		return count_of_users;
	}

	public async Task<UserDto> Update(UpdateUserDto updateUser)
	{
		var user = await userManager.FindByIdAsync(updateUser.Id.ToString()) ??
			throw new Exception("User not Found");

		if (!environmentAccessor.IsUserOrAdmin(updateUser.Id))
			throw new Exception("You have not access for change this user information.");

		var userNewData = mappingService.Map(updateUser, user);

		var result = await userManager.UpdateAsync(userNewData);

		if (!result.Succeeded)
			throw new Exception("It is not Succeeded.");

		dbContext.SaveChanges();

		return mappingService.Map<UserDto, User>(userNewData) ??
			throw new Exception("There are some errors.");
	}

	public async Task<bool> ResetPassword(ResetPasswordDto resetPasswordDto)
	{
		var user = await userManager.FindByIdAsync(resetPasswordDto.UserId.ToString()) ??
		throw new Exception("User not found.");

		if (!environmentAccessor.IsAdmin(resetPasswordDto.UserId))
			return false;

		var result = await userManager.ChangePasswordAsync(user, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword);

		if (!result.Succeeded)
			throw new Exception(result.Errors.Select(x => x.Description).ToString());
		dbContext.SaveChanges();
		return true;
	}

	public async Task<bool> ResetPassword(Guid userId, string newPassword)
	{
		if (!environmentAccessor.IsAdmin(Guid.Parse(environmentAccessor.GetUserId())))
			throw new Exception("You cannot change user password");

		var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == Convert.ToString(userId)) ??
			throw new Exception("User not found.");

		await userManager.RemovePasswordAsync(user);
		await userManager.AddPasswordAsync(user, newPassword);

		dbContext.SaveChanges();

		return true;
	}

	public async Task<UserRolesDto> AddRolesToUser(UserRolesDto userRolesDto)
	{
		var user = await userManager.FindByIdAsync(userRolesDto.UserId.ToString());
		IList<string> addingRoles = new List<string>();

		foreach (var role in userRolesDto.Roles)
			addingRoles.Add(role.Name);

		var result = await userManager.AddToRolesAsync(user, addingRoles);

		if (!result.Succeeded)
			throw new Exception(result.Errors.Select(x => x.Description).ToString());

		dbContext.SaveChanges();

		var userRoles = await userManager.GetRolesAsync(user);
		var rolesDTOs = new List<RoleDto>();

		foreach (var role in userRoles)
		{
			var userRole = await roleManager.FindByNameAsync(role);
			rolesDTOs.Add(new RoleDto(userRole.Name));
		}

		userRolesDto.Roles = rolesDTOs;

		return userRolesDto;
	}

	public async Task<bool> Delete(Guid id)
	{
		var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id.ToString() == Convert.ToString(id));

		if (user == null || !environmentAccessor.IsAdmin(id))
			return false;

		await userManager.DeleteAsync(user);
		dbContext.SaveChanges();

		return true;

	}

	public async Task<UserRolesDto> DeleteRolesFromUser(UserRolesDto userRolesDto)
	{
		var user = await userManager.FindByIdAsync(userRolesDto.UserId.ToString());
		var removingRoles = new List<string>();

		foreach (var role in userRolesDto.Roles)
			removingRoles.Add(role.Name);

		var result = await userManager.RemoveFromRolesAsync(user, removingRoles);

		if (!result.Succeeded)
			throw new Exception(result.Errors.Select(x => x.Description).ToString());

		dbContext.SaveChanges();

		var userRoles = await userManager.GetRolesAsync(user);
		var roles = new List<RoleDto>();

		foreach (var role in userRoles)
		{
			var userRole = await roleManager.FindByNameAsync(role);
			roles.Add(new RoleDto(userRole.Name));
		}

		userRolesDto.Roles = roles;

		return userRolesDto;
	}

	public async Task AddPointsAsync(GivePointsDto givePoints)
	{
		var employee = await userManager.Users.FirstOrDefaultAsync(x => x.Id == givePoints.Id)
					  ?? throw new KeyNotFoundException("User not found");

		if (!employee.IsActive)
			throw new InvalidOperationException("Cannot add points to inactive User");

		employee.AddPoints(givePoints.Points);
		await dbContext.SaveChangesAsync();
	}

	public async Task ActivateAsync(Guid id)
	{
		var employee = await userManager.Users.FirstOrDefaultAsync(X => X.Id == id)
					  ?? throw new KeyNotFoundException("Employee not found");

		employee.Activate();
		await dbContext.SaveChangesAsync();
	}

	public async Task DeactivateAsync(Guid id)
	{
		var employee = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id)
					  ?? throw new KeyNotFoundException("Employee not found");

		employee.Deactivate();
		await dbContext.SaveChangesAsync();
	}
}

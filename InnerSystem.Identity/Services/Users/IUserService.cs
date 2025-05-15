using InnerSystem.Identity.DTOs;
using InnerSystem.Identity.DTOs.Users;
using InnerSystem.Identity.Enums;

namespace InnerSystem.Identity.Services.Users;

public interface IUserService
{
	Task Create(CreateUserDto userDto);
	//Task<IEnumerable<UserDto>> GetAll(int pageNumber, int pageSize, GenderEnum? gender, string? email);
	Task<IEnumerable<UserDto>> GetAll(int pageNumber,
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
	bool sortDescending = false);
	Task<UserDto> GetById(Guid id);
	Task<string> GetUserFullName(Guid id);
	Task<UserRolesDto> GetUserRoles(Guid id);
	Task<int> GetCount();
	Task<UserDto> Update(UpdateUserDto userEM);
	Task<UserRolesDto> AddRolesToUser(UserRolesDto userRoles);
	Task<bool> ResetPassword(ResetPasswordDto resetPasswordDto);
	Task<bool> ResetPassword(Guid userId, string NewPassword);
	Task<bool> Delete(Guid id);
	Task<UserRolesDto> DeleteRolesFromUser(UserRolesDto userRoles);
	Task AddPointsAsync(GivePointsDto givePoints);
	Task ActivateAsync(Guid id);
	Task DeactivateAsync(Guid id);
}

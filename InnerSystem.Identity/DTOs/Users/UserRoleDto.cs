using System.Diagnostics.CodeAnalysis;

namespace InnerSystem.Identity.DTOs.Users;

public class UserRoleDto 
{
	public Guid UserId { get; set; }

	[AllowNull]
	public IEnumerable<RoleDto>? Roles { get; set; }
}

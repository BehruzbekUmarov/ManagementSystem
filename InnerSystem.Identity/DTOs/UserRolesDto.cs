using System.Diagnostics.CodeAnalysis;

namespace InnerSystem.Identity.DTOs;

public class UserRolesDto
{
	public Guid UserId { get; set; }

	[AllowNull]
	public IEnumerable<RoleDto>? Roles { get; set; }
}

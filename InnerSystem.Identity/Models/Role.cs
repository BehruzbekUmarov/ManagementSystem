using Microsoft.AspNetCore.Identity;

namespace InnerSystem.Identity.Models;

public class Role : IdentityRole<Guid>
{
	public Role()
	{
	}
	public Role(string role) : base(role)
	{
		Id = Guid.NewGuid();
		Name = role;
		NormalizedName = role.ToUpper();
	}
}

using InnerSystem.Identity.Constants;
using InnerSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace InnerSystem.Identity.ClaimsPrincipalFactory;

public class InnerSystemClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    public InnerSystemClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor)
		: base(userManager, optionsAccessor) {}

	protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
	{
		var identity = await base.GenerateClaimsAsync(user);
		identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
		identity.AddClaim(new Claim(ClaimNames.UserName, user.UserName));
		identity.AddClaim(new Claim(ClaimNames.Email, user.Email));
		identity.AddClaim(new Claim(ClaimNames.UserId, user.Id.ToString()));
		identity.AddClaim(new Claim(ClaimNames.FirstName, user.FirstName));
		identity.AddClaim(new Claim(ClaimNames.LastName, user.LastName));

		return identity;
	}
}

using InnerSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace InnerSystem.Identity.Middleware;

public static class RoleInitializer
{
	private static readonly string[] Roles = new[] { "Admin", "User", "Manager" };

	public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
	{
		foreach (var role in Roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new Role(role));
			}
		}
	}

	public static async Task SeedAdminAsync(UserManager<User> userManager)
	{
		var adminEmail = "admin@example.com";
		var adminUser = await userManager.FindByEmailAsync(adminEmail);

		if (adminUser == null)
		{
			var user = new User("System", "Administrator", adminEmail)
			{
				EmailConfirmed = true
			};

			var result = await userManager.CreateAsync(user, "Admin@123");
			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, "Admin");
			}
		}
	}
}

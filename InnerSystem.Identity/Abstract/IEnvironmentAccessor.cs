namespace InnerSystem.Identity.Abstract;

public interface IEnvironmentAccessor
{
	string GetFullName();
	string GetWebRootPath();
	bool HasRole(string role);
	bool IsAdmin(Guid id);
	bool IsUserOrAdmin(Guid id);
	string GetUserId();
	string? UserId();
	string GetUserName();
}

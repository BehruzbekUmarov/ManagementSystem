using InnerSystem.Identity.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Tls;
using System;
using System.Security.Claims;

namespace InnerSystem.Identity.Abstract;

public class EnvironmentAccessor(IHttpContextAccessor contextAccessor,
	IWebHostEnvironment environment) : IEnvironmentAccessor
{
	public string GetFullName()
	{
		var firstName = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimNames.FirstName.ToString()))?.Value;
		var lastName = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimNames.LastName.ToString()))?.Value;

		return $"{firstName} {lastName}";
	}

	public string GetUserId()
	{
		return contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimNames.UserId.ToString()))?.Value;
	}

	public string? UserId()
	{
		return contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
	}

	public string GetUserName()
	{
		return contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimNames.UserName.ToString()))?.Value;
	}

	public string GetWebRootPath()
	{
		return environment.WebRootPath;
	}

	public bool HasRole(string role)
	{
		return new[] { RoleNames.User, RoleNames.Admin }
		.Contains(role);
	}

	public bool IsAdmin(Guid id)
	{
		if (contextAccessor.HttpContext is null)
			throw new Exception("HttpContext can not be null");

		if (contextAccessor.HttpContext.User.IsInRole(RoleNames.Admin))
			return true;
		return false;
	}

	public bool IsUserOrAdmin(Guid id)
	{
		if (contextAccessor.HttpContext is null)
			throw new Exception("HttpContext can not be null.");

		var check = contextAccessor.HttpContext.User.IsInRole(RoleNames.Admin) || GetUserId() == id.ToString();

		if (check)
			return true;
		return false;
	}
}

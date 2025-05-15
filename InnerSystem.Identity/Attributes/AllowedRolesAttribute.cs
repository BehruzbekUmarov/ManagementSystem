using InnerSystem.Identity.Constants;
using System.ComponentModel.DataAnnotations;

namespace InnerSystem.Identity.Attributes;

public class AllowedRolesAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		var role = value as string;
		if (role != null && (role == RoleNames.User || role == RoleNames.Admin || role == RoleNames.Manager))
			return ValidationResult.Success;

		return new ValidationResult($"Invalid role. Allowed values are '{RoleNames.User}', {RoleNames.Manager} and '{RoleNames.Admin}'.");
	}
}

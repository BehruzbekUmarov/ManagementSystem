﻿using InnerSystem.Identity.Enums;

namespace InnerSystem.Identity.DTOs.Auth;

public class LoginResponseDto
{
	public LoginResponseDto(
				Guid id, string token, DateTime expiration,
				string firstName, string lastName,
				GenderEnum? gender, DateTime birthDate,
				string email, IList<string> roles)
	{
		Id = id;
		Token = token;
		Expiration = expiration;
		FirstName = firstName;
		LastName = lastName;
		Gender = gender;
		BirthDate = birthDate;
		Email = email;
		Roles = roles;
	}

	public Guid Id { get; set; }
	public string Token { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTime Expiration { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public GenderEnum? Gender { get; set; }
	public DateTime BirthDate { get; set; }
	public IList<string>? Roles { get; set; }
}

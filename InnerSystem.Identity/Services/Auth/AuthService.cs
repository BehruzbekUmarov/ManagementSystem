using InnerSystem.Identity.AccessConfigurations;
using InnerSystem.Identity.Constants;
using InnerSystem.Identity.DTOs.Auth;
using InnerSystem.Identity.Email;
using InnerSystem.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InnerSystem.Identity.Services.Auth;

public class AuthService(IOptions<AccessConfiguration> siteSettings,
			UserManager<User> userManager,
			ManagementSIdentityDbContext context,
			IEmailSender emailSender) : IAuthService
{
	public async Task<bool> ForgetPassword(string email)
	{
		var user = await userManager.FindByEmailAsync(email) ??
			throw new Exception("User not found.");

		await emailSender.SendOtpEmailAsync(user.Email!);

		return true;
	}

	public async Task<LoginResponseDto> Login(LoginDto loginDto)
	{
		var user = await userManager.FindByEmailAsync(loginDto.Email) ??
			throw new Exception("Not Found");

		if (!await userManager.CheckPasswordAsync(user, loginDto.Password))
			throw new Exception("Your Password is incorrect.");

		if (!user.EmailConfirmed)
		{
			await emailSender.SendOtpEmailAsync(user.Email);
			throw new Exception($"Please verify your email. {user.Email}");
		}

		var roles = await userManager.GetRolesAsync(user);

		var authClaims = new List<Claim>()
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new Claim(ClaimNames.UserId, Convert.ToString(user.Id)),
			new Claim(ClaimNames.FirstName, user.FirstName),
			new Claim(ClaimNames.LastName, user.LastName)
		};

		var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey.TheSecretKey));

		// add roels to claims
		foreach (var role in roles)
		{
			var roleClaim = new Claim(ClaimTypes.Role, role);
			authClaims.Add(roleClaim);
		}

		var jwtSecurityToken = new JwtSecurityToken(
			issuer: siteSettings.Value.Issuer,
			audience: siteSettings.Value.Audience,
			expires: DateTime.Now.AddDays(10),
			claims: authClaims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

		var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

		return new LoginResponseDto(
						user.Id, token, jwtSecurityToken.ValidTo,
						user.FirstName, user.LastName, user.Gender,
						user.BirthDate, user.PhoneNumber, roles);
	}

	public async Task<SignUpResponseDto> Register(SignUpDto signUpDto)
	{
		var user = await userManager.FindByEmailAsync(signUpDto.Email);

		if (user != null && user.EmailConfirmed == true)
			throw new Exception("This user already created. You can Login to your account.");

		if (user != null && user.EmailConfirmed == false)
			await userManager.DeleteAsync(user);

		var newUser = new User(signUpDto.FirstName, signUpDto.LastName, signUpDto.Email);

		var result = await userManager.CreateAsync(newUser, signUpDto.Password);

		if (!result.Succeeded)
			throw new Exception("Didn't Succeed.");

		var roles = new List<string> { RoleNames.User, signUpDto.Role };

		await userManager.AddToRolesAsync(newUser, roles);

		await emailSender.SendOtpEmailAsync(signUpDto.Email);

		return new SignUpResponseDto(newUser.Id, newUser.Email, newUser.FirstName, newUser.LastName, roles);
	}

	public async Task<bool> ResetPassword(string email, string newPassword, string code)
	{
		var user = await userManager.FindByEmailAsync(email) ??
			throw new Exception("User not found.");

		var emailToken = context.EmailTokens.Where(x => x.Email == email).FirstOrDefault() ??
			throw new Exception("Code not found.");

		if (emailToken.EmailCode != code)
			throw new Exception("Code is incorrect.");

		await userManager.RemovePasswordAsync(user);
		await userManager.AddPasswordAsync(user, newPassword);

		return true;
	}

	public async Task<bool> VerifyEmail(string email, string userCode)
	{
		var user = await userManager.FindByEmailAsync(email) ??
			throw new Exception("User not found.");

		var code = context.EmailTokens.Where(x => x.Email == email).FirstOrDefault()?.EmailCode ??
			throw new Exception("Code not found.");

		if (code != userCode)
			throw new Exception("Code is incorrect.");

		user.EmailConfirmed = true;
		await userManager.UpdateAsync(user);

		return true;
	}
}

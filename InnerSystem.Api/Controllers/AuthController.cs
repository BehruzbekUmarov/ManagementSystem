using InnerSystem.Identity.DTOs.Auth;
using InnerSystem.Identity.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
	/// <summary>
	/// Authenticates the user and returns a JWT token if successful.
	/// </summary>
	/// <param name="loginDto">User credentials (email and password).</param>
	/// <returns>JWT token and user info.</returns>
	[HttpPost]
	public async Task<ActionResult<LoginResponseDto>> LoginAsync([FromBody] LoginDto loginDto)
	{
		return Ok(await authService.Login(loginDto));
	}

	/// <summary>
	/// Registers a new user.
	/// </summary>
	/// <param name="signUpDto">New user information.</param>
	/// <returns>Success message and user ID.</returns>
	[HttpPost]
	public async Task<ActionResult<SignUpResponseDto>> SignUpAsync([FromBody] SignUpDto signUpDto)
	{
		return Ok(await authService.Register(signUpDto));
	}

	/// <summary>
	/// Verifies the user's email using a verification code.
	/// </summary>
	/// <param name="verifyDto">Email and verification code.</param>
	/// <returns>Success or failure message.</returns>
	[HttpPost]
	public async Task<IActionResult> VerifyEmail([FromBody] VerifyPhoneNumberDto verifyDto)
	{
		if (await authService.VerifyEmail(verifyDto.Email, verifyDto.Code))
			return Ok("Successfull!");
		return BadRequest("Not Saccessfull!");
	}

	/// <summary>
	/// Sends a reset password code to the user's email.
	/// </summary>
	/// <param name="forgetPasswordDto">User's email address.</param>
	/// <returns>Success or failure message.</returns>
	[HttpPost]
	public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
	{
		if (await authService.ForgetPassword(forgetPasswordDto.Email))
			return Ok("Successfull!");
		return BadRequest("Not Saccessfull!");
	}

	/// <summary>
	/// Resets the user's password using the verification code.
	/// </summary>
	/// <param name="resetPasswordDto">Email, new password, and verification code.</param>
	/// <returns>Success or failure message.</returns>
	[HttpPost]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordByCodeDto resetPasswordDto)
	{
		if (await authService.ResetPassword(resetPasswordDto.Email, resetPasswordDto.NewPassword, resetPasswordDto.Code))
			return Ok("Successfull!");
		return BadRequest("Not Saccessfull!");
	}
}

using InnerSystem.Identity.DTOs.Auth;
using InnerSystem.Identity.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace InnerSystem.Api.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult<LoginResponseDto>> LoginAsync([FromBody] LoginDto loginDto)
	{
		return Ok(await authService.Login(loginDto));
	}

	[HttpPost]
	public async Task<ActionResult<SignUpResponseDto>> SignUpAsync([FromBody] SignUpDto signUpDto)
	{
		return Ok(await authService.Register(signUpDto));
	}

	[HttpPost]
	public async Task<IActionResult> VerifyEmail([FromBody] VerifyPhoneNumberDto verifyDto)
	{
		if (await authService.VerifyEmail(verifyDto.Email, verifyDto.Code))
			return Ok("Successfull!");
		return BadRequest("Not Saccessfull!");
	}

	[HttpPost]
	public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
	{
		if (await authService.ForgetPassword(forgetPasswordDto.Email))
			return Ok("Successfull!");
		return BadRequest("Not Saccessfull!");
	}

	[HttpPost]
	public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordByCodeDto resetPasswordDto)
	{
		if (await authService.ResetPassword(resetPasswordDto.Email, resetPasswordDto.NewPassword, resetPasswordDto.Code))
			return Ok("Successfull!");
		return BadRequest("Not Saccessfull!");
	}
}

using InnerSystem.Identity.DTOs.Auth;

namespace InnerSystem.Identity.Services.Auth;

public interface IAuthService
{
	Task<LoginResponseDto> Login(LoginDto loginDto);
	Task<SignUpResponseDto> Register(SignUpDto registerDto);
	Task<bool> VerifyEmail(string email, string userCode);
	Task<bool> ForgetPassword(string email);
	Task<bool> ResetPassword(string email, string newPassword, string code);
}

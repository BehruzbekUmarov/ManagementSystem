namespace InnerSystem.Identity.Email;

public interface IEmailSender
{
	Task SendOtpEmailAsync(string recipientEmail);
}

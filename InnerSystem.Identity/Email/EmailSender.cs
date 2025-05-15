using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace InnerSystem.Identity.Email;

public class EmailSender(IOptions<EmailClientOptions> options,
	ManagementSIdentityDbContext context) : IEmailSender
{
	private readonly EmailClientOptions _options = options.Value;

	public async Task SendOtpEmailAsync(string recipientEmail)
	{
		var code = new Random().Next(1000, 9999).ToString();
		var token = await context.EmailTokens.FirstOrDefaultAsync(x => x.Email == recipientEmail);

		if (token != null)
			context.EmailTokens.Remove(token);

		await context.EmailTokens.AddAsync(new EmailToken(code, recipientEmail));
		await context.SaveChangesAsync();

		var subject = "Management System Tasdiqlash Kodingiz";
		var body = $"Assalomu aleykum. Management System uchun tasdiqlash kodi: {code}";

		using var smtp = new SmtpClient(_options.SmtpServer, _options.SmtpPort)
		{
			EnableSsl = true,
			Credentials = new NetworkCredential(_options.SenderEmail, _options.SenderPassword)
		};

		var mailMessage = new MailMessage
		{
			From = new MailAddress(_options.SenderEmail, _options.SenderName),
			Subject = subject,
			Body = body,
			IsBodyHtml = false
		};

		mailMessage.To.Add(recipientEmail);
		await smtp.SendMailAsync(mailMessage);
	}
}

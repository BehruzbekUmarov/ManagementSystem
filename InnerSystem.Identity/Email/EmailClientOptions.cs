namespace InnerSystem.Identity.Email;

public class EmailClientOptions
{
	public const string EmailSectionName = "Email";
	public string SmtpServer { get; set; } = string.Empty;
	public int SmtpPort { get; set; }
	public string SenderEmail { get; set; } = string.Empty;
	public string SenderName { get; set; } = string.Empty;
	public string SenderPassword { get; set; } = string.Empty;
}

namespace InnerSystem.Identity.Email;

public class EmailToken
{

    public EmailToken(string emailCode, string email)
    {
        EmailCode = emailCode;
        Email = email;
    }

    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string EmailCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

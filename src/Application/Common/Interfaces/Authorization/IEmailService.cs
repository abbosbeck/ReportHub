namespace Application.Common.Interfaces.Authorization;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);

    Task SendEmailWithFileAsync(string email, string subject, string htmlMessage, byte[] file);
}
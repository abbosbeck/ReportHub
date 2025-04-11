namespace Application.Common.Interfaces.Authorization;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}
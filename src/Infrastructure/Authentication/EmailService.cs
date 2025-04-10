using Application.Common.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Authentication;

public class EmailService(IOptions<EmailOptions> emailOptions, IOptions<SmtpOptions> smtpOptions) : IEmailService
{
    private readonly EmailOptions emailOptions = emailOptions.Value;
    private readonly SmtpOptions smtpOptions = smtpOptions.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("ReportHub", emailOptions.Sender));
        message.To.Add(new MailboxAddress("User", email));
        message.Subject = subject;
        message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

        var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            host: smtpOptions.Host,
            port: smtpOptions.Port,
            options: SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            userName: emailOptions.Sender,
            password: emailOptions.Password);

        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
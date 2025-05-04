using Application.Common.Interfaces.Authorization;
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

    public async Task SendEmailWithFileAsync(string email, string subject, string htmlMessage, byte[] file)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("ReportHub", emailOptions.Sender));
        message.To.Add(new MailboxAddress("User", email));
        message.Subject = subject;

        var body = new TextPart(TextFormat.Html) { Text = htmlMessage };

        var attachment = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            Content = new MimeContent(new MemoryStream(file)),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = "Report.xlsx"
        };

        var multiPart = new Multipart("mixed");
        multiPart.Add(body);
        multiPart.Add(attachment);

        message.Body = multiPart;

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
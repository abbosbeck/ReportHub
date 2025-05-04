using Application.Common.Interfaces.Authorization;
using Application.ExportReports.ExportReportsToFile;
using Application.ExportReports.ExportReportsToFile.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Quartz;

namespace Infrastructure.Jobs;

public class ReportEmailSenderJob(
    IEmailService emailService,
    UserManager<User> userManager,
    IExportReportsToFileQueryHandler fileQueryHandler)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.MergedJobDataMap;
        var userId = dataMap.GetGuid("UserId").ToString();
        var user = await userManager.FindByIdAsync(userId);
        var clientId = dataMap.GetGuid("ClientId");
        var request = new ExportReportsToFileQuery(clientId, ExportReportsFileType.Excel, null);
        var report = await fileQueryHandler.Handle(request, CancellationToken.None);

        var message = $"<p><strong>Hi {user!.FirstName}," +
                      $"</strong></p>\r\n\r\n<p>Your weekly report is ready and waiting for you!</p>" +
                      $"\r\n\r\n<p>\r\n<p>Have a productive week ahead!</p>" +
                      $"\r\n\r\n<p>Best regards,<br>ReportHub!</p>\r\n";

        await emailService.SendEmailWithFileAsync(
            user!.Email,
            $"Your Weekly Report!",
            message,
            report.ByteArray);
    }
}
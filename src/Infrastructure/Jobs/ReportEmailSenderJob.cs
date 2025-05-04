using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Quartz;

namespace Infrastructure.Jobs;

public class ReportEmailSenderJob(IEmailService emailService, UserManager<User> userManager) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.MergedJobDataMap;
        var userId = dataMap.GetGuid("UserId").ToString();
        var user = await userManager.FindByIdAsync(userId);

        await emailService.SendEmailAsync(user!.Email, $"{user.UserName}", $"{user.FirstName}");
    }
}
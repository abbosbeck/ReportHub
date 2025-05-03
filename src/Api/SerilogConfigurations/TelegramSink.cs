using Application.Common.Services;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Api.SerilogConfigurations;

public class TelegramSink(IFormatProvider formatProvider, IConfiguration configuration) : ILogEventSink
{
    public void Emit(LogEvent logEvent)
    {
        var message = logEvent.RenderMessage(formatProvider);
        LogSenderService.SendToTelegram(
            configuration["TelegramLogBot:Api"],
            configuration["TelegramLogBot:ChatId"],
            DateTimeOffset.Now.ToString() + " " + message);
    }
}

public static class TelegramSinkExtensions
{
    public static LoggerConfiguration Telegram(
              this LoggerSinkConfiguration loggerConfiguration,
              IConfiguration configuration,
              IFormatProvider formatProvider = null)
    {
        return loggerConfiguration.Sink(new TelegramSink(formatProvider, configuration));
    }
}
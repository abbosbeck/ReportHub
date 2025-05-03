using System.Text;
using System.Text.Json;

namespace Application.Common.Services;

public static class LogSenderService
{
    public static void SendToTelegram(string botToken, string chatId, string message)
    {
        using (var client = new HttpClient())
        {
            var url = $"https://api.telegram.org/bot{botToken}/sendMessage";
            var content = new StringContent(
                JsonSerializer.Serialize(new
            {
                chat_id = chatId,
                text = message,
            }), Encoding.UTF8,
                "application/json");

            try
            {
                _ = client.PostAsync(url, content).Result;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("An error occurred while sending a message to the Telegram group about this exception.");
                Console.ResetColor();
            }
        }
    }
}

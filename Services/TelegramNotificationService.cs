using Telegram.Bot;
using Telegram.Bot.Types.Enums;


namespace Voltix.NotificationMicroservice.Services;

public interface ITelegramNotificationService {
    public Task SendMessageAsync(long telegramId, string message);
}

public class TelegramNotificationService : ITelegramNotificationService {
    private readonly IConfiguration _configuration;
    private readonly TelegramBotClient _botClient;

    public TelegramNotificationService(IConfiguration configuration) {
        _configuration = configuration;
        _botClient = new TelegramBotClient(_configuration.GetValue<string>("Telegram:Token")!);
    }

    public async Task SendMessageAsync(long telegramId, string message) {
        await _botClient.SendMessage(
            telegramId,
            message,
            parseMode: ParseMode.Html
        );
    }
}

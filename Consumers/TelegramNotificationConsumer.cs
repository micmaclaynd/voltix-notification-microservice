using MassTransit;
using Voltix.NotificationMicroservice.Interfaces.Amqp;
using Voltix.NotificationMicroservice.Services;


namespace Voltix.NotificationMicroservice.Consumers;

public class TelegramNotificationConsumer(ITelegramNotificationService telegramNotificationService) : IConsumer<ITelegramNotificationRequest> {
    private readonly ITelegramNotificationService _telegramNotificationService = telegramNotificationService;

    public async Task Consume(ConsumeContext<ITelegramNotificationRequest> context) {
        var message = context.Message;
        await _telegramNotificationService.SendMessageAsync(message.TelegramId, message.Message);
    }
}

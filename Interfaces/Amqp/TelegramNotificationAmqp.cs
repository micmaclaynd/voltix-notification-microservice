namespace Voltix.NotificationMicroservice.Interfaces.Amqp;

public class ITelegramNotificationRequest {
    public required long TelegramId { get; set; }
    public required string Message { get; set; }
}

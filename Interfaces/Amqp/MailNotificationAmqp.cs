namespace Voltix.NotificationMicroservice.Interfaces.Amqp;

public class IMailNotificationRequest {
    public required string Receiver { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
}

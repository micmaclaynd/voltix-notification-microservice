using MassTransit;
using Voltix.NotificationMicroservice.Interfaces.Amqp;
using Voltix.NotificationMicroservice.Services;


namespace Voltix.NotificationMicroservice.Consumers;

public class MailNotificationConsumer(IMailNotificationService mailNotificationService) : IConsumer<IMailNotificationRequest> {
    private readonly IMailNotificationService _mailNotificationService = mailNotificationService;

    public async Task Consume(ConsumeContext<IMailNotificationRequest> context) {
        var message = context.Message;
        await _mailNotificationService.SendMailAsync(message.Receiver, message.Subject, message.Body);
    }
}

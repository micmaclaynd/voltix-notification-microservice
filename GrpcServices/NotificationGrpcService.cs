using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MassTransit;
using Microsoft.Extensions.Options;
using Voltix.NotificationMicroservice.Interfaces.Amqp;
using Voltix.NotificationMicroservice.Interfaces.Options;
using Voltix.NotificationMicroservice.Models;
using Voltix.NotificationMicroservice.Protos;
using Voltix.NotificationMicroservice.Services;


namespace Voltix.NotificationMicroservice.GrpcServices;

public class NotificationGrpcService(
    IOptions<IMailOptions> mailOptions,
    IOptions<ITelegramOptions> telegramOptions,
    IPublishEndpoint publishEndpoint,
    IWebNotificationService webNotificationService
) : NotificationProto.NotificationProtoBase {
    private readonly IMailOptions _mailOptions = mailOptions.Value;
    private readonly ITelegramOptions _telegramOptions = telegramOptions.Value;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly IWebNotificationService _webNotificationService = webNotificationService;

    public override async Task<Empty> SendWebNotification(SendWebNotificationRequest request, ServerCallContext context) {
        await _webNotificationService.AddWebNotificationAsync(new WebNotificationModel {
            AddedDateTime = DateTime.UtcNow,
            Message = request.Message,
            UserId = request.UserId
        });
        return new Empty();
    }

    public override async Task<Empty> SendConfirmEmailNotification(SendConfirmEmailNotificationRequest request, ServerCallContext context) {
        var mailTemplate = await File.ReadAllTextAsync(_mailOptions.Templates.ConfirmEmail);
        await _publishEndpoint.Publish<IMailNotificationRequest>(new() {
            Receiver = request.Email,
            Subject = "Confirm email for voltix",
            Body = mailTemplate.Replace("{{token}}", request.Token)
        });
        return new Empty();
    }

    public override async Task<Empty> SendConfirmTelegramNotification(SendConfirmTelegramNotificationRequest request, ServerCallContext context) {
        var telegramTemplate = await File.ReadAllTextAsync(_telegramOptions.Templates.ConfirmTelegram);
        await _publishEndpoint.Publish<ITelegramNotificationRequest>(new() {
            TelegramId = request.TelegramId,
            Message = telegramTemplate.Replace("{{token}}", request.Token)
        });
        return new Empty();

    }

    public override async Task<Empty> SendLoginNotification(SendLoginNotificationRequest request, ServerCallContext context) {
        var mailTemplate = await File.ReadAllTextAsync(_mailOptions.Templates.Login);
        await _publishEndpoint.Publish<IMailNotificationRequest>(new() {
            Receiver = request.Email,
            Subject = "Login for your account",
            Body = mailTemplate
        });

        if (request.TelegramId.HasValue) {
            var telegramTemplate = await File.ReadAllTextAsync(_telegramOptions.Templates.Login);
            await _publishEndpoint.Publish<ITelegramNotificationRequest>(new() {
                TelegramId = request.TelegramId.Value,
                Message = telegramTemplate
            });
        }

        return new Empty();
    }

    public override async Task<Empty> SendRecoveryPasswordNotification(SendRecoveryPasswordNotificationRequest request, ServerCallContext context) {
        var mailTemplate = await File.ReadAllTextAsync(_mailOptions.Templates.RecoveryPassword);
        await _publishEndpoint.Publish<IMailNotificationRequest>(new() {
            Receiver = request.Email,
            Subject = "Login for your account",
            Body = mailTemplate.Replace("{{token}}", request.Token)
        });

        if (request.TelegramId.HasValue) {
            var telegramTemplate = await File.ReadAllTextAsync(_telegramOptions.Templates.RecoveryPassword);
            await _publishEndpoint.Publish<ITelegramNotificationRequest>(new() {
                TelegramId = request.TelegramId.Value,
                Message = telegramTemplate.Replace("{{token}}", request.Token)
            });
        }

        return new Empty();
    }
}

using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Voltix.NotificationMicroservice.Interfaces.Options;


namespace Voltix.NotificationMicroservice.Services;

public interface IMailNotificationService {
    public Task SendMailAsync(string receiver, string subject, string body);
}

public class MailNotificationService(IOptions<IMailOptions> mailOptions) : IMailNotificationService {
    private readonly IMailOptions _mailOptions = mailOptions.Value;

    public async Task SendMailAsync(string receiver, string subject, string body) {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(string.Empty, _mailOptions.Email));
        message.To.Add(new MailboxAddress(string.Empty, receiver));
        message.Subject = subject;
        message.Body = new TextPart("html") {
            Text = body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(_mailOptions.Host);
        await client.AuthenticateAsync(_mailOptions.Email, _mailOptions.Password);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}

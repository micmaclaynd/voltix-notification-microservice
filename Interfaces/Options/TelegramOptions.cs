namespace Voltix.NotificationMicroservice.Interfaces.Options;

public class ITelegramOptions {
    public class ITemplateOptions {
        public required string ConfirmTelegram { get; set; }
        public required string RecoveryPassword { get; set; }
        public required string Login { get; set; }
    }

    public required string Token { get; set; }
    public required ITemplateOptions Templates { get; set; }
}

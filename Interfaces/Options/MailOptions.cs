namespace Voltix.NotificationMicroservice.Interfaces.Options;

public class IMailOptions {
    public class ITemplateOptions {
        public required string ConfirmEmail { get; set; }
        public required string RecoveryPassword { get; set; }
        public required string Login { get; set; }
    }

    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required ITemplateOptions Templates { get; set; }
}

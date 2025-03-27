namespace Voltix.NotificationMicroservice.Interfaces.Http;

public class IWebNotification {
    public required int Id { get;set; }
    public required string Message { get;set; }
    public required DateTime AddedDateTime { get;set; }
}

public class IGetWebNotificationsResponse {
    public required IEnumerable<IWebNotification> WebNotifications { get; set; }
}
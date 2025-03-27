using Microsoft.AspNetCore.Mvc;
using Voltix.NotificationMicroservice.Interfaces.Http;
using Voltix.NotificationMicroservice.Services;


namespace Voltix.NotificationMicroservice.Controllers;

[Route("web-notifications")]
[ApiController]
public class WebNotificationController(IWebNotificationService webNotificationService) : ControllerBase {
    private readonly IWebNotificationService _webNotificationService = webNotificationService;

    [HttpGet]
    public async Task<ActionResult> GetWebNotificationsAsync([FromHeader(Name = "User-Id")] int userId, bool? isReaded = null) {
        var webNotificationModels = await _webNotificationService.GetWebNotificationsAsync(userId, isReaded);
        return Ok(new IGetWebNotificationsResponse {
            WebNotifications = webNotificationModels.Select(webNotificationModel => new IWebNotification {
                Id = webNotificationModel.Id,
                AddedDateTime = webNotificationModel.AddedDateTime,
                Message = webNotificationModel.Message,
            })
        });
    }
}

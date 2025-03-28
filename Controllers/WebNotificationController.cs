using Microsoft.AspNetCore.Mvc;
using Voltix.NotificationMicroservice.Interfaces.Http;
using Voltix.NotificationMicroservice.Services;
using Voltix.Shared.Interfaces.Http;


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

    [HttpPut("{webNotificationId}")]
    public async Task<ActionResult> ReadWebNotificationAsync([FromHeader(Name = "User-Id")] int userId, int webNotificationId) {
        var webNotificationModel = await _webNotificationService.GetWebNotificationAsync(webNotificationId);
        if (webNotificationModel == null) {
            return NotFound(new IError {
                Message = "Web notification not found"
            });
        }

        if (webNotificationModel.UserId == userId) {
            return StatusCode(StatusCodes.Status403Forbidden, new IError {
                Message = "Web notification does not belong to the user"
            });
        }

        webNotificationModel.IsReaded = true;
        await _webNotificationService.UpdateWebNotificationAsync(webNotificationModel);
        return Ok();
    }

    [HttpDelete("{webNotificationId}")]
    public async Task<ActionResult> RemoveWebNotificationAsync([FromHeader(Name = "User-Id")] int userId, int webNotificationId) {
        var webNotificationModel = await _webNotificationService.GetWebNotificationAsync(webNotificationId);
        if (webNotificationModel == null) {
            return NotFound(new IError {
                Message = "Web notification not found"
            });
        }

        if (webNotificationModel.UserId == userId) {
            return StatusCode(StatusCodes.Status403Forbidden, new IError {
                Message = "Web notification does not belong to the user"
            });
        }

        await _webNotificationService.RemoveWebNotificationAsync(webNotificationModel);
        return Ok();
    }
}

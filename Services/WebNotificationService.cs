using Microsoft.EntityFrameworkCore;
using Voltix.NotificationMicroservice.Contexts;
using Voltix.NotificationMicroservice.Models;

namespace Voltix.NotificationMicroservice.Services;

public interface IWebNotificationService {
    public Task<IEnumerable<WebNotificationModel>> GetWebNotificationsAsync(int userId, bool? isReaded = null);

    public Task AddWebNotificationAsync(WebNotificationModel webNotificationModel);
}

public class WebNotificationService(ApplicationContext context) : IWebNotificationService {
    private readonly ApplicationContext _context = context;

    public async Task<IEnumerable<WebNotificationModel>> GetWebNotificationsAsync(int userId, bool? isReaded = null) {
        var query = _context.WebNotifications.Where(webNotificationModel => webNotificationModel.UserId == userId);

        if (isReaded != null) {
            query = query.Where(webNotificationModel => webNotificationModel.IsReaded == isReaded);
        }

        return await query.ToListAsync();
    }

    public async Task AddWebNotificationAsync(WebNotificationModel webNotificationModel) {
        await _context.WebNotifications.AddAsync(webNotificationModel);
        await _context.SaveChangesAsync();
    }
}

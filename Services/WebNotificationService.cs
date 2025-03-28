using Microsoft.EntityFrameworkCore;
using Voltix.NotificationMicroservice.Contexts;
using Voltix.NotificationMicroservice.Models;

namespace Voltix.NotificationMicroservice.Services;

public interface IWebNotificationService {
    public Task<IEnumerable<WebNotificationModel>> GetWebNotificationsAsync(int userId, bool? isReaded = null);
    public Task<WebNotificationModel?> GetWebNotificationAsync(int id);

    public Task AddWebNotificationAsync(WebNotificationModel webNotificationModel);

    public Task UpdateWebNotificationAsync(WebNotificationModel webNotification);

    public Task RemoveWebNotificationAsync(WebNotificationModel webNotificationModel);
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

    public async Task<WebNotificationModel?> GetWebNotificationAsync(int id) {
        return await _context.WebNotifications.FirstOrDefaultAsync(webNotificationModel => webNotificationModel.Id == id);
    }

    public async Task UpdateWebNotificationAsync(WebNotificationModel webNotification) {
        _context.WebNotifications.Update(webNotification);
        await _context.SaveChangesAsync();
    }

    public async Task AddWebNotificationAsync(WebNotificationModel webNotificationModel) {
        await _context.WebNotifications.AddAsync(webNotificationModel);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveWebNotificationAsync(WebNotificationModel webNotificationModel) {
        _context.WebNotifications.Remove(webNotificationModel);
        await _context.SaveChangesAsync();
    }
}

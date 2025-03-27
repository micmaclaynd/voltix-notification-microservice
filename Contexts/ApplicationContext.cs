using Microsoft.EntityFrameworkCore;
using Voltix.NotificationMicroservice.Models;


namespace Voltix.NotificationMicroservice.Contexts;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options) {
    public required DbSet<WebNotificationModel> WebNotifications { get; set; }
}

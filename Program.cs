using Voltix.Shared.Extensions;
using MassTransit;
using Voltix.NotificationMicroservice.Services;
using Voltix.NotificationMicroservice.Consumers;
using Voltix.NotificationMicroservice.Interfaces.Options;
using Voltix.NotificationMicroservice.Contexts;
using Voltix.NotificationMicroservice.GrpcServices;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ApplicationContext>("voltix-notification-microservice-database");

builder.Services.AddGrpc().AddJsonTranscoding();
builder.Services.AddGrpcSwagger();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.Configure<IMailOptions>(builder.Configuration.GetSection("Mail"));
builder.Services.Configure<ITelegramOptions>(builder.Configuration.GetSection("Telegram"));

builder.Services.AddScoped<IWebNotificationService, WebNotificationService>();
builder.Services.AddScoped<IMailNotificationService, MailNotificationService>();
builder.Services.AddScoped<ITelegramNotificationService, TelegramNotificationService>();

builder.Services.AddMassTransit(options => {
    options.AddConsumer<MailNotificationConsumer>();
    options.AddConsumer<TelegramNotificationConsumer>();

    options.UsingRabbitMq((context, configurator) => {
        configurator.Host(new Uri(builder.Configuration.GetConnectionString("voltix-notification-microservice-queue")!));

        configurator.ReceiveEndpoint("mail-notification-consumer", endpoint => {
            endpoint.ConfigureConsumer<MailNotificationConsumer>(context);
        });

        configurator.ReceiveEndpoint("telegram-notification-consumer", endpoint => {
            endpoint.ConfigureConsumer<TelegramNotificationConsumer>(context);
        });
    });
});

var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGrpcService<NotificationGrpcService>();

app.UpdateDatabase<ApplicationContext>();

app.Run();
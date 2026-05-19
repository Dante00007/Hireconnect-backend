using Microsoft.EntityFrameworkCore;

using HireConnect.Notification.Database;
using HireConnect.Notification.Repository.Interface;
using HireConnect.Notification.Service.Interface;

using HireConnect.Notification.Hubs;
using HireConnect.Notification.Repository.Implementation;
using HireConnect.Notification.Service.Implementation;
using HireConnect.Notification.Consumers;

using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Configuration (Neon PostgreSQL)
// Uses Npgsql to connect to your PostgreSQL instance 
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NotificationConnection")));

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("NotificationConnection")!
    );

// 2
builder.Services
    .AddScoped<
        INotificationRepository,
        NotificationRepository
    >();

builder.Services
    .AddScoped<
        INotificationService,
        NotificationService
    >();


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

// 3. API & Swagger Setup
// Configures OpenAPI 3.0 documentation for testing endpoints 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

var rabbitMqSettings =
    builder.Configuration
        .GetSection("RabbitMQ");

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ApplicationSubmittedConsumer>();
    x.AddConsumer<
       InterviewScheduledConsumer
   >();

    x.AddConsumer<
        InterviewConfirmedConsumer
    >();

    x.AddConsumer<
        InterviewCancelledConsumer
    >();

    x.AddConsumer<
        ApplicationAcceptedConsumer
    >();

    x.AddConsumer<
    InterviewRescheduledConsumer
    >();

    x.AddConsumer<
        InterviewCompletedConsumer
    >();

    x.AddConsumer<
        ApplicationInterviewConsumer
    >();

    x.AddConsumer<
        ApplicationRejectedConsumer
    >();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            rabbitMqSettings["Host"],
            "/",
            h =>
            {
                h.Username(
                    rabbitMqSettings["Username"]!
                );

                h.Password(
                    rabbitMqSettings["Password"]!
                );
            }
        );
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSignalR();

// 4. CORS Policy (For your Angular Frontend)
// Allows your dashboard to communicate with this microservice [cite: 15, 644]

var app = builder.Build();

// 5. Middleware Pipeline


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>(
    "/notificationHub"
);

app.MapHealthChecks("/health");

app.Run();
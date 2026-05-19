using System.Text;

using HireConnect.Application.Database;
using HireConnect.Application.External.Interfaces;
using HireConnect.Application.External.Services;
using HireConnect.Application.Repository.Interface;
using HireConnect.Application.Repository.RepoImplement;
using HireConnect.Application.Service.Interface;
using HireConnect.Application.Service.ServiceImplement;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationConnection"))
    );

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("ApplicationConnection")!
    );

builder.Services.AddScoped<IApplicationRepository, ApplicationRepositoryImpl>();
builder.Services.AddScoped<IApplicationService, ApplicationServiceImpl>();

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

// builder.Services.AddMassTransit(x =>
// {
//     x.UsingRabbitMq((context, cfg) =>
//     {
//         cfg.Host(
//             "localhost",
//             "/",
//             h =>
//             {
//                 h.Username("guest");

//                 h.Password("guest");
//             }
//         );
//     });
// });

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMQ");

builder.Services.AddMassTransit(x =>
{
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
    });
});

builder.Services
    .AddHttpClient<IJobApiClient, JobApiClient>(
        client =>
        {
            client.BaseAddress =
                new Uri(
                    builder.Configuration["Services:JobService"]!
                );
        });

builder.Services
    .AddHttpClient<IProfileApiClient, ProfileApiClient>(
        client =>
        {
            client.BaseAddress =
                new Uri(
                    builder.Configuration["Services:ProfileService"]!
                );
        });


builder.Services.AddOpenApi();
builder.Services.AddControllers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();


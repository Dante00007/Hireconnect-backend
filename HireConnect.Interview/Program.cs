using System.Text;

using HireConnect.Interview.Data;
using HireConnect.Interview.External.Interfaces;
using HireConnect.Interview.External.Services;
using HireConnect.Interview.Repository.Interface;
using HireConnect.Interview.Repository.RepoImplement;
using HireConnect.Interview.Service.Interface;
using HireConnect.Interview.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InterviewDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("InterviewConnection")));

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("InterviewConnection")!
    );

builder.Services.AddScoped<IInterviewRepository, InterviewRepositoryImpl>();
builder.Services.AddScoped<IInterviewService, InterviewServiceImpl>();


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

var rabbitMqSettings =
    builder.Configuration
        .GetSection("RabbitMQ");


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqUri =
            rabbitMqSettings["Uri"];

        if (!string.IsNullOrEmpty(rabbitMqUri))
        {
            cfg.Host(new Uri(rabbitMqUri));
        }
        else
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
        }
    });
});

// builder.Services.AddMassTransit(x =>
// {
//     x.UsingRabbitMq((context, cfg) =>
//     {
//         cfg.Host(
//             rabbitMqSettings["Host"],
//             "/",
//             h =>
//             {
//                 h.Username(
//                     rabbitMqSettings["Username"]!
//                 );

//                 h.Password(
//                     rabbitMqSettings["Password"]!
//                 );
//             }
//         );
//     });
// });

builder.Services.AddHttpClient<IApplicationApiClient, ApplicationApiClient>(
    client =>
        {
            client.BaseAddress =
                new Uri(
                    builder.Configuration["Services:ApplicationService"]!
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
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
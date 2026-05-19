using System.Text;
using HireConnect.Analytic.External.Interfaces;
using HireConnect.Analytic.External.Services;
using HireConnect.Analytic.Service.Interface;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);




// builder.Services.AddDbContext<AnalyticsDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDashboardService, DashboardService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

builder.Services.AddHealthChecks();

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

builder.Services
    .AddHttpClient<IJobApiClient, JobApiClient>(client =>
    {
        client.BaseAddress =
            new Uri(
                builder.Configuration[
                    "Services:JobService"
                ]!
            );
    });
builder.Services
    .AddHttpClient<IApplicationApiClient, ApplicationApiClient>(client =>
    {
        client.BaseAddress =
            new Uri(
                builder.Configuration[
                    "Services:ApplicationService"
                ]!
            );
    });
builder.Services
    .AddHttpClient<IInterviewApiClient, InterviewApiClient>(client =>
    {
        client.BaseAddress =
            new Uri(
                builder.Configuration[
                    "Services:InterviewService"
                ]!
            );
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
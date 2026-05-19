using System.Text;

using HireConnect.Job.Database;
using HireConnect.Job.Repository.Interface;
using HireConnect.Job.Repository.RepoImplement;
using HireConnect.Job.Service.Interface;
using HireConnect.Job.Service.ServiceImplement;

using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;


using HireConnect.Job.External.Services;
using HireConnect.Job.External.Interfaces;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("JobConnection");
builder.Services.AddDbContext<JobDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("JobConnection")!
    );

builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobServiceImpl>();

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

builder.Services
    .AddHttpClient<IProfileApiClient, ProfileApiClient>(
        client =>
        {
            client.BaseAddress =
                new Uri(
                    builder.Configuration["Services:ProfileService"]!
                );
        });


builder.Services.AddControllers();



var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
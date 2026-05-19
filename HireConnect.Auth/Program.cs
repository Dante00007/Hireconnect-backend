
using HireConnect.Auth.Database;
using HireConnect.Auth.Middleware;
using HireConnect.Auth.Repository.Interface;
using HireConnect.Auth.Repository.RepoImplement;
using HireConnect.Auth.Service.Interface;
using HireConnect.Auth.Services.ServiceImplement;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AuthConnection");
 
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services
    .AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("AuthConnection")!
    );


builder.Services.AddScoped<IAuthRepository, AuthRepositoryImpl>();
builder.Services.AddScoped<IAuthService, AuthServiceImpl>();

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
// .AddCookie()
// .AddGitHub(options =>
// {
//     options.ClientId = builder.Configuration["GitHubOAuth:ClientId"]!;
//     options.ClientSecret = builder.Configuration["GitHubOAuth:ClientSecret"]!;
//     options.CallbackPath = "/api/auth/callback/github";
//     options.Scope.Add("user:email");
// });

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<AuthExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
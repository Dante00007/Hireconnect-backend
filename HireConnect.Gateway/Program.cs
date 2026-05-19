var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
             "https://hireconnect-frontend-mlwg.onrender.com"
            ) 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // MECHANICAL NECESSITY for HttpOnly cookies 
    });
});


var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseWebSockets();
app.MapReverseProxy();
app.UseHealthChecks("/health");

app.Run();
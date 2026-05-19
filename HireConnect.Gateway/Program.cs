var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("HireConnectPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Your Angular URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // MECHANICAL NECESSITY for HttpOnly cookies 
    });
});

var app = builder.Build();

app.UseCors("HireConnectPolicy");

app.UseWebSockets();
app.MapReverseProxy();

app.Run();
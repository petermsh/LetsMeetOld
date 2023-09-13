using System.Text.Json.Serialization;
using LetsMeet.Application;
using LetsMeet.Application.Hubs;
using LetsMeet.Application.Middleware;
using LetsMeet.Core;
using LetsMeet.Infrastructure;
using LetsMeet.Infrastructure.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", p =>
        p.WithOrigins("https://localhost:3000")
            .WithOrigins("https://localhost:7120")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true));
});

builder.Services
    .AddApplication()
    .AddCore()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});



var app = builder.Build();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionsMiddleware>();

app.MapHub<ChatHub>("/chat");
app.MapDefaultControllerRoute();
app.UseHttpsRedirection();
app.UseAuth();
app.MapControllers();

app.Run();
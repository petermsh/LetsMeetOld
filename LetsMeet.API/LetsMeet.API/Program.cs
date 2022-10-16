using System.Reflection;
using System.Security.Cryptography;
using AutoMapper;
using LetsMeet.API.Database;
using LetsMeet.API.Hubs;
using LetsMeet.API.Infrastructure;
using LetsMeet.API.Services;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using MapperConfiguration = LetsMeet.API.DTO.MapperConfiguration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddEntityFrameworkSqlServer();
builder.Services.AddControllersWithValidations();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// builder.Services.AddDbContext<DataContext>(
//     o => o.UseNpgsql(builder.Configuration.GetConnectionString("db")));
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddHostedService<DbMigrator>();
builder.Services.AddAutoMapper(typeof(MapperConfiguration).Assembly);
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.RegisterInterfaces();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://letsmeet-api.azurewebsites.net/")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


var authOptions = builder.Configuration.GetOptions<AuthOptions>("Auth");
builder.Services.AddAuth(authOptions);

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.MapHub<ChatHub>("/chatter");
app.UseSwaggerDocs();

app.UseCors();
app.UseHttpsRedirection();
app.UseAuth();
app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.Run();
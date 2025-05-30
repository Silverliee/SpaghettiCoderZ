using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using Microsoft.OpenApi.Models;
using ReservationAPI.Infrastructure.Database;
using ReservationAPI.Middlewares.Security;
using ReservationAPI.Repositories;
using ReservationAPI.Services;

namespace ReservationAPI;

public abstract partial class Program
{
    private static FileVersionInfo GetAssemblyFileVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        return FileVersionInfo.GetVersionInfo(assembly.Location);
    }

    private static readonly string MajorVersionTag = $"v{GetAssemblyFileVersion().FileMajorPart}";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Configuration de scope
        builder.Services.AddSingleton(new ConcurrentDictionary<string, WebSocket>());
        builder.Services.AddSingleton<SqLiteDbContext>(s => new SqLiteDbContext(builder.Configuration));
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IParkingRepository, ParkingRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IParkingService, ParkingService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<IStatisticService, StatisticService>();
        builder.Services.AddSingleton<ICryptographer, Cryptographer>();
        builder.Services.AddDbContext<SqLiteDbContext>();
        
        // Configuration des controllers/endpoints
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        // Configuration de CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                policyBuilder => { policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }
            );
        });
        // Configuration de swagger
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                MajorVersionTag,
                new OpenApiInfo
                {
                    Title = "Reservation API",
                    Version = "V1.0.0",
                    Description =
                        "API de gestion des réservations pour les hôtels, restaurants et autres services.",
                    Contact = new OpenApiContact
                    {
                        Name = "SpaghettiCoderZ",
                        Email = "mohamedstrore@hotmail.fr",
                        Url = new Uri("https://github.com/Silverliee")
                    }
                }
            );
        });

        // Configuration de la base de données SQLite
        var app = builder.Build();
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();
            db.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        // configure swagger
        app.UseSwagger();
        app.UseSwaggerUI();
        // map controllers and run the app
        app.MapControllers();
        app.Run();
    }
}
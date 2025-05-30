using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReservationAPI.Infrastructure.Database;
using ReservationAPI.Middlewares.Authentication;
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
        // Configuration de l'authentification JWT
        builder.Services.AddSingleton<AuthenticationMiddleware>();
        // Configuration du health check
        builder.Services.AddHealthChecks();
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

            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Description = "Tu peux mettre ton token ici ;) PS: ajoute Bearer suivie d'un espace avant le token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                }
            );

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[] { }
                }
            });
        });

        // Configuration de JWT
        builder.Services.AddAuthorization();
        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
               options.RequireHttpsMetadata = false;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                   ValidIssuer = builder.Configuration["Jwt:Issuer"],
                   ValidAudience = builder.Configuration["Jwt:Audience"],
                   ClockSkew = TimeSpan.Zero
               };
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
        // configure authentication
        app.UseAuthentication();
        app.UseAuthorization();
        // map controllers and run the app
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.Run();
    }
}
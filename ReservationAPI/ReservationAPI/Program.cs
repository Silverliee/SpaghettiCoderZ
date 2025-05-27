using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Reflection;
using Microsoft.OpenApi.Models;

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
        // Configuration des controllers/endpoints
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        // Configuration du health check
        builder.Services.AddHealthChecks();
        // Configuration de la base de données
        //builder.Services.AddDbContext<MySqlDbContext>(options =>
        //{
        //    options.UseSqlServer(builder.Configuration.GetConnectionString("database"));
        //});

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
                "JWT-BEARER-TOKEN",
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
                            Id = "JWT-BEARER-TOKEN"
                        }
                    },
                    new string[] { }
                }
            });
        });

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        // configure swagger
        app.UseSwagger();
        app.UseSwaggerUI();
        // configure authentication
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseWebSockets();
        // map controllers and run the app
        app.MapControllers();
        app.MapHealthChecks("/health");
        app.Run();
    }
}

public abstract partial class Program
{
}
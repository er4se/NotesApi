using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NotesApi.Application.Common;
using NotesApi.Application.DTO;
using NotesApi.Application.Interfaces;
using NotesApi.Controllers;
using NotesApi.Domain.Models;
using NotesApi.Extensions;
using NotesApi.Infrastructure.Data;
using NotesApi.Infrastructure.Identity;
using NotesApi.Infrastructure.Repository;
using NotesApi.Infrastructure.Services;
using NotesApi.Middleware;
using NotesApi.Web.Extensions;
using Serilog;
using Serilog.Enrichers.CorrelationId;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using RabbitMQ.Client;

namespace NotesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting NotesApi application");

                var builder = WebApplication.CreateBuilder(args);

                LoggingExtensions.ConfigureSerilog(builder);
                builder.Host.UseSerilog();

                builder.Services.AddHealthChecks()
                    .AddCheck("self", () => HealthCheckResult.Healthy())
                    .AddDbContextCheck<AppDbContext>("notes_postgres")
                    .AddRedis(
                        builder.Configuration.GetConnectionString("Redis")!,
                        name: "notes_redis")
                    .AddRabbitMQ(
                        factory: async sp =>
                        {
                            var config = sp.GetRequiredService<IConfiguration>();
                            var factory = new ConnectionFactory
                            {
                                HostName = config["RabbitMQ:Host"]!,
                                Port = config.GetValue<int>("RabbitMQ:Port"),
                                UserName = config["RabbitMQ:Username"]!,
                                Password = config["RabbitMQ:Password"]!
                            };
                            return await factory.CreateConnectionAsync();
                        },
                        name: "rabbitmq"
                    );

                builder.Services.AddDatabase(builder.Configuration);
                builder.Services.AddCaching(builder.Configuration);

                builder.Services.AddAuthenticationServices(builder.Configuration);

                builder.Services.AddApiServices();
                builder.Services.AddApplicationLayer();

                builder.Services.AddSwaggerDocumentation();

                var app = builder.Build();

                app.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                app.ApplyMigrations();
                app.ConfigureMiddleware();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}

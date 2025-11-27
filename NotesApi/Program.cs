using Mapster;
using Serilog;
using Serilog.Enrichers.CorrelationId;
using Microsoft.EntityFrameworkCore;
using NotesApi.Controllers;
using NotesApi.Infrastructure.Data;
using NotesApi.Application.DTO;
using NotesApi.Domain.Models;
using NotesApi.Application.Repository;
using NotesApi.Infrastructure.Repository;
using NotesApi.Application.Common;
using NotesApi.Middleware;
using System.Globalization;

namespace NotesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] ({CorrelationId}) {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logs/api-.log",
                    rollingInterval: RollingInterval.Day,
                    shared: true,
                    formatProvider: CultureInfo.InvariantCulture,
                    fileSizeLimitBytes: 100_000_000,
                    retainedFileCountLimit: 30)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions =>
                    {
                        // ЯВНО указываем сборку с миграциями
                        npgsqlOptions.MigrationsAssembly("NotesApi.Infrastructure");

                        // Также добавь retry для устойчивости
                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorCodesToAdd: null
                        );
                    }
                );
            });

            builder.Services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplication();
            MappingConfig.RegisterMappings();
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            app.UseExceptionHandler();
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "Handled {RequestPath} {RequestMethod} responded {StatusCode} in {Elapsed:0.0000}ms (CorrelationId: {CorrelationId})";
            });

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("=== Migration Debug Info ===");
                    logger.LogInformation($"Can connect: {db.Database.CanConnect()}");
                    logger.LogInformation($"DbContext assembly: {db.GetType().Assembly.FullName}");
                    logger.LogInformation($"DbContext location: {db.GetType().Assembly.Location}");

                    // Проверка найденных миграций
                    var migrations = db.Database.GetPendingMigrations();
                    logger.LogInformation($"Pending migrations count: {migrations.Count()}");
                    foreach (var migration in migrations)
                    {
                        logger.LogInformation($"  - {migration}");
                    }

                    var appliedMigrations = db.Database.GetAppliedMigrations();
                    logger.LogInformation($"Applied migrations count: {appliedMigrations.Count()}");
                    foreach (var migration in appliedMigrations)
                    {
                        logger.LogInformation($"  - {migration}");
                    }

                    logger.LogInformation("Attempting to migrate...");
                    db.Database.Migrate();
                    logger.LogInformation("Migration completed successfully");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Migration failed");
                    throw;
                }
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}

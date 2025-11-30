using Microsoft.EntityFrameworkCore;
using NotesApi.Infrastructure.Data;

namespace NotesApi.Extensions
{
    public static class DatabaseExtensions
    {
        public static void AddDatabase(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly("NotesApi.Infrastructure");

                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorCodesToAdd: null
                        );
                    }
                );
            });
        }

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Starting database migration");

                if (logger.IsEnabled(LogLevel.Debug))
                {
                    LogMigrationDebugInfo(db, logger);
                }

                db.Database.Migrate();
                logger.LogInformation("Database migration completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database migration failed");
                throw;
            }
        }

        public static void LogMigrationDebugInfo(AppDbContext db, ILogger logger)
        {
            logger.LogDebug("=== Migration Debug Info ===");
            logger.LogDebug("Can connect: {CanConnect}", db.Database.CanConnect());

            var pending = db.Database.GetPendingMigrations().ToList();
            logger.LogDebug("Pending migrations: {Count}", pending.Count);
            pending.ForEach(m => logger.LogDebug("  - {Migration}", m));

            var applied = db.Database.GetAppliedMigrations().ToList();
            logger.LogDebug("Applied migrations: {Count}", applied.Count);
            applied.ForEach(m => logger.LogDebug("  - {Migration}", m));
        }
    }
}

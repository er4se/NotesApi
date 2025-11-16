using Mapster;
using Microsoft.EntityFrameworkCore;
using NotesApi.Controllers;
using NotesApi.Infrastructure.Data;
using NotesApi.Application.DTO;
using NotesApi.Domain.Models;
using NotesApi.Application.Repository;
using NotesApi.Infrastructure.Repository;
using NotesApi.Application.Common;

namespace NotesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            var app = builder.Build();

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

using Mapster;
using Microsoft.EntityFrameworkCore;
using NotesApi.Application.Common;
using NotesApi.Application.DTO;
using NotesApi.Application.Repository;
using NotesApi.Controllers;
using NotesApi.Domain.Models;
using NotesApi.Extensions;
using NotesApi.Infrastructure.Data;
using NotesApi.Infrastructure.Repository;
using NotesApi.Middleware;
using Serilog;
using Serilog.Enrichers.CorrelationId;
using System.Globalization;

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
                builder.Services.AddDatabase(builder.Configuration);
                builder.Services.AddApiServices();
                builder.Services.AddApplicationLayer();

                builder.Services.AddSwaggerGen();

                var app = builder.Build();

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

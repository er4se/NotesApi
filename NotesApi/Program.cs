using Mapster;
using Microsoft.EntityFrameworkCore;
using NotesApi.Controllers;
using NotesApi.Infrastructure.Data;
using NotesApi.Application.DTO;
using NotesApi.Domain.Models;
using NotesApi.Application.Services;
using NotesApi.Application.Repository;
using NotesApi.Infrastructure.Repository;

namespace NotesApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<INoteRepository, NoteRepository>();
            builder.Services.AddScoped<INotesService, NotesService>();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            TypeAdapterConfig<Note, NoteDto>.NewConfig();
            TypeAdapterConfig<NoteCreateDto, Note>.NewConfig();
            TypeAdapterConfig<NoteUpdateDto, Note>.NewConfig();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}

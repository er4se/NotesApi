using Mapster;
using NotesApi.Application.Common;
using NotesApi.Application.Repository;
using NotesApi.Infrastructure.Repository;
using NotesApi.Middleware;

namespace NotesApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddControllers();

            services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            services.AddTransient<CorrelationIdentifierHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }

        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddApplication();
            MappingConfig.RegisterMappings();
            services.AddScoped<INoteRepository, NoteRepository>();
        }
    }
}

using Mapster;
using NotesApi.Application.Common;
using NotesApi.Application.Interfaces;
using NotesApi.Infrastructure.Repository;
using NotesApi.Infrastructure.Services;
using NotesApi.Middleware;
using NotesApi.Web.Handlers;

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

            services.AddTransient<CorrelationHeaderHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }

        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddApplication();
            MappingConfig.RegisterMappings();
            services.AddScoped<INoteRepository, NoteRepository>();

            services.AddHttpContextAccessor();
        }
    }
}

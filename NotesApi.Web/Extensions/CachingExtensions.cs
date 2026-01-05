using NotesApi.Application.Interfaces;
using NotesApi.Infrastructure.Services;

namespace NotesApi.Web.Extensions
{
    public static class CachingExtensions
    {
        public static void AddCaching(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            services.AddScoped<ICacheService, RedisCacheService>();
        }
    }
}

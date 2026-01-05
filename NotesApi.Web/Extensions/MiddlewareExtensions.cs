using NotesApi.Middleware;
using Serilog;

namespace NotesApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            app.UseMiddleware<CorrelationIdentifierHandler>();
            app.UseExceptionHandler();

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "Handled {RequestPath} {RequestMethod} responded {StatusCode} in {Elapsed:0.0000}ms (CorrelationId: {CorrelationId})";

                options.GetLevel = (httpContext, elapsed, ex) =>
                {
                    if (httpContext.Request.Path.StartsWithSegments("/health"))
                        return Serilog.Events.LogEventLevel.Verbose;

                    return ex != null
                        ? Serilog.Events.LogEventLevel.Error
                        : Serilog.Events.LogEventLevel.Information;
                };
            });

            app.UseRouting();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}

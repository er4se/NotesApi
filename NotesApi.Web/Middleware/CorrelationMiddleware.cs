using NotesApi.Infrastructure.Services;
using Serilog.Events;
using Serilog.Context;
using NotesApi.Application.Common.Context;

namespace NotesApi.Middleware
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationIdHeader = context.Request.Headers["CorrelationId"];
            if (!string.IsNullOrWhiteSpace(correlationIdHeader))
            {
                var correlationId = Guid.Parse(correlationIdHeader.ToString());
                Serilog.Context.LogContext.PushProperty("CorrelationId", new ScalarValue(correlationId));
                AsyncStorage<Correlation>.Store(new Correlation
                {
                    Id = correlationId
                });
            }

            await _next(context);
        }
    }
}

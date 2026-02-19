using NotesApi.Infrastructure.Consumers;
using NotesApi.Infrastructure.Services;
using Serilog.Context;
using Serilog.Events;

namespace NotesApi.Middleware
{
    public class CorrelationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationMiddleware> _logger;

        public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationIdHeader = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(correlationIdHeader)) correlationIdHeader = null;
            var correlationId = correlationIdHeader ?? Guid.NewGuid().ToString();

            context.Response.Headers["X-Correlation-ID"] = correlationId;
            context.Items["CorrelationId"] = correlationId;
            context.TraceIdentifier = correlationId;

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", Guid.Parse(correlationId)))
            {
                await _next(context);
            }
        }
    }
}

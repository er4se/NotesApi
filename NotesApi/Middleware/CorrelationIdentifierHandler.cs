namespace NotesApi.Middleware
{
    public class CorrelationIdentifierHandler : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            var incoming = httpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault();
            if (string.IsNullOrEmpty(incoming)) incoming = null;

            var correlationId = incoming ?? Guid.NewGuid().ToString();

            httpContext.Response.Headers["X-Correlation-ID"] = correlationId;
            httpContext.Items["CorrelationId"] = correlationId;
            httpContext.TraceIdentifier = correlationId;

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
            {
                await next(httpContext);
            }
        }
    }
}

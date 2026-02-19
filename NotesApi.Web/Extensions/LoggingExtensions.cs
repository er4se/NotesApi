using Serilog;
using Serilog.Enrichers.CorrelationId;
using System.Globalization;
using Serilog.Expressions;
using Serilog.Templates;

namespace NotesApi.Extensions
{
    public static class LoggingExtensions
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            var isDevelopment = builder.Environment.IsDevelopment();

            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information();

            if (isDevelopment)
            {
                loggerConfig
                    .MinimumLevel.Debug()
                    .WriteTo.Console(new ExpressionTemplate(
                        // время и уровень
                        "[{@t:HH:mm:ss} {@l:u3}]" +
                        // CorrelationId только если есть
                        "{#if CorrelationId is not null} ({CorrelationId}){#end} " +
                        // само сообщение
                        "{@m}{@x}\n"
                    ));
                //.WriteTo.Console(outputTemplate:
                //    "[{Timestamp:HH:mm:ss} {Level:u3}] ({CorrelationId}) {Message:lj}{NewLine}{Exception}");
            }
            else
            {
                loggerConfig
                    .WriteTo.Console(outputTemplate:
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level}] {Message}{NewLine}{Exception}");
            }

            loggerConfig.WriteTo.File("logs/api-.log",
                rollingInterval: RollingInterval.Day,
                shared: true,
                formatProvider: CultureInfo.InvariantCulture,
                fileSizeLimitBytes: 100_000_000,
                retainedFileCountLimit: isDevelopment ? 7 : 30);

            Log.Logger = loggerConfig.CreateLogger();
        }
    }
}

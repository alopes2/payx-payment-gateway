using System;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PayX.Api.Extensions
{
    public static class LoggingExtensions
    {
        public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
        {
            builder
                .UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        var filePath = hostingContext.Configuration
                            .GetSection("Serilog:FilePath")
                            .Value;

                        loggerConfiguration
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext()
                            .WriteTo.Console()
                            .WriteTo.File(filePath,
                                fileSizeLimitBytes: 1_000_000,
                                rollOnFileSizeLimit: true,
                                rollingInterval: RollingInterval.Day,
                                shared: true,
                                flushToDiskInterval: TimeSpan.FromSeconds(1)
                            );
                    }
                );

            return builder;
        }
    }
}
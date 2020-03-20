using Microsoft.AspNetCore.Builder;
using PayX.Api.Middlewares;
using Prometheus;

namespace PayX.Api.Extensions
{
    public static class MetricsExtensions
    {
        /// <summary>
        /// Initialize Metrics middleware and Prometheus HttpMetrics middleware.
        /// </summary>
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder app)
        {
            app.UseHttpMetrics();
            app.UseMiddleware<MetricsMiddleware>();

            return app;
        }
    }
}
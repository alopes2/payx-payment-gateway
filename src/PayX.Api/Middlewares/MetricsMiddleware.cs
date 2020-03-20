using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PayX.Api.Metrics;

namespace PayX.Api.Middlewares
{
    public class MetricsMiddleware
    {
        private readonly IMetricsService _metricsService;

        private readonly RequestDelegate _next;

        public MetricsMiddleware(RequestDelegate next, IMetricsService metricsService)
        {
            _next = next;
            _metricsService = metricsService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(Exception e)
            {
                _metricsService.ExceptionsCounter.Inc();
                throw e;
            }
        }
    }
}
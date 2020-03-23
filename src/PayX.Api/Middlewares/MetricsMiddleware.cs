using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PayX.Api.Metrics;
using PayX.Core.Exceptions;

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
                // Should only log unhandled and impredicted exceptions
                if (e.GetType() != typeof(HttpResponseException))
                {
                    _metricsService.UnhandledExceptionsCounter.Inc();
                }
                
                throw e;
            }
        }
    }
}
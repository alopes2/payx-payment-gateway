using Microsoft.AspNetCore.Builder;
using PayX.Api.Middlewares;

namespace PayX.Api.Extensions
{
    public static class ExceptionHandlerMiddleware
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<Middlewares.ExceptionHandlerMiddleware>();

            return app;
        }
    }
}
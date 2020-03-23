using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PayX.Api.Models;
using PayX.Core.Exceptions;
using Serilog;

namespace PayX.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception thrown");
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var details = new ErrorDetails()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = $"Exception message: {exception.Message}.",
                Title = "An unexpected error occurred."
            };

            if (exception.InnerException != null)
            {
                details.Message += $" Inner message: {exception.InnerException.Message}";
            }

            if (exception is HttpResponseException httpResponseException)
            {
                details.StatusCode = httpResponseException.StatusCode;
                details.Message = httpResponseException.Message;
                details.Title = httpResponseException.Title;
            }

            await WriteResponse(context, details);
        }

        private static async Task WriteResponse(HttpContext context, ErrorDetails details)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)details.StatusCode;
            await context.Response.WriteAsync(details.ToString());
        }
    }
}
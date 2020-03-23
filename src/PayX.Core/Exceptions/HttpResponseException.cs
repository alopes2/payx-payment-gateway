using System;
using System.Net;

namespace PayX.Core.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

        public string Title { get; set; } = "An unexpected error occurred";

        public HttpResponseException(string message)
            : base(message)
        { }

        public HttpResponseException NotFound(string title = "Data not found")
        {
            Title = title;
            StatusCode = HttpStatusCode.NotFound;
            return this;
        }

        public HttpResponseException BadRequest(string title = "Request has invalid data")
        {
            Title = title;
            StatusCode = HttpStatusCode.BadRequest;
            return this;
        }

        public HttpResponseException Forbidden(string title = "Not enough access")
        {
            Title = title;
            StatusCode = HttpStatusCode.Forbidden;
            return this;
        }

        public HttpResponseException Unauthorized(string title = "Unauthorized")
        {
            Title = title;
            StatusCode = HttpStatusCode.Unauthorized;
            return this;
        }
    }
}
using System.Net;
using System.Text.Json;

namespace PayX.Api.Models
{
    public class ErrorDetails
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
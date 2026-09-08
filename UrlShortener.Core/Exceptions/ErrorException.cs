using System.Net;

namespace UrlShortener.Core.Exceptions
{
    public class ErrorException : Exception
    {
        public HttpStatusCode Code { get; }

        public ErrorException(HttpStatusCode code, string message) : base(message)
        {
            Code = code;
        }

        public static ErrorException BadRequest(string message) => new(HttpStatusCode.BadRequest, message);
        public static ErrorException Unauthorized(string message) => new(HttpStatusCode.Unauthorized, message);
        public static ErrorException NotFound(string message) => new(HttpStatusCode.NotFound, message);
        public static ErrorException AlreadyExists(string message) => new(HttpStatusCode.Conflict, message);
    }
}
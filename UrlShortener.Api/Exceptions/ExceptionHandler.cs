using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Exceptions;

namespace UrlShortener.Api.Exceptions
{
    internal class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not ErrorException problemException)
            {
                return false;
            }

            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = (int)problemException.Code,
                Title = problemException.Code.ToString(),
                Detail = problemException.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };

            httpContext.Response.StatusCode = problemDetails.Status ?? 500;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
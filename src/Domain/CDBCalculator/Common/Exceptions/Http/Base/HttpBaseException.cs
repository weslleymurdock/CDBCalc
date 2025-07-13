using Microsoft.AspNetCore.Http;

namespace Domain.Common.Exceptions.Http.Base;

public class HttpBaseException : Exception
{
    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;
    public HttpBaseException() : base("An internal server error occurred")
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
    public HttpBaseException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode; 
    }
}

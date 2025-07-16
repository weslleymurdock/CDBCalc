using Microsoft.AspNetCore.Http;

namespace Domain.Common.Exceptions.Http.Base;

/// <summary>
/// Represents an exception that occurs during HTTP operations, providing additional context about the HTTP status code.
/// </summary>
/// <remarks>This exception is intended to encapsulate errors related to HTTP requests or responses,  including
/// the associated status code for easier debugging and error handling.</remarks>
public class HttpBaseException : Exception
{
    public int StatusCode { get; set; } 
    
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpBaseException"/> class with a default error message.
    /// </summary>
    /// <remarks>This exception represents an internal server error and is associated with the HTTP status
    /// code 500.</remarks>
    public HttpBaseException() : base("An internal server error occurred")
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpBaseException"/> class with a specified error message and HTTP
    /// status code.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception.</param>
    public HttpBaseException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode; 
    }
}

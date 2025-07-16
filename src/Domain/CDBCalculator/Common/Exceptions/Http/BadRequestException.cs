using Domain.Common.Exceptions.Http.Base;
using Microsoft.AspNetCore.Http;

namespace Domain.Common.Exceptions.Http;

/// <summary>
/// Represents an exception that is thrown when a bad request is encountered.
/// </summary>
/// <remarks>This exception is typically used to indicate that the client has made an invalid request, such as
/// providing incorrect parameters or violating expected constraints. It is associated with the HTTP 400 Bad Request
/// status code.</remarks>
/// <param name="message"></param>
public class BadRequestException(string message = "Bad Request") : HttpBaseException(message, StatusCodes.Status400BadRequest)
{
}

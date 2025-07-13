using Domain.Common.Exceptions.Http.Base;
using Microsoft.AspNetCore.Http;

namespace Domain.Common.Exceptions.Http;

public class BadRequestException(string message = "Bad Request") : HttpBaseException(message, StatusCodes.Status400BadRequest)
{
}

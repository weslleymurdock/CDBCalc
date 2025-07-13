using Application.CQRS.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CDBCalculator.Controllers;

[ApiController]
[Route("[controller]")]
public class CDBController(ILogger<CDBController> logger, IMediator mediator) : ControllerBase
{
    [HttpPost("Simulate")]
    public async Task<IActionResult> Simulate([FromForm(Name = "Months")] uint months, [FromForm(Name = "InitialValue")] double initial)
    {
        try
        {
            var request = new SimulateCDBCommand() { InitialValue = initial, Months = months };
            var response = await mediator.Send(request);
            return response?.StatusCode switch
            {
                200 => Ok(response),
                400 => BadRequest(response),    
                422 => UnprocessableEntity(response),
                500 => new ContentResult() { Content = response!.Message, ContentType = "text/plain", StatusCode = 500 },
                _ => new ContentResult() { Content = response!.Message, ContentType = "text/plain", StatusCode = response!.StatusCode }
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return new ContentResult() { Content = e.Message, ContentType = "text/plain", StatusCode = 500 };
        }
    }
}

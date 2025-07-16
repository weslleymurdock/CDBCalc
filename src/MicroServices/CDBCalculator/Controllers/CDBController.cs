using Application.CQRS.Requests.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CDBCalculator.Controllers;

[ApiController]
[Route("[controller]")]
public class CdbController(ILogger<CdbController> logger, IMediator mediator) : ControllerBase
{
    [HttpPost("Simulate")]
    public async Task<IActionResult> Simulate([FromForm(Name = "Months")] uint months, [FromForm(Name = "InitialValue")] decimal initial)
    {
        try
        {
            logger.LogInformation("Simulando cdb com valor de {Initial} em {Months} meses", initial, months);
            var request = new SimulateCdbCommand() { InitialValue = Convert.ToDouble(initial), Months = months };
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
            logger.LogError(e, "Houve um erro: {Message}", e.Message);
            return new ContentResult() { Content = e.Message, ContentType = "text/plain", StatusCode = 500 };
        }
    }
}

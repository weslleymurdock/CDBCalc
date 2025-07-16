using Application.CQRS.Requests.Commands;
using Application.CQRS.Responses.CDBCalculator;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CDBCalculator.Controllers;

/// <summary>
/// Provides endpoints for simulating a CDB (Certificate of Deposit) investment.
/// </summary>
/// <remarks>This controller handles requests related to CDB simulations, including calculating the projected
/// value  of an investment over a specified number of months. It uses dependency injection to access logging 
/// functionality and the mediator pattern for handling commands.</remarks>
/// <param name="logger">The logger for controller</param>
/// <param name="mediator">The mediator instance for send commands</param>
[ApiController]
[Route("[controller]")]
public class CdbController(ILogger<CdbController> logger, IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Simulates the performance of a CDB (Certificate of Deposit) investment over a specified number of months.
    /// </summary>
    /// <remarks>This method calculates the projected returns of a CDB investment based on the initial value
    /// and the investment duration. The simulation result is returned as a <see cref="SimulateCdbResponse"/> object,
    /// which includes details about the calculated returns. The method supports multiple HTTP response codes to
    /// indicate the outcome of the operation: <list type="bullet"> <item><description><see
    /// cref="StatusCodes.Status200OK"/>: The simulation was successful, and the result is
    /// returned.</description></item> <item><description><see cref="StatusCodes.Status400BadRequest"/>: The request was
    /// invalid, typically due to incorrect input parameters.</description></item> <item><description><see
    /// cref="StatusCodes.Status422UnprocessableEntity"/>: The simulation could not be processed due to business logic
    /// constraints.</description></item> <item><description><see cref="StatusCodes.Status500InternalServerError"/>: An
    /// unexpected error occurred during processing.</description></item> </list></remarks>
    /// <param name="months">The number of months for which the investment simulation is performed. Must be a positive integer.</param>
    /// <param name="initial">The initial investment amount in decimal format. Must be a positive value.</param>
    /// <returns>An <see cref="IActionResult"/> containing the simulation result or an error message. On success, the result is
    /// encapsulated in a <see cref="SimulateCdbResponse"/> object.</returns>
    [HttpPost("Simulate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SimulateCdbResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(SimulateCdbResponse))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(SimulateCdbResponse))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ContentResult))]
    public async Task<IActionResult> Simulate([FromForm(Name = "Months")] uint months, [FromForm(Name = "InitialValue")] decimal initial)
    {
        try
        {

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

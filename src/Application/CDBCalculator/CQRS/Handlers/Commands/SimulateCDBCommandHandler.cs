using Application.CQRS.Requests.Commands;
using Application.CQRS.Responses.CDBCalculator;
using Domain.Common.Exceptions.CDBCalculator;
using Infrastructure.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Handlers.Commands;

public class SimulateCDBCommandHandler(ILogger<SimulateCDBCommandHandler> logger, IValidator<SimulateCDBCommand> validator, ICDBCalculator simulator) : IRequestHandler<SimulateCDBCommand, SimulateCDBResponse>
{
    public async Task<SimulateCDBResponse> Handle(SimulateCDBCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"{nameof(SimulateCDBCommandHandler)} started");
            var validation = await validator.ValidateAsync(request, cancellationToken); 
            if (!validation.IsValid)
            {
                logger.LogWarning("Validation failure");
                return new SimulateCDBResponse() { Message = string.Join(", ", validation.Errors), StatusCode = 422, Success = false };
            }
            (double totalGross, double totalNet) = simulator.Calculate(new Domain.Business.Records.CDB(request.InitialValue, request.Months));
            var response = new SimulateCDBResponse() { Gross = totalGross, Net = totalNet, Message = "Ok", StatusCode = 200, Success = true };

            return response;
        }
        catch (CDBException e)
        {
            logger.LogError(e, e.Message);
            return new SimulateCDBResponse { Message  = e.Message, StatusCode = 400, Success = false };
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return new SimulateCDBResponse { Message  = e.Message, StatusCode = 500, Success = false };
        }
        finally
        {
            logger.LogInformation($"{nameof(SimulateCDBCommandHandler)} finished");
        }
    }
}

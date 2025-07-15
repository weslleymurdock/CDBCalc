using Application.CQRS.Requests.Commands;
using Application.CQRS.Responses.CDBCalculator;
using Domain.Common.Exceptions.CDBCalculator;
using Infrastructure.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Handlers.Commands;

public class SimulateCdbCommandHandler(ILogger<SimulateCdbCommandHandler> logger, IValidator<SimulateCdbCommand> validator, ICdbCalculator simulator) : IRequestHandler<SimulateCdbCommand, SimulateCdbResponse>
{
    public async Task<SimulateCdbResponse> Handle(SimulateCdbCommand request, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"{nameof(SimulateCdbCommandHandler)} started");
            var validation = await validator.ValidateAsync(request, cancellationToken); 
            if (!validation.IsValid)
            {
                logger.LogWarning("Validation failure");
                return new SimulateCdbResponse() { Message = string.Join(", ", validation.Errors), StatusCode = 422, Success = false };
            }
            (double totalGross, double totalNet) = simulator.Calculate(new Domain.Business.Records.Cdb(request.InitialValue, request.Months));
            var response = new SimulateCdbResponse() { Gross = totalGross, Net = totalNet, Message = "Ok", StatusCode = 200, Success = true };

            return response;
        }
        catch (CdbException e)
        {
            logger.LogError(e, "Houve um erro ao calcular o Cdb: {Message}",e.Message);
            return new SimulateCdbResponse { Message  = e.Message, StatusCode = 400, Success = false };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Houve um erro: {Message}", e.Message);
            return new SimulateCdbResponse { Message  = e.Message, StatusCode = 500, Success = false };
        }
        finally
        {
            logger.LogInformation($"{nameof(SimulateCdbCommandHandler)} finished");
        }
    }
}

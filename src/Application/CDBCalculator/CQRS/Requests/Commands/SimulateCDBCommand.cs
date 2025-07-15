using Application.CQRS.Responses.CDBCalculator;
using MediatR;

namespace Application.CQRS.Requests.Commands;

public class SimulateCdbCommand : IRequest<SimulateCdbResponse>
{
    public uint Months { get; set; } = 1;
    public double InitialValue { get; set; } = 0.00;
}

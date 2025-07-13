using Application.CQRS.Responses.CDBCalculator;
using MediatR;

namespace Application.CQRS.Requests.Commands;

public class SimulateCDBCommand : IRequest<SimulateCDBResponse>
{
    public uint Months { get; set; } = 1;
    public double InitialValue { get; set; } = 0.00;
}

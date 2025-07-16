using Application.CQRS.Responses.CDBCalculator;
using MediatR;

namespace Application.CQRS.Requests.Commands;

/// <summary>
/// Represents a command to simulate a Certificate of Deposit Bond (CDB) calculation.
/// </summary>
/// <remarks>This command is used to calculate the projected value of a CDB investment over a specified number of
/// months, given an initial investment amount. The simulation results are returned as a <see
/// cref="SimulateCdbResponse"/>.</remarks>
public class SimulateCdbCommand : IRequest<SimulateCdbResponse>
{
    public uint Months { get; set; } = 1;
    public double InitialValue { get; set; } = 0.00;
}

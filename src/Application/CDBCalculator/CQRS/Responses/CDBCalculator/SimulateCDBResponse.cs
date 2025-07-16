namespace Application.CQRS.Responses.CDBCalculator;

/// <summary>
/// Represents the response of a simulated CDB (Certificate of Deposit) calculation.
/// </summary>
/// <remarks>This class provides the gross and net values resulting from the simulation. These values can be used
/// to analyze the financial outcome of the simulated investment.</remarks>
public class SimulateCdbResponse : Base.Response
{
    public double Gross {  get; set; } 
    public double Net {  get; set; } 
}

using Domain.Business.Records;

namespace Infrastructure.Common.Interfaces;

/// <summary>
/// Defines a contract for calculating the gross and net values of a CDB (Certificate of Deposit) investment.
/// </summary>
/// <remarks>This interface provides a method to calculate the total gross and net values for a given CDB
/// investment. Implementations of this interface should ensure that the calculation adheres to the specific rules and
/// formulas applicable to the CDB investment type.</remarks>
public interface ICdbCalculator
{
    /// <summary>
    /// Calculates the total gross and net values for the specified title.
    /// </summary>
    /// <param name="title">The title for which the calculations are performed. Cannot be null.</param>
    /// <returns>A tuple containing the total gross and total net values: <list type="bullet">
    /// <item><description><c>totalGross</c>: The total gross value.</description></item>
    /// <item><description><c>totalNet</c>: The total net value.</description></item> </list></returns>
    public (double totalGross, double totalNet) Calculate(Cdb title);
}

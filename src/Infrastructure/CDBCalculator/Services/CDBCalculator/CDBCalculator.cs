using Domain.Common.Exceptions.CDBCalculator;
using Domain.Business.Records;
using Infrastructure.Common.Interfaces;

namespace Infrastructure.Services.CDBCalculator;

/// <summary>
/// Provides methods to calculate the gross and net values of a CDB (Certificate of Deposit) investment.
/// </summary>
public class CdbCalculator : ICdbCalculator
{

    /// <summary>
    /// Calculates the total gross and net values for the specified CDB title.
    /// </summary>
    /// <param name="title"></param>
    /// <returns></returns>
    /// <exception cref="CdbException"></exception>
    public (double totalGross, double totalNet) Calculate(Cdb title)
    {
        if (title.Months < 1)
            throw new CdbException("The months of the Cdb title are less than zero (0),");

        double grossValue = Gross(title.VI, title.Months);
        double tax = Taxes(title.Months, grossValue, title.VI);
        double netValue = grossValue - tax;
        if (!double.IsFinite(grossValue))
        {
            throw new CdbException("The value generated for grossValue is not a finite number");
        }

        if (!double.IsFinite(netValue))
        {
            throw new CdbException("The value generated for netValue is not a finite number");
        }

        return (totalGross: Math.Round(grossValue, 2), totalNet: Math.Round(netValue, 2));
    }
   
    /// <summary>
    /// Calculates the gross value of a CDB investment over a specified number of months,
    /// based on compound interest using the formula:
    /// VF = VI × [1 + (CDI × TB)] ^ months,
    /// where:
    /// - VF is the final value;
    /// - VI is the initial investment;
    /// - CDI is the base interest rate (last month's rate);
    /// - TB is the percentage paid by the bank over CDI.
    ///
    /// This formula is applied iteratively using a for loop to simulate compound growth.
    /// </summary>
    /// <param name="initialValue">Initial investment value (VI)</param>
    /// <param name="months">Number of months the investment is applied</param>
    /// <returns>Gross final value (VF)</returns>
    public static double Gross(double initialValue, uint months)
    {
        double balance = initialValue;
        double monthlyRate = Cdb.CDI * Cdb.TB;

        for (int i = 0; i < months; i++)
        {
            balance += balance * monthlyRate; // equivalent to: balance *= (1 + monthlyRate)
        }

        return balance;
    }

    /// <summary>
    /// Calculates the taxes applicable to a CDB investment based on the number of months held and the initial and final values.
    /// </summary>
    /// <param name="months"></param>
    /// <param name="vf"></param>
    /// <param name="vi"></param>
    /// <returns></returns>
    public static double Taxes(uint months, double vf, double vi)
    {
        double balance = vf - vi;
        double aliquota = months switch
        {
            < 6 => 0.225,
            <= 12 => 0.20,
            <= 24 => 0.175,
            _ => 0.15
        };

        return balance * aliquota;
    }
}

using Domain.Common.Exceptions.CDBCalculator;
using Domain.Business.Records;
using Infrastructure.Common.Interfaces;

namespace Infrastructure.Services.CDBCalculator;

public class CdbCalculator : ICdbCalculator
{
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

    private static double Gross(double initialValue, uint months)
    {
        double balance = initialValue;
        double monthlyRate = Cdb.CDI * Cdb.TB;

        for (int i = 0; i < months; i++)
        {
            balance += balance * monthlyRate;
        }

        return balance;
    }


    private static double Taxes(uint months, double vf, double vi)
    {
        double rendimento = vf - vi;
        double aliquota = months switch
        {
            <= 6 => 0.225,
            <= 12 => 0.20,
            <= 24 => 0.175,
            _ => 0.15
        };

        return rendimento * aliquota;
    }
}

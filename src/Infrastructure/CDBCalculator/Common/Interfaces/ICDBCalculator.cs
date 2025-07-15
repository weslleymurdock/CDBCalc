using Domain.Business.Records;

namespace Infrastructure.Common.Interfaces;

public interface ICdbCalculator
{
    public (double totalGross, double totalNet) Calculate(Cdb title);
}

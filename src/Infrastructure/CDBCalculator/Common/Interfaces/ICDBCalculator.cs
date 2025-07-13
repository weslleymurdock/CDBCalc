using Domain.Business.Records;

namespace Infrastructure.Common.Interfaces;

public interface ICDBCalculator
{
    public (double totalGross, double totalNet) Calculate(CDB title);

}

namespace Domain.Business.Records;

/// <summary>
/// Represents a Certificate of Deposit (CDB) with its associated interest rate and investment duration.
/// </summary>
/// <remarks>A CDB is a fixed-income investment instrument commonly used in financial markets. This type
/// encapsulates  the value of the investment (VI) and the duration of the investment in months.</remarks>
/// <param name="VI">Initial value</param>
/// <param name="Months">Months to wait</param>
public readonly record struct Cdb(double VI, uint Months)
{
    public static readonly double TB = 1.08;  // 108% como fator
    public static readonly double CDI = 0.009;  // 0.9% como decimal
}
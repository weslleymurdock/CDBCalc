namespace Domain.Business.Records;

public readonly record struct CDB(double VI, uint Months)
{
    public static readonly double TB = 1.08;  // 108% como fator
    public static readonly double CDI = 0.009;  // 0.9% como decimal
}
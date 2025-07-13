namespace CDBCalculator.Server
{
    public static class IWebHostApplicationEnvironmentExtensions
    {
        public static bool IsDocker(this IWebHostEnvironment environment) => environment.EnvironmentName.Equals("Docker", StringComparison.OrdinalIgnoreCase);
    }
}

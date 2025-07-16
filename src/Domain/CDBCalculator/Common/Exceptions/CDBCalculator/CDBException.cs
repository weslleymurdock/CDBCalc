namespace Domain.Common.Exceptions.CDBCalculator;
/// <summary>
/// Represents an exception specific to the CDB (Custom Database) operations.
/// </summary>
/// <remarks>This exception is typically thrown when an error occurs during CDB-related operations. It provides a
/// message describing the nature of the error.</remarks>
/// <param name="message">The error message that explains the reason for the exception.</param>
public class CdbException(string message) : Exception(message)
{
}

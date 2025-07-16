namespace Application.CQRS.Responses.Base;

/// <summary>
/// Represents the result of an operation, including its success status, HTTP status code, and an optional message.
/// </summary>
/// <remarks>This class serves as a base type for responses in an application, providing common properties to
/// indicate the outcome of an operation. Derived classes can extend this type to include additional details specific to
/// the operation.</remarks>
public abstract class Response
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
}

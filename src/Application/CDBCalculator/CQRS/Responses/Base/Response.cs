namespace Application.CQRS.Responses.Base;

public abstract class Response
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
}

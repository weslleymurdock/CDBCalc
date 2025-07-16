namespace CDBCalculator.ApiGateway.UnitTests.Common.Handlers;


public class CdbCalculatorMicroServiceHandler(HttpClient client) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var forwardRequest = new HttpRequestMessage(request.Method, request.RequestUri!.PathAndQuery)
        {
            Content = request.Content,
            Version = request.Version
        };

        foreach (var header in request.Headers)
        {
            forwardRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return await client.SendAsync(forwardRequest, cancellationToken);
    }
}


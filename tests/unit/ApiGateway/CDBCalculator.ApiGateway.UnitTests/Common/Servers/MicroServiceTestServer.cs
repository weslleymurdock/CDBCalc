using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace CDBCalculator.ApiGateway.UnitTests.Common.Servers;

public class MicroServiceTestServer : IDisposable
{
    private readonly TestServer _server;
    public HttpClient Client { get; }
    private bool _disposed = false;

    public MicroServiceTestServer()
    {
        var builder = new WebHostBuilder()
            .UseEnvironment("Test")
            .UseStartup<Startup>(); 

        _server = new TestServer(builder);
        Client = _server.CreateClient();
        Client.BaseAddress = new Uri("http://localhost:5010/"); 
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _server.Dispose();
                Client.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

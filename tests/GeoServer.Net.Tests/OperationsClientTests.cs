using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class OperationsClientTests
{
    [Fact]
    public async Task ResetAndReloadAsync_UsePost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Operations.ResetAsync();
            await client.Operations.ReloadAsync();
        }

        Assert.Equal("/geoserver/rest/reset", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/reload", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.All(handler.Requests, r => Assert.Equal(HttpMethod.Post, r.Method));
    }

    [Fact]
    public void LoggingCrudSync_UsesExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""logging"":{""level"":""DEFAULT_LOGGING""}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = client.Operations.GetLogging();
            client.Operations.UpdateLogging(new { logging = new { level = "DEFAULT_LOGGING" } });
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
    }

    [Fact]
    public async Task MonitoringRawEndpoints_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            _ = await client.Operations.GetMonitoringRequestsRawAsync("from=2020-01-01");
            _ = await client.Operations.GetMonitoringRequestRawAsync("123");
            await client.Operations.ClearMonitoringRequestsAsync();
        }

        Assert.Contains("/geoserver/rest/monitor/requests", handler.Requests[0].RequestUri!.AbsoluteUri);
        Assert.Equal("/geoserver/rest/monitor/requests/123", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
    }
}

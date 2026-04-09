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
            var logging = client.Operations.GetLogging();
            client.Operations.UpdateLogging(new { logging = new { level = "DEFAULT_LOGGING" } });

            Assert.Equal("DEFAULT_LOGGING", logging.LoggingTyped.Level);
            Assert.Equal("DEFAULT_LOGGING", ((geoserver.net.Models.Operations.LoggingConfigurationDto)logging.Logging!).Level);
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

    [Fact]
    public async Task MonitoringTypedEndpoints_UseJsonRoutesAndDeserialize()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("/monitor/requests.json"))
            {
                return TestHttpMessageHandler.Json(@"{""requests"":[{""id"":""1"",""path"":""/wms"",""method"":""GET""}]}");
            }

            return TestHttpMessageHandler.Json(@"{""request"":{""id"":""1"",""path"":""/wms"",""method"":""GET""}}");
        });

        using (client)
        {
            var list = await client.Operations.GetMonitoringRequestsTypedAsync("from=2020-01-01");
            var single = client.Operations.GetMonitoringRequestTyped("1");

            Assert.Single(list.Requests);
            Assert.Equal("1", single.Request.Id);
        }

        Assert.Equal("/geoserver/rest/monitor/requests.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/monitor/requests/1.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.All(handler.Requests.Take(2), request => Assert.Equal(HttpMethod.Get, request.Method));
    }
}

using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class AboutClientTests
{
    [Fact]
    public async Task GetManifestAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""about"":{}}"));
        using (client)
        {
            _ = await client.About.GetManifestAsync();
        }

        Assert.Equal("/geoserver/rest/about/manifest.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
    }

    [Fact]
    public void GetManifestSync_WithQuery_AppendsQuery()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""about"":{}}"));
        using (client)
        {
            _ = client.About.GetManifest("key=GeoServerModule&value=core");
        }

        Assert.Contains("key=GeoServerModule&value=core", handler.Requests[0].RequestUri!.Query);
    }

    [Fact]
    public async Task GetVersionAndStatusAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""about"":{}}"));
        using (client)
        {
            _ = await client.About.GetVersionAsync();
            _ = await client.About.GetStatusAsync();
        }

        Assert.Equal("/geoserver/rest/about/version.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/about/status.json", handler.Requests[1].RequestUri!.AbsolutePath);
    }

    [Fact]
    public void GetVersionAndStatusSync_UseGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""about"":{}}"));
        using (client)
        {
            _ = client.About.GetVersion();
            _ = client.About.GetStatus();
        }

        Assert.All(handler.Requests, request => Assert.Equal(HttpMethod.Get, request.Method));
    }

    [Fact]
    public async Task TypedMethods_DeserializeResources()
    {
        var json = @"{""about"":{""resource"":[{""name"":""GeoServer"",""available"":true}]}}";
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(json));
        using (client)
        {
            var manifest = await client.About.GetManifestTypedAsync();
            var version = client.About.GetVersionTyped();
            var status = await client.About.GetStatusTypedAsync();

            Assert.Single(manifest.About.Resources);
            Assert.Single(version.About.Resources);
            Assert.Single(status.About.Resources);
        }

        Assert.Equal("/geoserver/rest/about/manifest.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/about/version.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/about/status.json", handler.Requests[2].RequestUri!.AbsolutePath);
        Assert.All(handler.Requests, request => Assert.Equal(HttpMethod.Get, request.Method));
    }
}

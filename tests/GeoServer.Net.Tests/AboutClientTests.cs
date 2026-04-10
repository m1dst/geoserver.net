using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the AboutClientTests type.
/// </summary>
public sealed class AboutClientTests
{
    /// <summary>
    /// Executes the GetManifestAsync_UsesExpectedRoute operation.
    /// </summary>
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

    /// <summary>
    /// Executes the GetManifestSync_WithQuery_AppendsQuery operation.
    /// </summary>
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

    /// <summary>
    /// Executes the GetVersionAndStatusAsync_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task GetVersionAndStatusAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""about"":{}}"));
        using (client)
        {
            _ = await client.About.GetVersionAsync();
            _ = await client.About.GetStatusAsync();
            _ = await client.About.GetSystemStatusAsync();
        }

        Assert.Equal("/geoserver/rest/about/version.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/about/status.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/about/system-status.json", handler.Requests[2].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the GetVersionAndStatusSync_UseGet operation.
    /// </summary>
    [Fact]
    public void GetVersionAndStatusSync_UseGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""about"":{}}"));
        using (client)
        {
            _ = client.About.GetVersion();
            _ = client.About.GetStatus();
            _ = client.About.GetSystemStatus();
        }

        Assert.All(handler.Requests, request => Assert.Equal(HttpMethod.Get, request.Method));
    }

    /// <summary>
    /// Executes the TypedMethods_DeserializeResources operation.
    /// </summary>
    [Fact]
    public async Task TypedMethods_DeserializeResources()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""about"":{""resource"":[{""name"":""GeoServer"",""available"":true}]}}"));
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

    /// <summary>
    /// Executes the TypedMethods_DeserializeManifestAndStatusAliases operation.
    /// </summary>
    [Fact]
    public async Task TypedMethods_DeserializeManifestAndStatusAliases()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("/version.json"))
            {
                return TestHttpMessageHandler.Json(@"{""about"":{""resource"":[{""@name"":""GeoTools"",""Version"":""1.0.0""}]}}");
            }

            return TestHttpMessageHandler.Json(@"{""about"":{""status"":[{""name"":""GeoServer Main"",""isEnabled"":true,""isAvailable"":true}]}}");
        });

        using (client)
        {
            var version = await client.About.GetVersionTypedAsync();
            var status = client.About.GetStatusTyped();

            Assert.Single(version.About.Resources);
            Assert.Equal("GeoTools", version.About.Resources[0].Name);

            Assert.Single(status.About.Resources);
            Assert.Equal("GeoServer Main", status.About.Resources[0].Name);
            Assert.True(status.About.Resources[0].Enabled);
            Assert.True(status.About.Resources[0].Available);
        }

        Assert.Equal("/geoserver/rest/about/version.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/about/status.json", handler.Requests[1].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the GetSystemStatus_DeserializesMetricsAndAliases operation.
    /// </summary>
    [Fact]
    public async Task GetSystemStatus_DeserializesMetricsAndAliases()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("/about/system-status.json"))
            {
                return TestHttpMessageHandler.Json(@"{""metrics"":{""metric"":[{""name"":""PARTITION_TOTAL"",""available"":true,""value"":99614720}]}}");
            }

            return TestHttpMessageHandler.Json(@"{""about"":{}}");
        });

        using (client)
        {
            var metrics = await client.About.GetSystemStatusAsync();
            var metricsSync = client.About.GetSystemStatus();

            Assert.Single(metrics.Metrics.Metric);
            Assert.Equal("PARTITION_TOTAL", metrics.Metrics.Metric[0].Name);
            Assert.True(metrics.Metrics.Metric[0].Available);
            Assert.NotNull(metrics.Metrics.Metric[0].Value);

            Assert.Single(metricsSync.Metrics.Metric);
        }

        Assert.Equal("/geoserver/rest/about/system-status.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/about/system-status.json", handler.Requests[1].RequestUri!.AbsolutePath);
    }
}

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class ResourcesClientTests
{
    [Fact]
    public async Task GetRawAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""ResourceDirectory"":{}}"));
        using (client)
        {
            _ = await client.Resources.GetRawAsync("styles");
        }

        Assert.Equal("/geoserver/rest/resource/styles", handler.Requests.Single().RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
    }

    [Fact]
    public async Task GetMetadataAsync_UsesMetadataQuery()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""ResourceMetadata"":{}}"));
        using (client)
        {
            _ = await client.Resources.GetMetadataAsync("styles/default_point.sld");
        }

        var request = handler.Requests.Single();
        Assert.Equal("/geoserver/rest/resource/styles/default_point.sld", request.RequestUri!.AbsolutePath);
        Assert.Contains("operation=metadata", request.RequestUri.Query);
        Assert.Contains("format=json", request.RequestUri.Query);
    }

    [Fact]
    public async Task HeadAsync_UsesHead()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Resources.HeadAsync("logs");
        }

        Assert.Equal(HttpMethod.Head, handler.Requests.Single().Method);
    }

    [Fact]
    public async Task PutAsync_UsesPutAndMediaType()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Resources.PutAsync("styles/new.sld", "<StyledLayerDescriptor/>", "application/vnd.ogc.sld+xml");
        }

        var request = handler.Requests.Single();
        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.Equal("application/vnd.ogc.sld+xml", request.Content!.Headers.ContentType!.MediaType);
    }

    [Fact]
    public async Task DeleteAsync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Resources.DeleteAsync("styles/new.sld");
        }

        Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        Assert.Equal("/geoserver/rest/resource/styles/new.sld", handler.Requests.Single().RequestUri!.AbsolutePath);
    }

    [Fact]
    public void SyncMethods_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""ResourceMetadata"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = client.Resources.GetRaw("styles");
            _ = client.Resources.GetMetadata("styles/default_point.sld");
            client.Resources.Head("logs");
            client.Resources.Put("styles/new.sld", "content");
            client.Resources.Delete("styles/new.sld");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Head, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
    }
}

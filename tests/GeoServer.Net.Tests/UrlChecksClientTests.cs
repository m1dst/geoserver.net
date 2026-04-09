using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class UrlChecksClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""urlChecks"":{}}"));
        using (client)
        {
            _ = await client.UrlChecks.GetAllAsync();
        }

        Assert.Equal("/geoserver/rest/urlchecks.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
    }

    [Fact]
    public async Task GetByNameAsync_EncodesNameInRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""urlCheck"":{}}"));
        using (client)
        {
            _ = await client.UrlChecks.GetByNameAsync("my check");
        }

        Assert.Equal("/geoserver/rest/urlchecks/my%20check.json", handler.Requests.Single().RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task CrudAsync_UsesExpectedVerbsAndRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.UrlChecks.CreateAsync(new { urlCheck = new { name = "check1" } });
            await client.UrlChecks.UpdateAsync("check1", new { urlCheck = new { enabled = true } });
            await client.UrlChecks.DeleteAsync("check1");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/geoserver/rest/urlchecks", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Equal("/geoserver/rest/urlchecks/check1", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
        Assert.Equal("/geoserver/rest/urlchecks/check1", handler.Requests[2].RequestUri!.AbsolutePath);
    }

    [Fact]
    public void CrudSync_UsesExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""urlCheck"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = client.UrlChecks.GetByName("check1");
            client.UrlChecks.Create(new { urlCheck = new { name = "check1" } });
            client.UrlChecks.Update("check1", new { urlCheck = new { enabled = true } });
            client.UrlChecks.Delete("check1");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }
}

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class FontsClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""fonts"":[""Arial"",""Calibri""]}"));
        using (client)
        {
            _ = await client.Fonts.GetAllAsync();
        }

        Assert.Equal("/geoserver/rest/fonts", handler.Requests.Single().RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
    }

    [Fact]
    public void GetAllSync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""fonts"":[""Arial""]}"));
        using (client)
        {
            _ = client.Fonts.GetAll();
        }

        Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
    }
}

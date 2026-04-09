using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Wmts;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class WmtsStoresClientTests
{
    [Fact]
    public async Task CrudAsync_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return request.RequestUri!.AbsolutePath.EndsWith("wmtsstores.json")
                    ? TestHttpMessageHandler.Json(@"{""wmtsStores"":{""wmtsStore"":[]}}")
                    : TestHttpMessageHandler.Json(@"{""wmtsStore"":{""name"":""remote""}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.WmtsStores.GetAllAsync("ws1");
            _ = await client.WmtsStores.GetByNameAsync("ws1", "remote");
            await client.WmtsStores.CreateAsync("ws1", new WmtsStoreCreateRequest { Name = "remote" });
            await client.WmtsStores.UpdateAsync("ws1", "remote", new WmtsStoreCreateRequest { Name = "remote" });
            await client.WmtsStores.DeleteAsync("ws1", "remote", recurse: true);
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
        Assert.Contains("recurse=true", handler.Requests[4].RequestUri!.Query);
    }
}

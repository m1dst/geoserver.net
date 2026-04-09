using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Wms;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class WmsLayersClientTests
{
    [Fact]
    public async Task LayerOperationsAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return request.RequestUri!.AbsolutePath.Contains("/wmslayers/")
                    ? TestHttpMessageHandler.Json(@"{""wmsLayer"":{""name"":""dem""}}")
                    : TestHttpMessageHandler.Json(@"{""wmsLayers"":{""wmsLayer"":[]}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.WmsLayers.GetAllAsync("ws1");
            _ = await client.WmsLayers.GetAllForStoreAsync("ws1", "remote", listAvailable: true);
            _ = await client.WmsLayers.GetByNameAsync("ws1", "dem");
            await client.WmsLayers.CreateAsync("ws1", new WmsLayerCreateRequest { Name = "dem" });
            await client.WmsLayers.CreateForStoreAsync("ws1", "remote", new WmsLayerCreateRequest { Name = "dem" });
            await client.WmsLayers.UpdateAsync("ws1", "dem", new WmsLayerCreateRequest { Name = "dem" });
            await client.WmsLayers.DeleteAsync("ws1", "dem", recurse: true);
        }

        Assert.Contains("list=available", handler.Requests[1].RequestUri!.Query);
        Assert.Equal(HttpMethod.Post, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[6].Method);
    }
}

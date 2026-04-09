using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Wmts;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the WmtsLayersClientTests type.
/// </summary>
public sealed class WmtsLayersClientTests
{
    /// <summary>
    /// Executes the LayerOperationsAsync_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task LayerOperationsAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return request.RequestUri!.AbsolutePath.Contains("/wmtslayers/")
                    ? TestHttpMessageHandler.Json(@"{""wmtsLayer"":{""name"":""dem""}}")
                    : TestHttpMessageHandler.Json(@"{""wmtsLayers"":{""wmtsLayer"":[]}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.WmtsLayers.GetAllAsync("ws1");
            _ = await client.WmtsLayers.GetAllForStoreAsync("ws1", "remote", listAvailable: true);
            _ = await client.WmtsLayers.GetByNameAsync("ws1", "dem");
            await client.WmtsLayers.CreateAsync("ws1", new WmtsLayerCreateRequest { Name = "dem" });
            await client.WmtsLayers.CreateForStoreAsync("ws1", "remote", new WmtsLayerCreateRequest { Name = "dem" });
            await client.WmtsLayers.UpdateAsync("ws1", "dem", new WmtsLayerCreateRequest { Name = "dem" });
            await client.WmtsLayers.DeleteAsync("ws1", "dem", recurse: true);
        }

        Assert.Contains("list=available", handler.Requests[1].RequestUri!.Query);
        Assert.Equal(HttpMethod.Post, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[6].Method);
    }
}

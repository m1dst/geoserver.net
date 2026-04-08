using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Layers;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class LayersClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""layers"":{""layer"":[{""name"":""l1""}]}}"));
        using (client)
        {
            _ = await client.Layers.GetAllAsync();
            Assert.Equal("/geoserver/rest/layers.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetAll_Sync_Works()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""layers"":{""layer"":[]}}"));
        using (client)
        {
            _ = client.Layers.GetAll();
        }
    }

    [Fact]
    public async Task GetByNameAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""layer"":{""name"":""l1""}}"));
        using (client)
        {
            _ = await client.Layers.GetByNameAsync("l1");
            Assert.Equal("/geoserver/rest/layers/l1.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetByName_Sync_ReturnsLayer()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""layer"":{""name"":""l1""}}"));
        using (client)
        {
            var result = client.Layers.GetByName("l1");
            Assert.Equal("l1", result.Layer.Name);
        }
    }

    [Fact]
    public async Task UpdateAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Layers.UpdateAsync("l1", new LayerUpdateRequest { Enabled = true });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void Update_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Layers.Update("l1", new LayerUpdateRequest { Enabled = true });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task DeleteAsync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Layers.DeleteAsync("l1", recurse: true);
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Delete, request.Method);
            Assert.Contains("recurse=true", request.RequestUri!.Query);
        }
    }

    [Fact]
    public void Delete_Sync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Layers.Delete("l1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.FeatureTypes;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class FeatureTypesClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""featureTypes"":{""featureType"":[{""name"":""ft1""}]}}"));
        using (client)
        {
            _ = await client.FeatureTypes.GetAllAsync("ws1", "ds1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/datastores/ds1/featuretypes.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetAll_Sync_Works()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""featureTypes"":{""featureType"":[]}}"));
        using (client)
        {
            _ = client.FeatureTypes.GetAll("ws1", "ds1");
        }
    }

    [Fact]
    public async Task GetByNameAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""featureType"":{""name"":""ft1""}}"));
        using (client)
        {
            _ = await client.FeatureTypes.GetByNameAsync("ws1", "ds1", "ft1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/datastores/ds1/featuretypes/ft1.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetByName_Sync_ReturnsItem()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""featureType"":{""name"":""ft1""}}"));
        using (client)
        {
            var result = client.FeatureTypes.GetByName("ws1", "ds1", "ft1");
            Assert.Equal("ft1", result.FeatureType.Name);
        }
    }

    [Fact]
    public async Task CreateAsync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.FeatureTypes.CreateAsync("ws1", "ds1", new FeatureTypeCreateRequest { Name = "ft1" });
            Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void Create_Sync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            client.FeatureTypes.Create("ws1", "ds1", new FeatureTypeCreateRequest { Name = "ft1" });
            Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task UpdateAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.FeatureTypes.UpdateAsync("ws1", "ds1", "ft1", new FeatureTypeCreateRequest { Name = "ft1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void Update_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.FeatureTypes.Update("ws1", "ds1", "ft1", new FeatureTypeCreateRequest { Name = "ft1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task DeleteAsync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.FeatureTypes.DeleteAsync("ws1", "ds1", "ft1", recurse: true);
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
            client.FeatureTypes.Delete("ws1", "ds1", "ft1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

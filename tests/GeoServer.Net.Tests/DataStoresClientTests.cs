using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Stores;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class DataStoresClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesWorkspaceRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""dataStores"":{""dataStore"":[{""name"":""ds1""}]}}"));
        using (client)
        {
            _ = await client.DataStores.GetAllAsync("ws1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/datastores.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetAll_Sync_Works()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""dataStores"":{""dataStore"":[]}}"));
        using (client)
        {
            _ = client.DataStores.GetAll("ws1");
        }
    }

    [Fact]
    public async Task GetByNameAsync_UsesStoreRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""dataStore"":{""name"":""ds1""}}"));
        using (client)
        {
            _ = await client.DataStores.GetByNameAsync("ws1", "ds1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/datastores/ds1.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetByName_Sync_ReturnsStore()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""dataStore"":{""name"":""ds1""}}"));
        using (client)
        {
            var result = client.DataStores.GetByName("ws1", "ds1");
            Assert.Equal("ds1", result.DataStore.Name);
        }
    }

    [Fact]
    public async Task CreateAsync_PostsPayload()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.DataStores.CreateAsync("ws1", new DataStoreCreateRequest { Name = "ds1", Type = "PostGIS" });
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Post, request.Method);
            var payload = JObject.Parse(await request.Content!.ReadAsStringAsync());
            Assert.Equal("ds1", payload["dataStore"]!["name"]!.Value<string>());
        }
    }

    [Fact]
    public void Create_Sync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            client.DataStores.Create("ws1", new DataStoreCreateRequest { Name = "ds1" });
            Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task UpdateAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.DataStores.UpdateAsync("ws1", "ds1", new DataStoreCreateRequest { Name = "ds1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void Update_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.DataStores.Update("ws1", "ds1", new DataStoreCreateRequest { Name = "ds1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task DeleteAsync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.DataStores.DeleteAsync("ws1", "ds1", recurse: true);
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
            client.DataStores.Delete("ws1", "ds1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

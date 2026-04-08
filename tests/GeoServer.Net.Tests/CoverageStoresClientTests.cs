using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Stores;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class CoverageStoresClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesWorkspaceRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""coverageStores"":{""coverageStore"":[{""name"":""cs1""}]}}"));
        using (client)
        {
            _ = await client.CoverageStores.GetAllAsync("ws1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/coveragestores.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetAll_Sync_Works()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""coverageStores"":{""coverageStore"":[]}}"));
        using (client)
        {
            _ = client.CoverageStores.GetAll("ws1");
        }
    }

    [Fact]
    public async Task GetByNameAsync_UsesStoreRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""coverageStore"":{""name"":""cs1""}}"));
        using (client)
        {
            _ = await client.CoverageStores.GetByNameAsync("ws1", "cs1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/coveragestores/cs1.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetByName_Sync_ReturnsStore()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""coverageStore"":{""name"":""cs1""}}"));
        using (client)
        {
            var result = client.CoverageStores.GetByName("ws1", "cs1");
            Assert.Equal("cs1", result.CoverageStore.Name);
        }
    }

    [Fact]
    public async Task CreateAsync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.CoverageStores.CreateAsync("ws1", new CoverageStoreCreateRequest { Name = "cs1" });
            Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void Create_Sync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            client.CoverageStores.Create("ws1", new CoverageStoreCreateRequest { Name = "cs1" });
            Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task UpdateAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.CoverageStores.UpdateAsync("ws1", "cs1", new CoverageStoreCreateRequest { Name = "cs1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void Update_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.CoverageStores.Update("ws1", "cs1", new CoverageStoreCreateRequest { Name = "cs1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task DeleteAsync_UsesPurgeQuery()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.CoverageStores.DeleteAsync("ws1", "cs1", recurse: true, purge: true);
            var query = handler.Requests.Single().RequestUri!.Query;
            Assert.Contains("recurse=true", query);
            Assert.Contains("purge=all", query);
        }
    }

    [Fact]
    public void Delete_Sync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.CoverageStores.Delete("ws1", "cs1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

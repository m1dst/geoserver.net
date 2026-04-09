using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Coverages;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class CoveragesClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""coverages"":{""coverage"":[{""name"":""cov1""}]}}"));
        using (client)
        {
            _ = await client.Coverages.GetAllAsync("ws1", "cs1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/coveragestores/cs1/coverages.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetAll_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""coverages"":{""coverage"":[]}}"));
        using (client)
        {
            _ = client.Coverages.GetAll("ws1", "cs1");
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task GetByNameAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""coverage"":{""name"":""cov1""}}"));
        using (client)
        {
            _ = await client.Coverages.GetByNameAsync("ws1", "cs1", "cov1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/coveragestores/cs1/coverages/cov1.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetByName_Sync_ReturnsCoverage()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""coverage"":{""name"":""cov1""}}"));
        using (client)
        {
            var result = client.Coverages.GetByName("ws1", "cs1", "cov1");
            Assert.Equal("cov1", result.Coverage.Name);
        }
    }

    [Fact]
    public async Task CreateAsync_PostsCoveragePayload()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Coverages.CreateAsync("ws1", "cs1", new CoverageCreateRequest { Name = "cov1" });
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Post, request.Method);
            var payload = JObject.Parse(await request.Content!.ReadAsStringAsync());
            Assert.Equal("cov1", payload["coverage"]!["name"]!.Value<string>());
        }
    }

    [Fact]
    public void Create_Sync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            client.Coverages.Create("ws1", "cs1", new CoverageCreateRequest { Name = "cov1" });
            Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task UpdateAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Coverages.UpdateAsync("ws1", "cs1", "cov1", new CoverageCreateRequest { Name = "cov1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void Update_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Coverages.Update("ws1", "cs1", "cov1", new CoverageCreateRequest { Name = "cov1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task DeleteAsync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Coverages.DeleteAsync("ws1", "cs1", "cov1", recurse: true);
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
            client.Coverages.Delete("ws1", "cs1", "cov1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class StructuredCoveragesClientTests
{
    [Fact]
    public async Task GetIndexAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""Schema"":{}}"));
        using (client)
        {
            _ = await client.StructuredCoverages.GetIndexAsync("ws1", "cs1", "cov1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/coveragestores/cs1/coverages/cov1/index.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetIndex_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""Schema"":{}}"));
        using (client)
        {
            _ = client.StructuredCoverages.GetIndex("ws1", "cs1", "cov1");
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task GetGranulesAsync_BuildsQuery()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""type"":""FeatureCollection""}"));
        using (client)
        {
            _ = await client.StructuredCoverages.GetGranulesAsync("ws1", "cs1", "cov1", "location like '%tif%'", 10, 5);
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Contains("filter=", request.RequestUri!.Query);
            Assert.Contains("offset=10", request.RequestUri.Query);
            Assert.Contains("limit=5", request.RequestUri.Query);
        }
    }

    [Fact]
    public void GetGranules_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""type"":""FeatureCollection""}"));
        using (client)
        {
            _ = client.StructuredCoverages.GetGranules("ws1", "cs1", "cov1");
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task DeleteGranulesAsync_UsesDeleteWithParameters()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.StructuredCoverages.DeleteGranulesAsync("ws1", "cs1", "cov1", "id=1", "all", true);
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Delete, request.Method);
            Assert.Contains("purge=all", request.RequestUri!.Query);
            Assert.Contains("updateBBox=true", request.RequestUri.Query);
        }
    }

    [Fact]
    public void DeleteGranules_Sync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.StructuredCoverages.DeleteGranules("ws1", "cs1", "cov1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task GetGranuleByIdAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""type"":""FeatureCollection""}"));
        using (client)
        {
            _ = await client.StructuredCoverages.GetGranuleByIdAsync("ws1", "cs1", "cov1", "cov1.1");
            Assert.Equal("/geoserver/rest/workspaces/ws1/coveragestores/cs1/coverages/cov1/index/granules/cov1.1.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetGranuleById_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""type"":""FeatureCollection""}"));
        using (client)
        {
            _ = client.StructuredCoverages.GetGranuleById("ws1", "cs1", "cov1", "cov1.1");
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }
}

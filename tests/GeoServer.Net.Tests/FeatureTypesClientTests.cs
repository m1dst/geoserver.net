using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.FeatureTypes;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the FeatureTypesClientTests type.
/// </summary>
public sealed class FeatureTypesClientTests
{
    /// <summary>
    /// Executes the GetAllAsync_UsesExpectedRoute operation.
    /// </summary>
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

    /// <summary>
    /// Executes the GetAll_Sync_Works operation.
    /// </summary>
    [Fact]
    public void GetAll_Sync_Works()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""featureTypes"":{""featureType"":[]}}"));
        using (client)
        {
            _ = client.FeatureTypes.GetAll("ws1", "ds1");
        }
    }

    /// <summary>
    /// Executes the GetByNameAsync_UsesExpectedRoute operation.
    /// </summary>
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

    /// <summary>
    /// Executes the GetByName_Sync_ReturnsItem operation.
    /// </summary>
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

    /// <summary>
    /// Executes the CreateAsync_UsesPost operation.
    /// </summary>
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

    /// <summary>
    /// Executes the Create_Sync_UsesPost operation.
    /// </summary>
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

    /// <summary>
    /// Executes the UpdateAsync_UsesPut operation.
    /// </summary>
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

    /// <summary>
    /// Executes the Update_Sync_UsesPut operation.
    /// </summary>
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

    /// <summary>
    /// Executes the DeleteAsync_UsesDelete operation.
    /// </summary>
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

    /// <summary>
    /// Executes the Delete_Sync_UsesDelete operation.
    /// </summary>
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

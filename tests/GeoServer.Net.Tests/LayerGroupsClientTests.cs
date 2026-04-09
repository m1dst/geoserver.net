using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.LayerGroups;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the LayerGroupsClientTests type.
/// </summary>
public sealed class LayerGroupsClientTests
{
    /// <summary>
    /// Executes the GetAllAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""layerGroups"":{""layerGroup"":[{""name"":""lg1""}]}}"));
        using (client)
        {
            _ = await client.LayerGroups.GetAllAsync();
            Assert.Equal("/geoserver/rest/layergroups.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    /// <summary>
    /// Executes the GetAll_Sync_UsesGet operation.
    /// </summary>
    [Fact]
    public void GetAll_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""layerGroups"":{""layerGroup"":[]}}"));
        using (client)
        {
            _ = client.LayerGroups.GetAll();
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the GetByNameAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""layerGroup"":{""name"":""lg1""}}"));
        using (client)
        {
            _ = await client.LayerGroups.GetByNameAsync("lg1");
            Assert.Equal("/geoserver/rest/layergroups/lg1.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    /// <summary>
    /// Executes the GetByName_Sync_ReturnsItem operation.
    /// </summary>
    [Fact]
    public void GetByName_Sync_ReturnsItem()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""layerGroup"":{""name"":""lg1""}}"));
        using (client)
        {
            var result = client.LayerGroups.GetByName("lg1");
            Assert.Equal("lg1", result.LayerGroup.Name);
        }
    }

    /// <summary>
    /// Executes the CreateAsync_PostsLayerGroupPayload operation.
    /// </summary>
    [Fact]
    public async Task CreateAsync_PostsLayerGroupPayload()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.LayerGroups.CreateAsync(new LayerGroupCreateRequest { Name = "lg1", Mode = "SINGLE" });
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Post, request.Method);
            var payload = JObject.Parse(await request.Content!.ReadAsStringAsync());
            Assert.Equal("lg1", payload["layerGroup"]!["name"]!.Value<string>());
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
            client.LayerGroups.Create(new LayerGroupCreateRequest { Name = "lg1" });
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
            await client.LayerGroups.UpdateAsync("lg1", new LayerGroupCreateRequest { Name = "lg1" });
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
            client.LayerGroups.Update("lg1", new LayerGroupCreateRequest { Name = "lg1" });
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
            await client.LayerGroups.DeleteAsync("lg1", recurse: true);
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
            client.LayerGroups.Delete("lg1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

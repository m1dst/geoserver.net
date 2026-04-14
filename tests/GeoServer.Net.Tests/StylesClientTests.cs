using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeoServer.Models.Styles;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the StylesClientTests type.
/// </summary>
public sealed class StylesClientTests
{
    /// <summary>
    /// Executes the GetAllAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""styles"":{""style"":[{""name"":""s1""}]}}"));
        using (client)
        {
            _ = await client.Styles.GetAllAsync();
            Assert.Equal("/geoserver/rest/styles.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    /// <summary>
    /// Executes the GetAll_Sync_Works operation.
    /// </summary>
    [Fact]
    public void GetAll_Sync_Works()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""styles"":{""style"":[]}}"));
        using (client)
        {
            _ = client.Styles.GetAll();
        }
    }

    /// <summary>
    /// Executes the GetByNameAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""style"":{""name"":""s1""}}"));
        using (client)
        {
            _ = await client.Styles.GetByNameAsync("s1");
            Assert.Equal("/geoserver/rest/styles/s1.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    /// <summary>
    /// Executes the GetByName_Sync_ReturnsStyle operation.
    /// </summary>
    [Fact]
    public void GetByName_Sync_ReturnsStyle()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""style"":{""name"":""s1""}}"));
        using (client)
        {
            var result = client.Styles.GetByName("s1");
            Assert.Equal("s1", result.Style.Name);
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
            await client.Styles.CreateAsync(new StyleCreateRequest { Name = "s1" });
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
            client.Styles.Create(new StyleCreateRequest { Name = "s1" });
            Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the UploadSldAsync_UsesPut operation.
    /// </summary>
    [Fact]
    public async Task UploadSldAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Styles.UploadSldAsync("s1", "<StyledLayerDescriptor/>");
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Put, request.Method);
            Assert.Equal("application/vnd.ogc.sld+xml", request.Content!.Headers.ContentType!.MediaType);
        }
    }

    /// <summary>
    /// Executes the UploadSld_Sync_UsesPut operation.
    /// </summary>
    [Fact]
    public void UploadSld_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Styles.UploadSld("s1", "<StyledLayerDescriptor/>");
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
            await client.Styles.DeleteAsync("s1", recurse: true);
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
            client.Styles.Delete("s1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

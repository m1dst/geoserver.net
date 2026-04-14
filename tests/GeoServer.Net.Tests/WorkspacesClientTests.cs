using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeoServer.Models.Workspaces;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the WorkspacesClientTests type.
/// </summary>
public sealed class WorkspacesClientTests
{
    /// <summary>
    /// Executes the GetAllAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""workspaces"":{""workspace"":[{""name"":""ws1""}]}}"));
        using (client)
        {
            var result = await client.Workspaces.GetAllAsync();
            Assert.Single(result.Workspaces.Workspace);
            Assert.Equal("/geoserver/rest/workspaces.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    /// <summary>
    /// Executes the GetAll_Sync_UsesExpectedVerb operation.
    /// </summary>
    [Fact]
    public void GetAll_Sync_UsesExpectedVerb()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""workspaces"":{""workspace"":[]}}"));
        using (client)
        {
            _ = client.Workspaces.GetAll();
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the GetByNameAsync_EncodesWorkspaceName operation.
    /// </summary>
    [Fact]
    public async Task GetByNameAsync_EncodesWorkspaceName()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""workspace"":{""name"":""top p""}}"));
        using (client)
        {
            _ = await client.Workspaces.GetByNameAsync("top p");
            Assert.Equal("/geoserver/rest/workspaces/top%20p.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    /// <summary>
    /// Executes the GetByName_Sync_Works operation.
    /// </summary>
    [Fact]
    public void GetByName_Sync_Works()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""workspace"":{""name"":""topp""}}"));
        using (client)
        {
            var result = client.Workspaces.GetByName("topp");
            Assert.Equal("topp", result.Workspace.Name);
        }
    }

    /// <summary>
    /// Executes the CreateAsync_PostsWorkspacePayload operation.
    /// </summary>
    [Fact]
    public async Task CreateAsync_PostsWorkspacePayload()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Workspaces.CreateAsync(new WorkspaceCreateRequest { Name = "wsnew" });
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("/geoserver/rest/workspaces", request.RequestUri!.AbsolutePath);
            var payload = JObject.Parse(await request.Content!.ReadAsStringAsync());
            Assert.Equal("wsnew", payload["workspace"]!["name"]!.Value<string>());
        }
    }

    /// <summary>
    /// Executes the Create_Sync_PostsWorkspacePayload operation.
    /// </summary>
    [Fact]
    public void Create_Sync_PostsWorkspacePayload()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            client.Workspaces.Create(new WorkspaceCreateRequest { Name = "wsnew" });
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
            await client.Workspaces.UpdateAsync("ws1", new WorkspaceCreateRequest { Name = "ws1" });
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
            client.Workspaces.Update("ws1", new WorkspaceCreateRequest { Name = "ws1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the DeleteAsync_UsesDeleteWithRecurse operation.
    /// </summary>
    [Fact]
    public async Task DeleteAsync_UsesDeleteWithRecurse()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Workspaces.DeleteAsync("ws1", recurse: true);
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
            client.Workspaces.Delete("ws1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

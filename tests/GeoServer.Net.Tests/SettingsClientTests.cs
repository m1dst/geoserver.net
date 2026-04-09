using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the SettingsClientTests type.
/// </summary>
public sealed class SettingsClientTests
{
    /// <summary>
    /// Executes the GetGlobalAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetGlobalAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""global"":{}}"));
        using (client)
        {
            _ = await client.Settings.GetGlobalAsync();
            Assert.Equal("/geoserver/rest/settings.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    /// <summary>
    /// Executes the GetGlobal_Sync_UsesGet operation.
    /// </summary>
    [Fact]
    public void GetGlobal_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""global"":{}}"));
        using (client)
        {
            _ = client.Settings.GetGlobal();
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the UpdateGlobalAsync_UsesPut operation.
    /// </summary>
    [Fact]
    public async Task UpdateGlobalAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Settings.UpdateGlobalAsync(new { global = new { settings = new { verbose = true } } });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the UpdateGlobal_Sync_UsesPut operation.
    /// </summary>
    [Fact]
    public void UpdateGlobal_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Settings.UpdateGlobal(new { global = new { settings = new { verbose = true } } });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the GetContactAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetContactAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""contact"":{}}"));
        using (client)
        {
            _ = await client.Settings.GetContactAsync();
            Assert.Equal("/geoserver/rest/settings/contact.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    /// <summary>
    /// Executes the GetContact_Sync_UsesGet operation.
    /// </summary>
    [Fact]
    public void GetContact_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""contact"":{}}"));
        using (client)
        {
            _ = client.Settings.GetContact();
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the UpdateContactAsync_UsesPut operation.
    /// </summary>
    [Fact]
    public async Task UpdateContactAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Settings.UpdateContactAsync(new { contact = new { contactEmail = "admin@example.com" } });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the UpdateContact_Sync_UsesPut operation.
    /// </summary>
    [Fact]
    public void UpdateContact_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Settings.UpdateContact(new { contact = new { contactEmail = "admin@example.com" } });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    /// <summary>
    /// Executes the WorkspaceSettingsCrud_UsesExpectedRoutesAndVerbs operation.
    /// </summary>
    [Fact]
    public async Task WorkspaceSettingsCrud_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""settings"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.Settings.GetWorkspaceAsync("ws1");
            await client.Settings.CreateWorkspaceAsync("ws1", new { settings = new { verbose = false } });
            await client.Settings.UpdateWorkspaceAsync("ws1", new { settings = new { verbose = true } });
            await client.Settings.DeleteWorkspaceAsync("ws1");

            Assert.Equal("/geoserver/rest/workspaces/ws1/settings.json", handler.Requests[0].RequestUri!.AbsolutePath);
            Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
            Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
            Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
        }
    }

    /// <summary>
    /// Executes the WorkspaceSettingsSyncCrud_UsesExpectedVerbs operation.
    /// </summary>
    [Fact]
    public void WorkspaceSettingsSyncCrud_UsesExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""settings"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = client.Settings.GetWorkspace("ws1");
            client.Settings.CreateWorkspace("ws1", new { settings = new { verbose = false } });
            client.Settings.UpdateWorkspace("ws1", new { settings = new { verbose = true } });
            client.Settings.DeleteWorkspace("ws1");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }
}

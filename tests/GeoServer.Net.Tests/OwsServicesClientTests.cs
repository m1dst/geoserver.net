using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class OwsServicesClientTests
{
    [Fact]
    public async Task GetGlobalAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""wms"":{}}"));
        using (client)
        {
            _ = await client.OwsServices.GetGlobalAsync("wms");
            Assert.Equal("/geoserver/rest/services/wms/settings.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetGlobal_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""wms"":{}}"));
        using (client)
        {
            _ = client.OwsServices.GetGlobal("wms");
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task UpdateGlobalAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.OwsServices.UpdateGlobalAsync("wms", new { wms = new { enabled = true } });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void UpdateGlobal_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.OwsServices.UpdateGlobal("wms", new { wms = new { enabled = true } });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task WorkspaceSettingsCrud_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""wms"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.OwsServices.GetWorkspaceAsync("wms", "ws1");
            await client.OwsServices.UpdateWorkspaceAsync("wms", "ws1", new { wms = new { enabled = false } });
            await client.OwsServices.DeleteWorkspaceAsync("wms", "ws1");
        }

        Assert.Equal("/geoserver/rest/services/wms/workspaces/ws1/settings.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
    }

    [Fact]
    public void WorkspaceSettingsSyncCrud_UsesExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""wms"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = client.OwsServices.GetWorkspace("wms", "ws1");
            client.OwsServices.UpdateWorkspace("wms", "ws1", new { wms = new { enabled = false } });
            client.OwsServices.DeleteWorkspace("wms", "ws1");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
    }

    [Fact]
    public async Task InvalidServiceType_Throws()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""wms"":{}}"));
        using (client)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => client.OwsServices.GetGlobalAsync("invalid-service"));
        }
    }
}

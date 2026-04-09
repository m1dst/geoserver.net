using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class GeoWebCacheClientTests
{
    [Fact]
    public async Task GetIndexRawAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            _ = await client.GeoWebCache.GetIndexRawAsync();
        }

        Assert.Equal("/geoserver/gwc/rest", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
    }

    [Fact]
    public async Task ReloadAsync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.GeoWebCache.ReloadAsync("reload_configuration=1");
        }

        Assert.Equal("/geoserver/gwc/rest/reload", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
    }

    [Fact]
    public async Task GlobalCrudAsync_UsesExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""global"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.GeoWebCache.GetGlobalAsync();
            await client.GeoWebCache.UpdateGlobalAsync(new { global = new { backendTimeout = 60 } });
        }

        Assert.Equal("/geoserver/gwc/rest/global.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/geoserver/gwc/rest/global", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
    }

    [Fact]
    public void SyncMethods_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""global"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = client.GeoWebCache.GetIndexRaw();
            client.GeoWebCache.Reload();
            _ = client.GeoWebCache.GetGlobal();
            client.GeoWebCache.UpdateGlobal(new { global = new { runtimeStatsEnabled = true } });
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
    }

    [Fact]
    public async Task LayersCrudAsync_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""layers"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = await client.GeoWebCache.GetLayersAsync();
            _ = await client.GeoWebCache.GetLayerAsync("ws:roads");
            await client.GeoWebCache.PutLayerAsync("ws:roads", new { layer = new { enabled = true } });
            await client.GeoWebCache.DeleteLayerAsync("ws:roads");
        }

        Assert.Equal("/geoserver/gwc/rest/layers.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/layers/ws%3Aroads.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal("/geoserver/gwc/rest/layers/ws%3Aroads", handler.Requests[2].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }

    [Fact]
    public async Task SeedEndpointsAsync_UseExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""long-array-array"":[]}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = await client.GeoWebCache.GetSeedStatusesAsync();
            _ = await client.GeoWebCache.GetLayerSeedStatusAsync("ws:roads");
            await client.GeoWebCache.SeedLayerAsync("ws:roads", new { seedRequest = new { name = "ws:roads", type = "seed" } });
        }

        Assert.Equal("/geoserver/gwc/rest/seed.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/seed/ws%3Aroads.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal("/geoserver/gwc/rest/seed/ws%3Aroads.json", handler.Requests[2].RequestUri!.AbsolutePath);
    }

    [Fact]
    public void LayerAndSeedSyncMethods_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                if (request.RequestUri!.AbsolutePath.EndsWith("/seed.json"))
                {
                    return TestHttpMessageHandler.Json(@"{""long-array-array"":[]}");
                }

                return TestHttpMessageHandler.Json(@"{""layers"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = client.GeoWebCache.GetLayers();
            _ = client.GeoWebCache.GetLayer("ws:roads");
            client.GeoWebCache.PutLayer("ws:roads", new { layer = new { enabled = true } });
            client.GeoWebCache.DeleteLayer("ws:roads");
            _ = client.GeoWebCache.GetSeedStatuses();
            _ = client.GeoWebCache.GetLayerSeedStatus("ws:roads");
            client.GeoWebCache.SeedLayer("ws:roads", new { seedRequest = new { name = "ws:roads", type = "seed" } });
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[6].Method);
    }

    [Fact]
    public async Task MassTruncateEndpointsAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json("<massTruncateRequests/>");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.GeoWebCache.GetMassTruncateCapabilitiesRawAsync();
            await client.GeoWebCache.MassTruncateAsync("truncateLayer", "ws:roads");
        }

        Assert.Equal("/geoserver/gwc/rest/masstruncate", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/geoserver/gwc/rest/masstruncate", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Contains("requestType=truncateLayer", handler.Requests[1].RequestUri!.Query);
        Assert.Contains("layer=ws%3Aroads", handler.Requests[1].RequestUri!.Query);
    }

    [Fact]
    public async Task DiskQuotaCrudAsync_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""org.geowebcache.diskquota.DiskQuotaConfig"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.GeoWebCache.GetDiskQuotaAsync();
            await client.GeoWebCache.UpdateDiskQuotaAsync(new { enabled = true, cacheCleanUpFrequency = 10 });
        }

        Assert.Equal("/geoserver/gwc/rest/diskquota.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal("/geoserver/gwc/rest/diskquota", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
    }

    [Fact]
    public void MaintenanceSyncMethods_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                if (request.RequestUri!.AbsolutePath.EndsWith("diskquota.json"))
                {
                    return TestHttpMessageHandler.Json(@"{""org.geowebcache.diskquota.DiskQuotaConfig"":{}}");
                }

                return TestHttpMessageHandler.Json("<massTruncateRequests/>");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = client.GeoWebCache.GetMassTruncateCapabilitiesRaw();
            client.GeoWebCache.MassTruncate("truncateOrphans");
            _ = client.GeoWebCache.GetDiskQuota();
            client.GeoWebCache.UpdateDiskQuota(new { enabled = false });
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
    }
}

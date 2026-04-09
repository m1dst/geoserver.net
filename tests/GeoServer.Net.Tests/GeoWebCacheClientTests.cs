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

    [Fact]
    public async Task BlobStoresCrudAsync_UsesExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""blobStores"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = await client.GeoWebCache.GetBlobStoresAsync();
            _ = await client.GeoWebCache.GetBlobStoreAsync("defaultCache");
            await client.GeoWebCache.PutBlobStoreAsync("defaultCache", new { id = "defaultCache", enabled = true });
            await client.GeoWebCache.DeleteBlobStoreAsync("defaultCache");
        }

        Assert.Equal("/geoserver/gwc/rest/blobstores.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/blobstores/defaultCache.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }

    [Fact]
    public async Task GridSetsCrudAsync_UsesExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""gridSets"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = await client.GeoWebCache.GetGridSetsAsync();
            _ = await client.GeoWebCache.GetGridSetAsync("EPSG:4326");
            await client.GeoWebCache.PutGridSetAsync("EPSG:4326", new { name = "EPSG:4326" });
            await client.GeoWebCache.DeleteGridSetAsync("EPSG:4326");
        }

        Assert.Equal("/geoserver/gwc/rest/gridsets.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/gridsets/EPSG%3A4326.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }

    [Fact]
    public async Task BoundsEndpointsAsync_UseExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            _ = await client.GeoWebCache.GetBoundsRawAsync("ws:roads", "EPSG:4326");
        }

        Assert.Equal("/geoserver/gwc/rest/bounds/ws%3Aroads/EPSG%3A4326/java", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
    }

    [Fact]
    public void AdvancedSyncMethods_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                if (request.RequestUri!.AbsolutePath.Contains("/bounds/"))
                {
                    return TestHttpMessageHandler.NoContent();
                }

                if (request.RequestUri.AbsolutePath.Contains("/gridsets"))
                {
                    return TestHttpMessageHandler.Json(@"{""gridSets"":{}}");
                }

                return TestHttpMessageHandler.Json(@"{""blobStores"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = client.GeoWebCache.GetBlobStores();
            _ = client.GeoWebCache.GetBlobStore("defaultCache");
            client.GeoWebCache.PutBlobStore("defaultCache", new { id = "defaultCache" });
            client.GeoWebCache.DeleteBlobStore("defaultCache");
            _ = client.GeoWebCache.GetGridSets();
            _ = client.GeoWebCache.GetGridSet("EPSG:4326");
            client.GeoWebCache.PutGridSet("EPSG:4326", new { name = "EPSG:4326" });
            client.GeoWebCache.DeleteGridSet("EPSG:4326");
            _ = client.GeoWebCache.GetBoundsRaw("ws:roads", "EPSG:4326");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[6].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[7].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[8].Method);
    }

    [Fact]
    public async Task TypedMethods_UseExpectedJsonRoutesAndDeserialize()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            var path = request.RequestUri!.AbsolutePath;
            if (path.EndsWith("/global.json"))
            {
                return TestHttpMessageHandler.Json(@"{""global"":{""enabled"":true}}");
            }

            if (path.EndsWith("/layers.json"))
            {
                return TestHttpMessageHandler.Json(@"{""layers"":{""name"":""ws:roads""}}");
            }

            if (path.Contains("/layers/"))
            {
                return TestHttpMessageHandler.Json(@"{""layers"":{""name"":""ws:roads""}}");
            }

            if (path.Contains("/seed/") || path.EndsWith("/seed.json"))
            {
                return TestHttpMessageHandler.Json(@"{""long-array-array"":[[1,2,3]]}");
            }

            if (path.EndsWith("/diskquota.json"))
            {
                return TestHttpMessageHandler.Json(@"{""org.geowebcache.diskquota.DiskQuotaConfig"":{""enabled"":true}}");
            }

            if (path.Contains("/blobstores/"))
            {
                return TestHttpMessageHandler.Json(@"{""blobStores"":{""name"":""defaultCache""}}");
            }

            if (path.EndsWith("/blobstores.json"))
            {
                return TestHttpMessageHandler.Json(@"{""blobStores"":{""names"":[""defaultCache""]}}");
            }

            if (path.Contains("/gridsets/"))
            {
                return TestHttpMessageHandler.Json(@"{""gridSets"":{""name"":""EPSG:4326""}}");
            }

            return TestHttpMessageHandler.Json(@"{""gridSets"":{""names"":[""EPSG:4326""]}}");
        });

        using (client)
        {
            var global = await client.GeoWebCache.GetGlobalTypedAsync();
            var layers = client.GeoWebCache.GetLayersTyped();
            var layer = await client.GeoWebCache.GetLayerTypedAsync("ws:roads");
            var seedStatuses = client.GeoWebCache.GetSeedStatusesTyped();
            var layerSeedStatus = await client.GeoWebCache.GetLayerSeedStatusTypedAsync("ws:roads");
            var diskQuota = await client.GeoWebCache.GetDiskQuotaTypedAsync();
            var blobStores = client.GeoWebCache.GetBlobStoresTyped();
            var blobStore = await client.GeoWebCache.GetBlobStoreTypedAsync("defaultCache");
            var gridSets = await client.GeoWebCache.GetGridSetsTypedAsync();
            var gridSet = client.GeoWebCache.GetGridSetTyped("EPSG:4326");

            Assert.True(global.Global.Enabled);
            Assert.Equal("ws:roads", layers.Layers.Name);
            Assert.Equal("ws:roads", layer.Layers.Name);
            Assert.Single(seedStatuses.LongArrayArray);
            Assert.Single(layerSeedStatus.LongArrayArray);
            Assert.True(diskQuota.DiskQuotaConfig.Enabled);
            Assert.Single(blobStores.BlobStores.Names);
            Assert.Equal("defaultCache", blobStore.BlobStores.Name);
            Assert.Single(gridSets.GridSets.Names);
            Assert.Equal("EPSG:4326", gridSet.GridSets.Name);
        }

        Assert.Equal("/geoserver/gwc/rest/global.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/layers.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/layers/ws%3Aroads.json", handler.Requests[2].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/seed.json", handler.Requests[3].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/seed/ws%3Aroads.json", handler.Requests[4].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/diskquota.json", handler.Requests[5].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/blobstores.json", handler.Requests[6].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/blobstores/defaultCache.json", handler.Requests[7].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/gridsets.json", handler.Requests[8].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/gwc/rest/gridsets/EPSG%3A4326.json", handler.Requests[9].RequestUri!.AbsolutePath);
        Assert.All(handler.Requests, request => Assert.Equal(HttpMethod.Get, request.Method));
    }
}

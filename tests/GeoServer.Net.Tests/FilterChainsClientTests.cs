using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Security;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the FilterChainsClientTests type.
/// </summary>
public sealed class FilterChainsClientTests
{
    /// <summary>
    /// Executes the GetAllAndGetByNameAsync_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task GetAllAndGetByNameAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""filterchain"":{""filters"":[]}}"));
        using (client)
        {
            _ = await client.FilterChains.GetAllAsync();
            _ = await client.FilterChains.GetByNameAsync("web");
        }

        Assert.Equal("/geoserver/rest/security/filterchain", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/security/filterchain/web", handler.Requests[1].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the MutationsAsync_UseExpectedVerbs operation.
    /// </summary>
    [Fact]
    public async Task MutationsAsync_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            var payload = new
            {
                filters = new Dictionary<string, object?>
                {
                    ["@name"] = "t-new"
                }
            };

            await client.FilterChains.CreateAsync(payload);
            await client.FilterChains.UpdateAsync("t-new", payload, position: 2);
            await client.FilterChains.DeleteAsync("t-new");
            await client.FilterChains.SetOrderAsync(new FilterChainsOrderRequest { Order = { "web", "default" } });
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Contains("position=2", handler.Requests[1].RequestUri!.Query);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
        Assert.Equal("/geoserver/rest/security/filterchain/order", handler.Requests[3].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the SyncCalls_UseExpectedVerbs operation.
    /// </summary>
    [Fact]
    public void SyncCalls_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""filterchain"":{""filters"":[]}}"));
        using (client)
        {
            _ = client.FilterChains.GetAll();
            _ = client.FilterChains.GetByName("web");
        }

        Assert.All(handler.Requests, r => Assert.Equal(HttpMethod.Get, r.Method));
    }

    /// <summary>
    /// Executes the Update_WithNegativePosition_Throws operation.
    /// </summary>
    [Fact]
    public async Task Update_WithNegativePosition_Throws()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            var payload = new
            {
                filters = new Dictionary<string, object?>
                {
                    ["@name"] = "web"
                }
            };

            await Assert.ThrowsAsync<System.ArgumentOutOfRangeException>(
                () => client.FilterChains.UpdateAsync("web", payload, position: -1));
        }
    }
}

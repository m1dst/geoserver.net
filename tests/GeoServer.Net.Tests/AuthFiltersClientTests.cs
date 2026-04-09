using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the AuthFiltersClientTests type.
/// </summary>
public sealed class AuthFiltersClientTests
{
    /// <summary>
    /// Executes the GetAllAndGetByNameAsync_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task GetAllAndGetByNameAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""authfilters"":{""authfilter"":[]}}"));
        using (client)
        {
            _ = await client.AuthFilters.GetAllAsync();
            _ = await client.AuthFilters.GetByNameAsync("anonymous");
        }

        Assert.Equal("/geoserver/rest/security/authfilters", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/security/authfilters/anonymous", handler.Requests[1].RequestUri!.AbsolutePath);
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
            await client.AuthFilters.CreateAsync(new { authfilter = new { name = "myfilter" } });
            await client.AuthFilters.UpdateAsync("myfilter", new { authfilter = new { name = "myfilter" } });
            await client.AuthFilters.DeleteAsync("myfilter");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
    }

    /// <summary>
    /// Executes the SyncCalls_UseExpectedVerbs operation.
    /// </summary>
    [Fact]
    public void SyncCalls_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""authfilters"":{""authfilter"":[]}}"));
        using (client)
        {
            _ = client.AuthFilters.GetAll();
            _ = client.AuthFilters.GetByName("anonymous");
        }

        Assert.All(handler.Requests, r => Assert.Equal(HttpMethod.Get, r.Method));
    }
}

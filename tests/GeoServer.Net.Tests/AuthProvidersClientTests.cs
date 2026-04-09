using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Security;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the AuthProvidersClientTests type.
/// </summary>
public sealed class AuthProvidersClientTests
{
    /// <summary>
    /// Executes the GetAllAndGetByNameAsync_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task GetAllAndGetByNameAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""authproviders"":[]}"));
        using (client)
        {
            _ = await client.AuthProviders.GetAllAsync();
            _ = await client.AuthProviders.GetByNameAsync("default");
        }

        Assert.Equal("/geoserver/rest/security/authproviders", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/security/authproviders/default", handler.Requests[1].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the MutationsAsync_UseExpectedVerbsAndPositionQuery operation.
    /// </summary>
    [Fact]
    public async Task MutationsAsync_UseExpectedVerbsAndPositionQuery()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.AuthProviders.CreateAsync(new { authprovider = new { name = "ldap", className = "x" } }, position: 0);
            await client.AuthProviders.UpdateAsync("ldap", new { authprovider = new { name = "ldap" } }, position: 1);
            await client.AuthProviders.SetOrderAsync(new AuthProvidersOrderRequest { Order = { "default", "ldap" } });
            await client.AuthProviders.DeleteAsync("ldap");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Contains("position=0", handler.Requests[0].RequestUri!.Query);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Contains("position=1", handler.Requests[1].RequestUri!.Query);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal("/geoserver/rest/security/authproviders/order", handler.Requests[2].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }

    /// <summary>
    /// Executes the SyncCalls_UseExpectedVerbs operation.
    /// </summary>
    [Fact]
    public void SyncCalls_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""authproviders"":[]}"));
        using (client)
        {
            _ = client.AuthProviders.GetAll();
            _ = client.AuthProviders.GetByName("default");
        }

        Assert.All(handler.Requests, r => Assert.Equal(HttpMethod.Get, r.Method));
    }
}

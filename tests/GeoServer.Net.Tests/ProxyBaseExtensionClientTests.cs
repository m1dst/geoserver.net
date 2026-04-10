using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the ProxyBaseExtensionClientTests type.
/// </summary>
public sealed class ProxyBaseExtensionClientTests
{
    /// <summary>
    /// Executes the GetEndpointsAsync_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task GetEndpointsAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("/proxy-base-ext.json"))
            {
                return TestHttpMessageHandler.Json(@"{""ProxyBaseExtensionRules"":{""ProxyBaseExtensionRule"":[]}}");
            }

            return TestHttpMessageHandler.Json(@"{""ProxyBaseExtensionRule"":{""id"":""rule-1"",""activated"":true}}");
        });

        using (client)
        {
            var all = await client.ProxyBaseExtension.GetAllAsync();
            var single = await client.ProxyBaseExtension.GetByIdAsync("rule-1");

            Assert.NotNull(all.Payload);
            Assert.NotNull(single.Payload);
        }

        Assert.Equal("/geoserver/rest/proxy-base-ext.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/proxy-base-ext/rules/rule-1.json", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.All(handler.Requests, request => Assert.Equal(HttpMethod.Get, request.Method));
    }

    /// <summary>
    /// Executes the CrudAsync_UsesExpectedVerbsAndRoutes operation.
    /// </summary>
    [Fact]
    public async Task CrudAsync_UsesExpectedVerbsAndRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            var payload = new
            {
                ProxyBaseExtensionRule = new
                {
                    activated = true,
                    position = 0,
                    transformer = "http://localhost:8080/geoserver",
                    matcher = ".*"
                }
            };

            await client.ProxyBaseExtension.CreateAsync(payload);
            await client.ProxyBaseExtension.UpdateAsync("rule-1", payload);
            await client.ProxyBaseExtension.DeleteAsync("rule-1");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/geoserver/rest/proxy-base-ext", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Equal("/geoserver/rest/proxy-base-ext/rules/rule-1", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
        Assert.Equal("/geoserver/rest/proxy-base-ext/rules/rule-1", handler.Requests[2].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the CrudSync_UsesExpectedVerbs operation.
    /// </summary>
    [Fact]
    public void CrudSync_UsesExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""ProxyBaseExtensionRule"":{""id"":""rule-1""}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = client.ProxyBaseExtension.GetById("rule-1");
            client.ProxyBaseExtension.Create(new { ProxyBaseExtensionRule = new { matcher = ".*" } });
            client.ProxyBaseExtension.Update("rule-1", new { ProxyBaseExtensionRule = new { matcher = ".*/wms.*" } });
            client.ProxyBaseExtension.Delete("rule-1");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }

    /// <summary>
    /// Executes the EncodeValidation_ThrowsForBlankId operation.
    /// </summary>
    [Fact]
    public async Task EncodeValidation_ThrowsForBlankId()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await Assert.ThrowsAsync<System.ArgumentException>(() => client.ProxyBaseExtension.GetByIdAsync(" "));
            Assert.Throws<System.ArgumentException>(() => client.ProxyBaseExtension.Delete(string.Empty));
        }
    }
}

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using geoserver.net.Models.Namespaces;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class NamespacesClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""namespaces"":{""namespace"":[{""name"":""ns1""}]}}"));
        using (client)
        {
            _ = await client.Namespaces.GetAllAsync();
            Assert.Equal("/geoserver/rest/namespaces.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetAll_Sync_UsesGet()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""namespaces"":{""namespace"":[]}}"));
        using (client)
        {
            _ = client.Namespaces.GetAll();
            Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task GetByPrefixAsync_EncodesPrefix()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""namespace"":{""prefix"":""na me"",""uri"":""http://example/ns""}}"));
        using (client)
        {
            _ = await client.Namespaces.GetByPrefixAsync("na me");
            Assert.Equal("/geoserver/rest/namespaces/na%20me.json", handler.Requests.Single().RequestUri!.AbsolutePath);
        }
    }

    [Fact]
    public void GetByPrefix_Sync_ReturnsNamespace()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""namespace"":{""prefix"":""ns1"",""uri"":""http://example/ns1""}}"));
        using (client)
        {
            var result = client.Namespaces.GetByPrefix("ns1");
            Assert.Equal("ns1", result.Namespace.Prefix);
        }
    }

    [Fact]
    public async Task CreateAsync_PostsNamespacePayload()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Namespaces.CreateAsync(new NamespaceCreateRequest { Prefix = "ns1", Uri = "http://example/ns1" });
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Post, request.Method);
            var payload = JObject.Parse(await request.Content!.ReadAsStringAsync());
            Assert.Equal("ns1", payload["namespace"]!["prefix"]!.Value<string>());
        }
    }

    [Fact]
    public void Create_Sync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            client.Namespaces.Create(new NamespaceCreateRequest { Prefix = "ns1", Uri = "http://example/ns1" });
            Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task UpdateAsync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Namespaces.UpdateAsync("ns1", new NamespaceCreateRequest { Prefix = "ns1", Uri = "http://example/ns1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public void Update_Sync_UsesPut()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Namespaces.Update("ns1", new NamespaceCreateRequest { Prefix = "ns1", Uri = "http://example/ns1" });
            Assert.Equal(HttpMethod.Put, handler.Requests.Single().Method);
        }
    }

    [Fact]
    public async Task DeleteAsync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            await client.Namespaces.DeleteAsync("ns1", recurse: true);
            var request = handler.Requests.Single();
            Assert.Equal(HttpMethod.Delete, request.Method);
            Assert.Contains("recurse=true", request.RequestUri!.Query);
        }
    }

    [Fact]
    public void Delete_Sync_UsesDelete()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent());
        using (client)
        {
            client.Namespaces.Delete("ns1");
            Assert.Equal(HttpMethod.Delete, handler.Requests.Single().Method);
        }
    }
}

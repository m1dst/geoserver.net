using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

public sealed class ImporterClientTests
{
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""imports"":[]}"));
        using (client)
        {
            _ = await client.Importer.GetAllAsync();
        }

        Assert.Equal("/geoserver/rest/imports", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("expand=none", handler.Requests[0].RequestUri!.Query);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
    }

    [Fact]
    public async Task CreateAsync_UsesPostAndQueryParams()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Importer.CreateAsync(new { import = new { targetWorkspace = "ws1" } }, exec: true, runAsync: true, expand: "self");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/geoserver/rest/imports", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("exec=true", handler.Requests[0].RequestUri!.Query);
        Assert.Contains("async=true", handler.Requests[0].RequestUri!.Query);
    }

    [Fact]
    public async Task ImportByIdCrudAsync_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""import"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = await client.Importer.GetByIdAsync("12");
            await client.Importer.CreateOrExecuteAsync("12", new { import = new { targetWorkspace = "ws1" } }, exec: false);
            await client.Importer.PutImportAsync("12", new { import = new { targetWorkspace = "ws1" } }, exec: true);
            await client.Importer.DeleteByIdAsync("12");
        }

        Assert.Equal("/geoserver/rest/imports/12", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }

    [Fact]
    public void SyncMethods_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""imports"":[]}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = client.Importer.GetAll();
            client.Importer.Create(new { import = new { targetWorkspace = "ws1" } });
            _ = client.Importer.GetById("12");
            client.Importer.CreateOrExecute("12", new { import = new { targetWorkspace = "ws1" } });
            client.Importer.PutImport("12", new { import = new { targetWorkspace = "ws1" } });
            client.Importer.DeleteById("12");
            client.Importer.DeleteAll();
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[6].Method);
    }
}

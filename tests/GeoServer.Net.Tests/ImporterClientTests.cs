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

    [Fact]
    public async Task TaskCrudAsync_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""tasks"":[]}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = await client.Importer.GetTasksAsync("12");
            await client.Importer.CreateTaskAsync("12", new { task = new { updateMode = "CREATE" } });
            _ = await client.Importer.GetTaskAsync("12", "3");
            await client.Importer.UpdateTaskAsync("12", "3", new { task = new { updateMode = "APPEND" } });
            await client.Importer.DeleteTaskAsync("12", "3");
            _ = await client.Importer.GetTaskProgressAsync("12", "3");
            _ = await client.Importer.GetTaskTargetAsync("12", "3");
            await client.Importer.UpdateTaskTargetAsync("12", "3", new { target = new { store = new { name = "ds1" } } });
            _ = await client.Importer.GetTaskLayerAsync("12", "3");
            await client.Importer.UpdateTaskLayerAsync("12", "3", new { layer = new { name = "roads" } });
        }

        Assert.Equal("/geoserver/rest/imports/12/tasks", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Post, handler.Requests[1].Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3", handler.Requests[2].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/progress", handler.Requests[5].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/target", handler.Requests[6].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/layer", handler.Requests[8].RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task DataEndpointsAsync_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""data"":{}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.Importer.GetImportDataAsync("12");
            _ = await client.Importer.GetTaskDataAsync("12", "3");
            _ = await client.Importer.GetImportDataFilesAsync("12");
            _ = await client.Importer.GetImportDataFileAsync("12", "roads.shp");
            await client.Importer.DeleteImportDataFileAsync("12", "roads.shp");
            _ = await client.Importer.GetTaskDataFilesAsync("12", "3");
            _ = await client.Importer.GetTaskDataFileAsync("12", "3", "roads.shp");
            await client.Importer.DeleteTaskDataFileAsync("12", "3", "roads.shp");
        }

        Assert.Equal("/geoserver/rest/imports/12/data", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/data", handler.Requests[1].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/imports/12/data/files", handler.Requests[2].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/imports/12/data/files/roads.shp", handler.Requests[3].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/data/files", handler.Requests[5].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/data/files/roads.shp", handler.Requests[6].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Delete, handler.Requests[7].Method);
    }
}

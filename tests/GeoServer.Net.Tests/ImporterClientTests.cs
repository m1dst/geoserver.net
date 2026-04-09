using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the ImporterClientTests type.
/// </summary>
public sealed class ImporterClientTests
{
    /// <summary>
    /// Executes the GetAllAsync_UsesExpectedRoute operation.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_UsesExpectedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""imports"":[]}"));
        using (client)
        {
            _ = await client.Importer.GetAllAsync();
            _ = await client.Importer.GetAllTypedAsync();
        }

        Assert.Equal("/geoserver/rest/imports", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Contains("expand=none", handler.Requests[0].RequestUri!.Query);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
    }

    /// <summary>
    /// Executes the CreateAsync_UsesPostAndQueryParams operation.
    /// </summary>
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

    /// <summary>
    /// Executes the ImportByIdCrudAsync_UsesExpectedRoutesAndVerbs operation.
    /// </summary>
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
            _ = await client.Importer.GetByIdTypedAsync("12");
            await client.Importer.CreateOrExecuteAsync("12", new { import = new { targetWorkspace = "ws1" } }, exec: false);
            await client.Importer.PutImportAsync("12", new { import = new { targetWorkspace = "ws1" } }, exec: true);
            await client.Importer.DeleteByIdAsync("12");
        }

        Assert.Equal("/geoserver/rest/imports/12", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
    }

    /// <summary>
    /// Executes the SyncMethods_UseExpectedVerbs operation.
    /// </summary>
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
            _ = client.Importer.GetAllTyped();
            client.Importer.Create(new { import = new { targetWorkspace = "ws1" } });
            _ = client.Importer.GetById("12");
            _ = client.Importer.GetByIdTyped("12");
            client.Importer.CreateOrExecute("12", new { import = new { targetWorkspace = "ws1" } });
            client.Importer.PutImport("12", new { import = new { targetWorkspace = "ws1" } });
            client.Importer.DeleteById("12");
            client.Importer.DeleteAll();
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[6].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[7].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[8].Method);
    }

    /// <summary>
    /// Executes the TaskCrudAsync_UsesExpectedRoutesAndVerbs operation.
    /// </summary>
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
            _ = await client.Importer.GetTasksTypedAsync("12");
            await client.Importer.CreateTaskAsync("12", new { task = new { updateMode = "CREATE" } });
            _ = await client.Importer.GetTaskAsync("12", "3");
            _ = await client.Importer.GetTaskTypedAsync("12", "3");
            await client.Importer.UpdateTaskAsync("12", "3", new { task = new { updateMode = "APPEND" } });
            await client.Importer.DeleteTaskAsync("12", "3");
            _ = await client.Importer.GetTaskProgressAsync("12", "3");
            _ = await client.Importer.GetTaskTargetAsync("12", "3");
            await client.Importer.UpdateTaskTargetAsync("12", "3", new { target = new { store = new { name = "ds1" } } });
            _ = await client.Importer.GetTaskLayerAsync("12", "3");
            await client.Importer.UpdateTaskLayerAsync("12", "3", new { layer = new { name = "roads" } });
        }

        Assert.Equal("/geoserver/rest/imports/12/tasks", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3", handler.Requests[3].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[6].Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/progress", handler.Requests[7].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/target", handler.Requests[8].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/layer", handler.Requests[10].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the DataEndpointsAsync_UseExpectedRoutes operation.
    /// </summary>
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

    /// <summary>
    /// Executes the TransformEndpointsAsync_UseExpectedRoutesAndVerbs operation.
    /// </summary>
    [Fact]
    public async Task TransformEndpointsAsync_UseExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""transforms"":[]}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = await client.Importer.GetTaskTransformsAsync("12", "3");
            _ = await client.Importer.GetTaskTransformsTypedAsync("12", "3");
            await client.Importer.CreateTaskTransformAsync("12", "3", new { type = "ReprojectTransform", target = "EPSG:4326" });
            _ = await client.Importer.GetTaskTransformAsync("12", "3", "1");
            _ = await client.Importer.GetTaskTransformTypedAsync("12", "3", "1");
            await client.Importer.UpdateTaskTransformAsync("12", "3", "1", new { type = "ReprojectTransform", target = "EPSG:3857" });
            await client.Importer.DeleteTaskTransformAsync("12", "3", "1");
        }

        Assert.Equal("/geoserver/rest/imports/12/tasks/3/transforms", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks/3/transforms/1", handler.Requests[3].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[6].Method);
    }

    /// <summary>
    /// Executes the TransformEndpointsSync_UseExpectedVerbs operation.
    /// </summary>
    [Fact]
    public void TransformEndpointsSync_UseExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""transforms"":[]}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = client.Importer.GetTaskTransforms("12", "3");
            _ = client.Importer.GetTaskTransformsTyped("12", "3");
            client.Importer.CreateTaskTransform("12", "3", new { type = "DateFormatTransform", field = "date", format = "yyyyMMdd" });
            _ = client.Importer.GetTaskTransform("12", "3", "1");
            _ = client.Importer.GetTaskTransformTyped("12", "3", "1");
            client.Importer.UpdateTaskTransform("12", "3", "1", new { type = "DateFormatTransform", field = "date", format = "yyyy-MM-dd" });
            client.Importer.DeleteTaskTransform("12", "3", "1");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[5].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[6].Method);
    }

    /// <summary>
    /// Executes the UploadTaskFileAsync_UsesPutWithBinaryContent operation.
    /// </summary>
    [Fact]
    public async Task UploadTaskFileAsync_UsesPutWithBinaryContent()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Importer.UploadTaskFileAsync("12", "roads.zip", Encoding.UTF8.GetBytes("abc"), "application/zip");
        }

        var request = handler.Requests[0];
        Assert.Equal(HttpMethod.Put, request.Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks/roads.zip", request.RequestUri!.AbsolutePath);
        Assert.Equal("application/zip", request.Content!.Headers.ContentType!.MediaType);
    }

    /// <summary>
    /// Executes the CreateTaskFromUrlAsync_UsesFormUrlEncodedPost operation.
    /// </summary>
    [Fact]
    public async Task CreateTaskFromUrlAsync_UsesFormUrlEncodedPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Importer.CreateTaskFromUrlAsync("12", "file:/tmp/data/roads.zip");
        }

        var request = handler.Requests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks", request.RequestUri!.AbsolutePath);
        Assert.Equal("application/x-www-form-urlencoded", request.Content!.Headers.ContentType!.MediaType);
    }

    /// <summary>
    /// Executes the CreateTaskMultipartAsync_UsesMultipartFormDataPost operation.
    /// </summary>
    [Fact]
    public async Task CreateTaskMultipartAsync_UsesMultipartFormDataPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Importer.CreateTaskMultipartAsync("12", "roads.zip", Encoding.UTF8.GetBytes("abc"), mediaType: "application/zip");
        }

        var request = handler.Requests[0];
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("/geoserver/rest/imports/12/tasks", request.RequestUri!.AbsolutePath);
        Assert.StartsWith("multipart/form-data", request.Content!.Headers.ContentType!.MediaType);
    }

    /// <summary>
    /// Executes the CreateTaskMultipartSync_UsesPost operation.
    /// </summary>
    [Fact]
    public void CreateTaskMultipartSync_UsesPost()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            client.Importer.CreateTaskMultipart("12", "roads.zip", Encoding.UTF8.GetBytes("abc"));
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
    }
}

using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Net.Tests;

/// <summary>
/// Represents the TransformsClientTests type.
/// </summary>
public sealed class TransformsClientTests
{
    /// <summary>
    /// Executes the GetEndpoints_UseExpectedRoutes operation.
    /// </summary>
    [Fact]
    public async Task GetEndpoints_UseExpectedRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""transforms"":{}}"));
        using (client)
        {
            _ = await client.Transforms.GetAllAsync();
            _ = await client.Transforms.GetByNameAsync("t1");
        }

        Assert.Equal("/geoserver/rest/services/wfs/transforms.json", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("/geoserver/rest/services/wfs/transforms/t1.json", handler.Requests[1].RequestUri!.AbsolutePath);
    }

    /// <summary>
    /// Executes the CrudAsync_UsesExpectedVerbsAndRoutes operation.
    /// </summary>
    [Fact]
    public async Task CrudAsync_UsesExpectedVerbsAndRoutes()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created));
        using (client)
        {
            await client.Transforms.CreateAsync(new { transform = new { name = "t1" } });
            await client.Transforms.CreateXsltAsync("<xsl:stylesheet/>", "name=t2&sourceFormat=text/xml&outputFormat=text/html&outputMimeType=text/html");
            await client.Transforms.UpdateAsync("t1", new { transform = new { outputFormat = "text/html" } });
            await client.Transforms.DeleteAsync("t1");
        }

        Assert.Equal(HttpMethod.Post, handler.Requests[0].Method);
        Assert.Equal("/geoserver/rest/services/wfs/transforms", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal("application/xslt+xml", handler.Requests[1].Content!.Headers.ContentType!.MediaType);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal("/geoserver/rest/services/wfs/transforms/t1", handler.Requests[2].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
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
                return TestHttpMessageHandler.Json(@"{""transforms"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = client.Transforms.GetAll();
            _ = client.Transforms.GetByName("t1");
            client.Transforms.Create(new { transform = new { name = "t1" } });
            client.Transforms.CreateXslt("<xsl:stylesheet/>", "name=t2&sourceFormat=text/xml");
            client.Transforms.Update("t1", new { transform = new { outputFormat = "text/html" } });
            client.Transforms.Delete("t1");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[4].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[5].Method);
    }
}

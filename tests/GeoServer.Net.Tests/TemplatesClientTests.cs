using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the TemplatesClientTests type.
/// </summary>
public sealed class TemplatesClientTests
{
    /// <summary>
    /// Executes the GetAllAsync_DefaultScope_UsesTemplatesRoute operation.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_DefaultScope_UsesTemplatesRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""templates"":{}}"));
        using (client)
        {
            _ = await client.Templates.GetAllAsync();
        }

        Assert.Equal("/geoserver/rest/templates", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
    }

    /// <summary>
    /// Executes the TemplateCrudAsync_UsesScopedRoute operation.
    /// </summary>
    [Fact]
    public async Task TemplateCrudAsync_UsesScopedRoute()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""templates"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = await client.Templates.GetTemplateRawAsync("workspaces/ws1/templates", "content");
            await client.Templates.PutTemplateAsync("workspaces/ws1/templates", "content", "hello");
            await client.Templates.DeleteTemplateAsync("workspaces/ws1/templates", "content");
        }

        Assert.Equal("/geoserver/rest/workspaces/ws1/templates/content.ftl", handler.Requests[0].RequestUri!.AbsolutePath);
        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[2].Method);
    }

    /// <summary>
    /// Executes the TemplateCrudSync_UsesExpectedVerbs operation.
    /// </summary>
    [Fact]
    public void TemplateCrudSync_UsesExpectedVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return TestHttpMessageHandler.Json(@"{""templates"":{}}");
            }

            return TestHttpMessageHandler.NoContent(System.Net.HttpStatusCode.Created);
        });

        using (client)
        {
            _ = client.Templates.GetAll("workspaces/ws1/templates");
            _ = client.Templates.GetTemplateRaw("workspaces/ws1/templates", "content");
            client.Templates.PutTemplate("workspaces/ws1/templates", "content", "abc");
            client.Templates.DeleteTemplate("workspaces/ws1/templates", "content");
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[3].Method);
    }
}

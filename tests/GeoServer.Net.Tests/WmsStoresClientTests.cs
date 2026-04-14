using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GeoServer.Models.Wms;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the WmsStoresClientTests type.
/// </summary>
public sealed class WmsStoresClientTests
{
    /// <summary>
    /// Executes the CrudAsync_UsesExpectedRoutesAndVerbs operation.
    /// </summary>
    [Fact]
    public async Task CrudAsync_UsesExpectedRoutesAndVerbs()
    {
        var (client, handler) = GeoServerClientFactory.Create(request =>
        {
            if (request.Method == HttpMethod.Get)
            {
                return request.RequestUri!.AbsolutePath.EndsWith("wmsstores.json")
                    ? TestHttpMessageHandler.Json(@"{""wmsStores"":{""wmsStore"":[]}}")
                    : TestHttpMessageHandler.Json(@"{""wmsStore"":{""name"":""remote""}}");
            }

            return TestHttpMessageHandler.NoContent();
        });

        using (client)
        {
            _ = await client.WmsStores.GetAllAsync("ws1");
            _ = await client.WmsStores.GetByNameAsync("ws1", "remote");
            await client.WmsStores.CreateAsync("ws1", new WmsStoreCreateRequest { Name = "remote" });
            await client.WmsStores.UpdateAsync("ws1", "remote", new WmsStoreCreateRequest { Name = "remote" });
            await client.WmsStores.DeleteAsync("ws1", "remote", recurse: true);
        }

        Assert.Equal(HttpMethod.Get, handler.Requests[0].Method);
        Assert.Equal(HttpMethod.Get, handler.Requests[1].Method);
        Assert.Equal(HttpMethod.Post, handler.Requests[2].Method);
        Assert.Equal(HttpMethod.Put, handler.Requests[3].Method);
        Assert.Equal(HttpMethod.Delete, handler.Requests[4].Method);
        Assert.Contains("recurse=true", handler.Requests[4].RequestUri!.Query);
    }
}

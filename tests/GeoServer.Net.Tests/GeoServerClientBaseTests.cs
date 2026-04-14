using System.Net;
using System.Threading.Tasks;
using GeoServer;
using Xunit;

namespace GeoServer.Tests;

/// <summary>
/// Represents the GeoServerClientBaseTests type.
/// </summary>
public sealed class GeoServerClientBaseTests
{
    /// <summary>
    /// Executes the ThrowsGeoServerApiException_OnErrorResponse operation.
    /// </summary>
    [Fact]
    public async Task ThrowsGeoServerApiException_OnErrorResponse()
    {
        var (client, _) = GeoServerClientFactory.Create(_ => TestHttpMessageHandler.Json(@"{""message"":""bad request""}", HttpStatusCode.BadRequest));
        using (client)
        {
            var ex = await Assert.ThrowsAsync<GeoServerApiException>(() => client.Workspaces.GetAllAsync());
            Assert.Equal(HttpStatusCode.BadRequest, ex.StatusCode);
            Assert.Contains("bad request", ex.ResponseBody);
        }
    }
}

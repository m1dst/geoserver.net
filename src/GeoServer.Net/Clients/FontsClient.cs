using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GeoServer.Models.Fonts;

namespace GeoServer.Clients;

/// <summary>
/// Wrapper around GeoServer fonts endpoint.
/// </summary>
public sealed class FontsClient : GeoServerClientBase
{
    internal FontsClient(HttpClient httpClient, GeoServerRequestContext? requestContext = null) : base(httpClient, requestContext)
    {
    }

    /// <summary>
    /// Lists installed fonts.
    /// </summary>
    public Task<FontsResponse> GetAllAsync(CancellationToken cancellationToken = default)
        => SendAsync<FontsResponse>(HttpMethod.Get, "fonts", cancellationToken: cancellationToken);

    /// <summary>
    /// Lists installed fonts (synchronous).
    /// </summary>
    public FontsResponse GetAll()
        => Send<FontsResponse>(HttpMethod.Get, "fonts");
}

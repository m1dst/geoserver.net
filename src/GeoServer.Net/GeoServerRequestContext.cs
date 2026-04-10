using System;
using System.Net.Http.Headers;

namespace geoserver.net.Clients;

internal sealed class GeoServerRequestContext
{
    public Uri BaseUri { get; set; } = default!;

    public AuthenticationHeaderValue? Authorization { get; set; }
}

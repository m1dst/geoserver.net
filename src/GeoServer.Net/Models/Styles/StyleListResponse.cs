using System.Collections.Generic;
using Newtonsoft.Json;
using geoserver.net.Models.Common;

namespace geoserver.net.Models.Styles;

/// <summary>
/// List response for styles endpoint.
/// </summary>
public sealed class StyleListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("styles")]
    public StyleListEnvelope Styles { get; set; } = new();
}

/// <summary>
/// Style list envelope.
/// </summary>
public sealed class StyleListEnvelope
{
    /// <summary>
    /// Style entries.
    /// </summary>
    [JsonProperty("style")]
    public List<NamedResourceDto> Style { get; set; } = new();
}

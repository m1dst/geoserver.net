using System.Collections.Generic;
using Newtonsoft.Json;
using GeoServer.Models.Common;

namespace GeoServer.Models.Styles;

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

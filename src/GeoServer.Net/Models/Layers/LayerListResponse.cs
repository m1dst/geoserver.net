using System.Collections.Generic;
using Newtonsoft.Json;
using GeoServer.Models.Common;

namespace GeoServer.Models.Layers;

/// <summary>
/// List response for layers endpoint.
/// </summary>
public sealed class LayerListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("layers")]
    public LayerListEnvelope Layers { get; set; } = new();
}

/// <summary>
/// Layer list envelope.
/// </summary>
public sealed class LayerListEnvelope
{
    /// <summary>
    /// Layer entries.
    /// </summary>
    [JsonProperty("layer")]
    public List<NamedResourceDto> Layer { get; set; } = new();
}

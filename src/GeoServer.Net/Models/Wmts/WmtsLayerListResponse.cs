using System.Collections.Generic;
using geoserver.net.Models.Common;
using Newtonsoft.Json;

namespace geoserver.net.Models.Wmts;

/// <summary>
/// WMTS layers list response.
/// </summary>
public sealed class WmtsLayerListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("wmtsLayers")]
    public WmtsLayerListEnvelope WmtsLayers { get; set; } = new();
}

/// <summary>
/// WMTS layers list envelope.
/// </summary>
public sealed class WmtsLayerListEnvelope
{
    /// <summary>
    /// Layer entries.
    /// </summary>
    [JsonProperty("wmtsLayer")]
    public List<NamedResourceDto> WmtsLayer { get; set; } = new();
}

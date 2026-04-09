using System.Collections.Generic;
using geoserver.net.Models.Common;
using Newtonsoft.Json;

namespace geoserver.net.Models.Wms;

/// <summary>
/// WMS layers list response.
/// </summary>
public sealed class WmsLayerListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("wmsLayers")]
    public WmsLayerListEnvelope WmsLayers { get; set; } = new();
}

/// <summary>
/// WMS layers list envelope.
/// </summary>
public sealed class WmsLayerListEnvelope
{
    /// <summary>
    /// Layer entries.
    /// </summary>
    [JsonProperty("wmsLayer")]
    public List<NamedResourceDto> WmsLayer { get; set; } = new();
}

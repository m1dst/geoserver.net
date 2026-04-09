using System.Collections.Generic;
using geoserver.net.Models.Common;
using Newtonsoft.Json;

namespace geoserver.net.Models.Wmts;

/// <summary>
/// WMTS stores list response.
/// </summary>
public sealed class WmtsStoreListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("wmtsStores")]
    public WmtsStoreListEnvelope WmtsStores { get; set; } = new();
}

/// <summary>
/// WMTS stores list envelope.
/// </summary>
public sealed class WmtsStoreListEnvelope
{
    /// <summary>
    /// Store entries.
    /// </summary>
    [JsonProperty("wmtsStore")]
    public List<NamedResourceDto> WmtsStore { get; set; } = new();
}

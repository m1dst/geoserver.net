using System.Collections.Generic;
using GeoServer.Models.Common;
using Newtonsoft.Json;

namespace GeoServer.Models.Wms;

/// <summary>
/// WMS stores list response.
/// </summary>
public sealed class WmsStoreListResponse
{
    /// <summary>
    /// Envelope from GeoServer.
    /// </summary>
    [JsonProperty("wmsStores")]
    public WmsStoreListEnvelope WmsStores { get; set; } = new();
}

/// <summary>
/// WMS stores list envelope.
/// </summary>
public sealed class WmsStoreListEnvelope
{
    /// <summary>
    /// Store entries.
    /// </summary>
    [JsonProperty("wmsStore")]
    public List<NamedResourceDto> WmsStore { get; set; } = new();
}

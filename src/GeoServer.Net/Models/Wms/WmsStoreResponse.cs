using Newtonsoft.Json;

namespace GeoServer.Models.Wms;

/// <summary>
/// Single WMS store response wrapper.
/// </summary>
public sealed class WmsStoreResponse
{
    /// <summary>
    /// Store payload.
    /// </summary>
    [JsonProperty("wmsStore")]
    public WmsStoreDto WmsStore { get; set; } = new();
}

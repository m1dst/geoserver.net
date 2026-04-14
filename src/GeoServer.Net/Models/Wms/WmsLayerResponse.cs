using Newtonsoft.Json;

namespace GeoServer.Models.Wms;

/// <summary>
/// Single WMS layer response wrapper.
/// </summary>
public sealed class WmsLayerResponse
{
    /// <summary>
    /// Layer payload.
    /// </summary>
    [JsonProperty("wmsLayer")]
    public WmsLayerDto WmsLayer { get; set; } = new();
}

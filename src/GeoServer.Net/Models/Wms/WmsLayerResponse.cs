using Newtonsoft.Json;

namespace geoserver.net.Models.Wms;

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

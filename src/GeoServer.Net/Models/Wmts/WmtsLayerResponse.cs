using Newtonsoft.Json;

namespace GeoServer.Models.Wmts;

/// <summary>
/// Single WMTS layer response wrapper.
/// </summary>
public sealed class WmtsLayerResponse
{
    /// <summary>
    /// Layer payload.
    /// </summary>
    [JsonProperty("wmtsLayer")]
    public WmtsLayerDto WmtsLayer { get; set; } = new();
}

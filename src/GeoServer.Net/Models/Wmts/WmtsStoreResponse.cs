using Newtonsoft.Json;

namespace geoserver.net.Models.Wmts;

/// <summary>
/// Single WMTS store response wrapper.
/// </summary>
public sealed class WmtsStoreResponse
{
    /// <summary>
    /// Store payload.
    /// </summary>
    [JsonProperty("wmtsStore")]
    public WmtsStoreDto WmtsStore { get; set; } = new();
}
